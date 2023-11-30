using CommandLine;

namespace Waver;

[Verb("wav2raw", HelpText = @"Convert wav audio files to 16bit pcm raw. Example: .\Waver.exe wav2raw -v -i C:\wav\in\*.wav -o C:\wav\out\*.raw")]
public class ConvertWavToRawOptions : CommonOptions
{

}