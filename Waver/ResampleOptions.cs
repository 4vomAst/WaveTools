using CommandLine;

namespace Waver;

[Verb("resample", HelpText = "Resample audio files (wav, mp3) to 16bit wav.")]
public class ResampleOptions : CommonOptions
{
}