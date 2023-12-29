using CommandLine;

namespace Waver;

public class WaveOptions : OutputOptions
{
    [Option('m', "mono", Required = false, HelpText = "Output to mono file", Default = false)]
    public bool Mono { get; set; }
    
    [Option('s', "sample_rate", Required = false, HelpText = "Output sample rate in Hz", Default = 16000)]
    public int SampleRate { get; set; }
    
    [Option('n', "normalization", Required = false, HelpText = "Normalize output volume to max peek db level, e.g. -3", Default = null)]
    public int? Nomalization { get; set; }
    
    [Option('d', "split_duration", Required = false, HelpText = "Split output to split_count file(s) with split_duration (seconds)", Default = null)]
    public int? SplitDuration { get; set; }
    
    [Option('c', "split_count", Required = false, HelpText = "Split output to split_count file(s) with split_duration (seconds)", Default = null)]
    public int? SplitCount { get; set; }
    
    [Option('t', "trim", Required = false, HelpText = "Trim silence", Default = false)]
    public bool Trim { get; set; }
}