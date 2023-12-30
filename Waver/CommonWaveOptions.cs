using CommandLine;

namespace Waver;

public class CommonWaveOptions : CommonOutputOptions
{
    [Option('m', "mono", Required = false, HelpText = "Output to mono file", Default = false)]
    public bool Mono { get; set; }
    
    [Option('s', "sample_rate", Required = false, HelpText = "Output sample rate in Hz", Default = 16000)]
    public int SampleRate { get; set; }
}