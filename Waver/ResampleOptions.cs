using CommandLine;

namespace Waver;

[Verb("resample", HelpText = @"Resample audio files (wav, mp3) to 16bit wav. Example: .\Waver.exe resample -f -m -s 48000 -v -i C:\wav\in\*.* -o C:\wav\out\*.wav")]
public class ResampleOptions : CommonOptions
{
}