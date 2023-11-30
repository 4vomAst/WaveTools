using NAudio.Wave;

namespace Waver;

public class Converter : ResampleBase
{
    public void ConvertPcmToWavFiles(ConvertRawToWavOptions rawToWavOptions)
    {
        foreach (var inputFileName in GetInputFileNames(rawToWavOptions.InputFileMask))
        {
            ConvertPcmToWavFile(inputFileName, GetOutputFileName(inputFileName, rawToWavOptions.OutputFileMask, ".wav"), 
                rawToWavOptions);
        }
    }
    
    public void ConvertWavToPcmFiles(ConvertWavToRawOptions wavToRawOptions)
    {
        foreach (var inputFileName in GetInputFileNames(wavToRawOptions.InputFileMask))
        {
            ConvertWavToPcmFile(inputFileName, GetOutputFileName(inputFileName, wavToRawOptions.OutputFileMask, ".raw"), 
                wavToRawOptions);
        }
    }
    
    private void ConvertPcmToWavFile(string inputFileName, string outputFileName, ConvertRawToWavOptions rawToWavOptions)
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

    private void ConvertWavToPcmFile(string inputFileName, string outputFileName,
        ConvertWavToRawOptions wavToRawOptions)
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