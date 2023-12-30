using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Waver;

public class Resampler : ResampleBase
{
    protected override bool ProcessFile(string inputFileName, string outputFileName, 
        CommonOptions commonOptions, int counter)
    {
        if (commonOptions is not ResampleOptions resampleOptions) return false;
        //verify if output file already exists
        if (!resampleOptions.Force && File.Exists(outputFileName))
        {
            Console.WriteLine($"Skip {inputFileName}, file exists: {outputFileName}");
            return true;
        }
        
        //convert wav or mp3 file to 16bit wav file with given sample rate and channel count (mono / stereo)
        using var reader = new AudioFileReader(inputFileName);
        
        ApplyNormalization(reader, resampleOptions);

        if (resampleOptions.SplitDuration != null)
        {
            SplitWaveFile(inputFileName, outputFileName, reader, resampleOptions);
        }
        else
        {
            WriteWaveFile(inputFileName, outputFileName, reader, resampleOptions);
        }

        return true;
    }
}