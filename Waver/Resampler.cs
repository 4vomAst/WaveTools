using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Waver;

public class Resampler : ResampleBase
{
    protected override void ProcessFile(string inputFileName, string outputFileName, CommonOptions resampleOptions)
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
    
}