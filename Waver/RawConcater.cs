namespace Waver;

public class RawConcater : ResampleBase
{
    protected override void ProcessFile(string inputFileName, string outputFileName,
        CommonOptions wavToRawOptions)
    {
        Console.WriteLine($"{inputFileName} -> {outputFileName}");
    }
}