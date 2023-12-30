using CommandLine;

namespace Waver;

public class CommonOutputOptions : CommonOptions
{
    [Option('f', "force", Required = false, HelpText = "Overwrite existing file.")]
    public bool Force { get; set; }

    [Option('o', "output", Required = false, 
        HelpText = """
                   Output filename or mask. Examples: "c:\test\test.pcm", "c:\test\*.wav", "c:\test", "test.raw", "*.wav".
                   """)]
    public string? OutputFileMask { get; set; }
}