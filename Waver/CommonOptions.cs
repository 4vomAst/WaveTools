using CommandLine;

namespace Waver;

public class CommonOptions
{
    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }
    
    [Option('i', "input", Required = true, 
        HelpText = """
                   Input directory or file(s). Examples: "c:\test", "c:\test\*.wav", "c:\test\test.wav"
                   """)]
    public string? InputFileMask { get; set; }
    
}