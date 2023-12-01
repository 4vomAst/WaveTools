using NAudio.Wave;

namespace Waver;

public class Analyzer : ResampleBase
{
    protected override void ProcessFile(string inputFileName, string outputFileName, CommonOptions resampleOptions)
    {
        var waveReader = new WaveFileReader(inputFileName);
        var waveFormat = waveReader.WaveFormat;
        Console.WriteLine($"{inputFileName}: {waveFormat.Channels} channel(s), {waveFormat.SampleRate} Hz, {waveFormat.BitsPerSample} bit/sample, {waveFormat.Encoding}");
    }
}