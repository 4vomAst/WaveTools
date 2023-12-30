using NAudio.Wave;

namespace Waver;

public class Analyzer : ResampleBase
{
    protected override bool ProcessFile(string inputFileName, string outputFileName, CommonOptions commonOptions, int counter)
    {
        PrintWaveFileFormat(inputFileName);

        return true;
    }
}