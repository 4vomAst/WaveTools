using NAudio.Wave;

namespace Waver;

public class RawToWavConverter : ResampleBase
{
    protected override void ProcessFile(string inputFileName, string outputFileName, CommonOptions rawToWavOptions)
    {
        if (!rawToWavOptions.Force && File.Exists(outputFileName))
        {
            Console.WriteLine($"Skip {inputFileName}, file exists: {outputFileName}");
            return;
        }
        
        using(var fileStream = File.OpenRead(inputFileName))
        {
            var s = new RawSourceWaveStream(fileStream, 
                new WaveFormat(rawToWavOptions.SampleRate,rawToWavOptions.Mono ? 1 : 2));
            WaveFileWriter.CreateWaveFile(outputFileName, s);
        }        
        
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