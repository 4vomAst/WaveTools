namespace Waver;

public class RawConcater : ResampleBase
{
    protected override void ProcessFile(string inputFileName, string outputFileName,
        CommonOptions wavToRawOptions)
    {
        using(var inputFileStream = File.OpenRead(inputFileName))
        using(var outputFileStream = File.Open(outputFileName, FileMode.Append))
        {
            var bytes = new byte[inputFileStream.Length];
            var bytesRead = inputFileStream.Read(bytes, 0, bytes.Length);
            outputFileStream.Write(bytes, 0, bytesRead);
            outputFileStream.Flush();
        }        
        
        Console.WriteLine($"{inputFileName} -> {outputFileName}");
    }
}