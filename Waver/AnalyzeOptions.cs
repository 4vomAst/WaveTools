using CommandLine;

namespace Waver;

[Verb("analyze", HelpText = @"Analyze wav audio files. Example: .\Waver.exe analyze -v -i C:\wav\in\*.wav")]
public class AnalyzeOptions : CommonOptions
{
    
}