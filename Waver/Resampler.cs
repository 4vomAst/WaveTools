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

        if (waveOptions.SplitDuration != null)
        {
            SplitWaveFile(inputFileName, outputFileName, reader, waveOptions);
        }
        else
        {
            WriteWaveFile(inputFileName, outputFileName, reader, waveOptions);
        }
    }
}