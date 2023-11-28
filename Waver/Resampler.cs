using System.Text.RegularExpressions;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Waver;

public static class Resampler
{
    private const string Wildcard = "*";
    private const string WildcardRegEx = @"[\*]";
    
    public static void ResampleFiles(ResampleOptions resampleOptions)
    {
        foreach (var inputFileName in GetInputFileNames(resampleOptions.InputFileMask))
        {
            ResampleFile(inputFileName, 
                GetOutputFileName(inputFileName, resampleOptions.OutputFileMask), 
                resampleOptions);
        }
    }

    public static IEnumerable<string> GetInputFileNames(string inputFileMask)
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

    public static string GetOutputFileName(string inputFileName, string outputFileMask)
    {
        if (string.IsNullOrEmpty(inputFileName))
        {
            throw new ArgumentException("Input file name is null or empty");
        }
        
        if (string.IsNullOrEmpty(outputFileMask))
        {
            throw new ArgumentException("Output file mask is null or empty");
        }

        if (!File.Exists(inputFileName))
        {
            throw new FileNotFoundException($"Input file not found.", inputFileName);
        }
        
        if (Directory.Exists(outputFileMask))
        {
            return Path.Combine(outputFileMask, Path.GetFileName(inputFileName));
        }

        if (!ContainsWildcard(outputFileMask))
        {
            return outputFileMask;
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

    private static void ResampleFile(string inputFileName, string outputFileName, ResampleOptions resampleOptions)
    {
        if (!resampleOptions.Force && File.Exists(outputFileName))
        {
            Console.WriteLine($"Skip {inputFileName}, file exists: {outputFileName}");
            return;
        }
        
        using var reader = new AudioFileReader(inputFileName);

        var resamplingSampleProvider = new WdlResamplingSampleProvider(reader, resampleOptions.SampleRate);

        ISampleProvider sampleProvider;
        
        if (reader.WaveFormat.Channels == 2 && resampleOptions.Mono)
        {
            sampleProvider = new StereoToMonoSampleProvider(resamplingSampleProvider);
        }
        else
        {
            sampleProvider = resamplingSampleProvider;
        }
        
        //WaveFileWriter.CreateWaveFile16(outputFileName, sampleProvider);
        WaveFileWriter.CreateWaveFile16(outputFileName, sampleProvider);

        if (!resampleOptions.Verbose) return;
        Console.WriteLine($"{inputFileName} -> {outputFileName}");

        try
        {
            var waveReader = new WaveFileReader(outputFileName);
            var waveFormat = waveReader.WaveFormat;
            Console.WriteLine($"  -> {waveFormat.Channels} channel(s), {waveFormat.SampleRate} Hz, {waveFormat.BitsPerSample} bit/sample, {waveFormat.Encoding}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    public static void ConvertFiles(ConvertOptions options)
    {
        
    }
    
    public static void ConcatFiles(ConcatOptions options)
    {
        
    }
}