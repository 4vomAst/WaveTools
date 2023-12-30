using NAudio.Wave;

namespace Waver;

public class WavToRawConverter : ResampleBase
{
    protected override bool ProcessFile(string inputFileName, string outputFileName,
        CommonOptions commonOptions, int counter)
    {
        var outputOptions = commonOptions as CommonOutputOptions;
        if (outputOptions == null) return false;
        
        //verify if output file already exists
        if (!outputOptions.Force && File.Exists(outputFileName))
        {
            Console.WriteLine($"Skip {inputFileName}, file exists: {outputFileName}");
            return true;
        }
        
        //convert wav file to raw 16bit pcm
        using (var waveFileReader = new WaveFileReader(inputFileName))
        using (var conversionStream = WaveFormatConversionStream.CreatePcmStream(waveFileReader))
        using(var fileStream = File.OpenWrite(outputFileName))
        {
            var bytes = new byte[conversionStream.Length];
            var bytesRead = conversionStream.Read(bytes, 0, bytes.Length);
            fileStream.Write(bytes, 0, bytesRead);
            fileStream.Flush();        
        }

        Console.WriteLine($"{inputFileName} -> {outputFileName}");

        return true;
    }
}