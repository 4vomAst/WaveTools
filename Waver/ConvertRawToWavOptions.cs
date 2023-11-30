using CommandLine;

namespace Waver;

[Verb("raw2wav", HelpText = "Convert 16bit raw pcm audio files to wav.")]
public class ConvertRawToWavOptions : CommonOptions
{
}