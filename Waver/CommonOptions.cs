using CommandLine;

namespace Waver;

public class CommonOptions
{
    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }
    
    [Option('f', "force", Required = false, HelpText = "Overwrite existing file.")]
    public bool Force { get; set; }
    
    [Option('i', "input", Required = true, 
        HelpText = """
                   Input directory or file(s). Examples: "c:\test", "c:\test\*.wav", "c:\test\test.wav"
                   """)]
    public string? InputFileMask { get; set; }
    
    [Option('o', "output", Required = false, 
        HelpText = """
                   Output filename or mask. Examples: "c:\test\test.pcm", "c:\test\*.pcm", "c:\test", "test.pcm", "*.pcm".
                   """)]
    public string? OutputFileMask { get; set; }
    
    [Option('m', "mono", Required = false, HelpText = "Output to mono file", Default = false)]
    public bool Mono { get; set; }
    
    [Option('s', "samplerate", Required = false, HelpText = "Output sample rate in Hz", Default = 48000)]
    public int SampleRate { get; set; }
    
    [Option('n', "normalization", Required = false, HelpText = "Normalize output volume to max peek db level, e.g. -3", Default = null)]
    public int? Nomalization { get; set; }
}