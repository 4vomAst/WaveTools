using NAudio.Wave;

namespace Waver;

public class RawToWavConverter : ResampleBase
{
    protected override void ProcessFile(string inputFileName, string outputFileName, CommonOptions commonOptions)
    {
        if (commonOptions is not WaveOptions waveOptions) return;
        
        if (!waveOptions.Force && File.Exists(outputFileName))
        {
            Console.WriteLine($"Skip {inputFileName}, file exists: {outputFileName}");
            return;
        }
        
        //convert raw pcm file to wav
        using(var fileStream = File.OpenRead(inputFileName))
        {
            var rawSourceWaveStream = new RawSourceWaveStream(fileStream, 
                new WaveFormat(waveOptions.SampleRate,waveOptions.Mono ? 1 : 2));
            
            WaveFileWriter.CreateWaveFile(outputFileName, rawSourceWaveStream);
        }        
        
        Console.WriteLine($"{inputFileName} -> {outputFileName}");

        if (!commonOptions.Verbose) return;
        
        PrintWaveFileFormat(outputFileName);
    }
}