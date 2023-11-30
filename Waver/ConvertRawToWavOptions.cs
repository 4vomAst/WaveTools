using CommandLine;

namespace Waver;

[Verb("raw2wav", HelpText = @"Convert 16bit raw pcm audio files to wav. Example: .\Waver.exe raw2wav -f -m -s 48000 -v -i C:\wav\raw\*.raw -o C:\wav\out\*.wav")]
public class ConvertRawToWavOptions : CommonOptions
{
}