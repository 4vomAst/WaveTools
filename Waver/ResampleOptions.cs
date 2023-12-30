using CommandLine;

namespace Waver;

[Verb("resample", HelpText = @"Resample audio files (wav, mp3) to 16bit wav. Example: .\Waver.exe resample -f -m -s 48000 -v -i C:\wav\in\*.* -o C:\wav\out\*.wav")]
public class ResampleOptions : CommonWaveOptions
{
    [Option('n', "normalization", Required = false, HelpText = "Normalize output volume to max peek db level, e.g. -3", Default = null)]
    public int? Nomalization { get; set; }
    
    [Option('d', "split_duration", Required = false, HelpText = "Split output to split_count file(s) with split_duration (seconds)", Default = null)]
    public int? SplitDuration { get; set; }
    
    [Option('c', "split_count", Required = false, HelpText = "Split output to split_count file(s) with split_duration (seconds)", Default = null)]
    public int? SplitCount { get; set; }
    
    [Option('t', "trim", Required = false, HelpText = "Trim silence", Default = false)]
    public bool Trim { get; set; }
}