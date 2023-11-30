using NAudio.Wave;

namespace Waver;

public class WavToRawConverter : ResampleBase
{
    protected override void ProcessFile(string inputFileName, string outputFileName,
        CommonOptions wavToRawOptions)
    {
        if (!wavToRawOptions.Force && File.Exists(outputFileName))
        {
            Console.WriteLine($"Skip {inputFileName}, file exists: {outputFileName}");
            return;
        }
        
        using (var waveFileReader = new WaveFileReader(inputFileName))
        using (var conversionStream = WaveFormatConversionStream.CreatePcmStream(waveFileReader))
        using(var fileStream = File.OpenWrite(outputFileName))
        {
            var bytes = new byte[conversionStream.Length];
            var bytesRead = conversionStream.Read(bytes, 0, bytes.Length);
            fileStream.Write(bytes, 0, bytesRead);
            fileStream.Flush();        }

        Console.WriteLine($"{inputFileName} -> {outputFileName}");
    }
}