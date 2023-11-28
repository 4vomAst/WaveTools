using CommandLine;

namespace Waver;

[Verb("resample", HelpText = "Resample audio files (wav, mp3) to 16bit wav")]
public class ResampleOptions : CommonOptions
{
    [Option('m', "mono", Required = false, HelpText = "Output to mono file", Default = false)]
    public bool Mono { get; set; }
    
    [Option('s', "samplerate", Required = false, HelpText = "Output sample rate in Hz", Default = 48000)]
    public int SampleRate { get; set; }
}