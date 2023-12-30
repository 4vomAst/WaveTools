namespace Waver;

public class RawConcater : ResampleBase
{
    protected override bool ProcessFile(string inputFileName, string outputFileName,
        CommonOptions commonOptions, int counter)
    {
        if (commonOptions is not CommonOutputOptions outputOptions) return false;

        if ((counter == 0) && !outputOptions.Force && File.Exists(outputFileName))
        {
            Console.WriteLine($"Skip {inputFileName}, file exists: {outputFileName}");
            return false;
        }
        
        using(var inputFileStream = File.OpenRead(inputFileName))
        using(var outputFileStream = File.Open(outputFileName, FileMode.Append))
        {
            var bytes = new byte[inputFileStream.Length];
            var bytesRead = inputFileStream.Read(bytes, 0, bytes.Length);
            outputFileStream.Write(bytes, 0, bytesRead);
            outputFileStream.Flush();
        }        
        
        Console.WriteLine($"{inputFileName} -> {outputFileName}");

        return true;
    }
}