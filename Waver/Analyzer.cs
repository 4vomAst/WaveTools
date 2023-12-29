using NAudio.Wave;

namespace Waver;

public class Analyzer : ResampleBase
{
    protected override void ProcessFile(string inputFileName, string outputFileName, CommonOptions commonOptions)
    {
        PrintWaveFileFormat(inputFileName);
    }
}