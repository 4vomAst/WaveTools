// See https://aka.ms/new-console-template for more information


using CommandLine;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Waver;

try
{
    Parser.Default.ParseArguments<ResampleOptions, ConvertOptions, ConcatOptions>(args)
        .WithParsed<ResampleOptions>(Resampler.ResampleFiles)
        .WithParsed<ConvertOptions>(Resampler.ConvertFiles)
        .WithParsed<ConcatOptions>(Resampler.ConcatFiles);

}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"{ex.Message} {ex.FileName}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"{ex.Message}");
}


// using var reader = new AudioFileReader("input.wav");
//
// var sampleProvider = new WdlResamplingSampleProvider(reader, 48000);
//
// if (sampleProvider.WaveFormat.Channels == 2)
// {
//     var mono = new StereoToMonoSampleProvider(sampleProvider);
// }
//
// WaveFileWriter.CreateWaveFile16("blah", sampleProvider);