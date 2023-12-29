using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Waver;

public class Resampler : ResampleBase
{
    protected override void ProcessFile(string inputFileName, string outputFileName, CommonOptions commonOptions)
    {
        if (commonOptions is not WaveOptions waveOptions) return;
        //verify if output file already exists
        if (!waveOptions.Force && File.Exists(outputFileName))
        {
            Console.WriteLine($"Skip {inputFileName}, file exists: {outputFileName}");
            return;
        }
        
        //convert wav or mp3 file to 16bit wav file with given sample rate and channel count (mono / stereo)
        using var reader = new AudioFileReader(inputFileName);
        
        ApplyNormalization(reader, waveOptions);

        var sampleProvider = GetResamplingProvider(reader, waveOptions);

        if (waveOptions.SplitDuration != null)
        {
            SplitWaveFile(inputFileName, outputFileName, sampleProvider, waveOptions, reader.TotalTime);
        }
        else
        {
            WriteWaveFile(inputFileName, outputFileName, sampleProvider, commonOptions);
        }
    }
    
    private static void SplitWaveFile(string inputFileName, string outputFileName, ISampleProvider sampleProvider, WaveOptions waveOptions, TimeSpan totalDuration)
    {
        var splitDuration = TimeSpan.FromSeconds(waveOptions.SplitDuration ?? 10);
        var maxSplitCount = (int)Math.Ceiling(totalDuration.TotalSeconds / splitDuration.TotalSeconds);
        var splitCount = Math.Min(waveOptions.SplitCount ?? 1, maxSplitCount);

        for (var count = 0; count < splitCount; count++)
        {
            var extension = Path.GetExtension(outputFileName);
            var splitFileName = outputFileName.Replace(extension, $"_{count}{extension}");
            WriteWaveFile(inputFileName, splitFileName, sampleProvider.Take(splitDuration), waveOptions);
        }
    }
    
    private static void WriteWaveFile(string inputFileName, string outputFileName, ISampleProvider sampleProvider, CommonOptions resampleOptions)
    {
        WaveFileWriter.CreateWaveFile16(outputFileName, sampleProvider);
        
        Console.WriteLine($"{inputFileName} -> {outputFileName}");

        if (!resampleOptions.Verbose) return;
        
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

    private static void ApplyNormalization(AudioFileReader reader, WaveOptions waveOptions)
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