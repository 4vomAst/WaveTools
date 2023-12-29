using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Waver;

public abstract class ResampleBase
{
    private const string Wildcard = "*";
    private const string WildcardRegEx = @"[\*]";
    
    public void ProcessFiles(CommonOptions commonOptions, string defaultExtension)
    {
        foreach (var inputFileName in GetInputFileNames(commonOptions.InputFileMask))
        {
            ProcessFile(inputFileName, GetOutputFileName(inputFileName, commonOptions as OutputOptions, defaultExtension), 
                commonOptions);
        }
    }

    protected abstract void ProcessFile(string inputFileName, string outputFileName, CommonOptions resampleOptions);
    
    public static IEnumerable<string> GetInputFileNames(string? inputFileMask)
    {
        if (string.IsNullOrEmpty(inputFileMask))
        {
            throw new ArgumentException("Input file mask is null or empty.");
        }
        
        if (File.Exists(inputFileMask) && !Directory.Exists(inputFileMask))
        {
            return new List<string>() { inputFileMask };
        }

        string directory;
        var searchPattern = Wildcard;

        if (Directory.Exists(inputFileMask))
        {
            directory = inputFileMask;
        }
        else
        {
            directory = Path.GetDirectoryName(inputFileMask) ?? Directory.GetCurrentDirectory();
        
            searchPattern = Path.GetFileName(inputFileMask);
        }

        var fileList = Directory.EnumerateFiles(directory, searchPattern).ToList();

        if (fileList == null || !fileList.Any()) throw new FileNotFoundException("No input file found.", inputFileMask) ;
        
        return fileList;
    }

    private static string GetOutputFileName(string inputFileName, OutputOptions? outputOptions, string defaultExtension)
    {
        return GetOutputFileName(inputFileName, outputOptions?.OutputFileMask ?? string.Empty, defaultExtension);
    }

    public static string GetOutputFileName(string inputFileName, string outputFileMask, string defaultExtension)
    {
        if (string.IsNullOrEmpty(inputFileName))
        {
            throw new ArgumentException("Input file name is null or empty");
        }
        
        if (string.IsNullOrEmpty(defaultExtension))
        {
            defaultExtension = ".wav";
        }

        if (string.IsNullOrEmpty(outputFileMask))
        {
            outputFileMask = $"*{defaultExtension}";
        }

        if (!File.Exists(inputFileName))
        {
            throw new FileNotFoundException($"Input file not found.", inputFileName);
        }
        
        if (Directory.Exists(outputFileMask))
        {
            return Path.Combine(outputFileMask, $"{Path.GetFileNameWithoutExtension(inputFileName)}{defaultExtension}");
        }

        if (!ContainsWildcard(outputFileMask))
        {
            var directory = Path.GetDirectoryName(outputFileMask);
            var filename = $"{Path.GetFileNameWithoutExtension(outputFileMask)}{defaultExtension}";

            return string.IsNullOrEmpty(directory) ? filename : Path.Combine(directory, filename);
            
        }

        return outputFileMask.Replace(Wildcard, Path.GetFileNameWithoutExtension(inputFileName));
    }

    public static bool ContainsWildcard(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;
        
        var matchCount = Regex.Matches(input, WildcardRegEx).Count;

        if (matchCount > 1) throw new ArgumentException($"{input} contains multiple wildcards");

        return matchCount == 1;
    }

    protected static void PrintWaveFileFormat(string fileName)
    {
        var waveReader = new WaveFileReader(fileName);
        var waveFormat = waveReader.WaveFormat;
        Console.WriteLine($"{fileName}: {waveFormat.Channels} channel(s), {waveFormat.SampleRate} Hz, {waveFormat.BitsPerSample} bit/sample, {waveFormat.Encoding}");
        
    }

    protected static void SplitWaveFile(string inputFileName, string outputFileName, AudioFileReader reader, WaveOptions waveOptions)
    {
        var splitDuration = TimeSpan.FromSeconds(waveOptions.SplitDuration ?? 10);
        var maxSplitCount = (int)Math.Ceiling(reader.TotalTime.TotalSeconds / splitDuration.TotalSeconds);
        var splitCount = Math.Min(waveOptions.SplitCount ?? maxSplitCount, maxSplitCount);

        for (var count = 0; count < splitCount; count++)
        {
            var extension = Path.GetExtension(outputFileName);
            var splitFileName = outputFileName.Replace(extension, $"_{count}{extension}");
            WriteWaveFile(inputFileName, splitFileName, reader, waveOptions, splitDuration);
            if (!reader.HasData()) break;
        }
    }

    protected static void WriteWaveFile(string inputFileName, string outputFileName, AudioFileReader reader, WaveOptions waveOptions, TimeSpan? duration = null)
    {
        var sampleProvider = GetResamplingProvider(reader, waveOptions);

        if (waveOptions.Trim)
        {
            var silenceDuration = reader.GetSilenceDuration();
            reader.Skip(silenceDuration);

            if (waveOptions.Verbose)
                Console.WriteLine($"Skip silence of {silenceDuration.TotalMilliseconds} ms");

            if (!reader.HasData())
            {
                Console.WriteLine($"No audio left after skipping silence of {silenceDuration.TotalMilliseconds} ms");
                return;
            }
        }

        WaveFileWriter.CreateWaveFile16(outputFileName,
            duration != null ? sampleProvider.Take(duration.Value) : sampleProvider);

        Console.WriteLine($"{inputFileName} -> {outputFileName}");

        if (!waveOptions.Verbose) return;
        
        PrintWaveFileFormat(outputFileName);
    }

    private static ISampleProvider GetResamplingProvider(ISampleProvider reader, WaveOptions resampleOptions)
    {
        var resamplingSampleProvider = new WdlResamplingSampleProvider(reader, resampleOptions.SampleRate);

        if (reader.WaveFormat.Channels == 2 && resampleOptions.Mono)
        {
            return new StereoToMonoSampleProvider(resamplingSampleProvider);
        }

        return resamplingSampleProvider;
    }

    protected static void ApplyNormalization(AudioFileReader reader, WaveOptions waveOptions)
    {
        switch (waveOptions.Nomalization)
        {
            case null:
                return;
            case > 0:
                Console.WriteLine($"Skip normalization, invalid db level {waveOptions.Nomalization.Value}, must be <= 0.");
                return;
        }

        // find the max peak
        var buffer = new float[reader.WaveFormat.SampleRate];
        int read;
        var maxSampleValue = 0.0f;
        do
        {
            read = reader.Read(buffer, 0, buffer.Length);
            for (var n = 0; n < read; n++)
            {
                var abs = Math.Abs(buffer[n]);
                if (abs < maxSampleValue) continue;
                maxSampleValue = abs;
            }
        } while (read > 0);

        reader.Position = 0;

        if (maxSampleValue is > 0.0f and < 1.0f)
        {
            var maxRequestedSampleValue = Math.Pow(10, waveOptions.Nomalization.Value / 20f);
            var volumeFactor = (float)maxRequestedSampleValue / maxSampleValue;
                
            reader.Volume = volumeFactor;
                
            if (waveOptions.Verbose)
                Console.WriteLine($"Max sample value: {maxSampleValue}, {waveOptions.Nomalization.Value} dB = {maxRequestedSampleValue} -> set volume to {volumeFactor}");
        }
        else
        {
            if (waveOptions.Verbose)
                Console.WriteLine($"Max sample value: {maxSampleValue}, do not normalize");
        }
    }
}