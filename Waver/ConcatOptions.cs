using CommandLine;

namespace Waver;

[Verb("concat", HelpText = @"Concatenate (append) raw pcm audio files. Example: .\Waver.exe concat -v -i C:\wav\in\*.raw -o C:\wav\out\concat.raw")]
public class ConcatOptions : CommonOptions
{
    
}