using CommandLine;

namespace Waver;

public class OutputOptions : CommonOptions
{
    [Option('f', "force", Required = false, HelpText = "Overwrite existing file.")]
    public bool Force { get; set; }

    [Option('o', "output", Required = false, 
        HelpText = """
                   Output filename or mask. Examples: "c:\test\test.pcm", "c:\test\*.pcm", "c:\test", "test.pcm", "*.pcm".
                   """)]
    public string? OutputFileMask { get; set; }
}