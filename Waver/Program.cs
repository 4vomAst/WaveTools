using CommandLine;
using Waver;

try
{
    Parser.Default.ParseArguments<ResampleOptions, 
            ConvertRawToWavOptions, 
            ConvertWavToRawOptions, 
            ConcatOptions>(args)
        .WithParsed<ResampleOptions>(options =>
        {
            var resample = new Resampler();
            resample.ProcessFiles(options, ".wav");
        })
        .WithParsed<ConvertRawToWavOptions>(options =>
        {
            var convert = new RawToWavConverter();
            convert.ProcessFiles(options, ".wav");
        })
        .WithParsed<ConvertWavToRawOptions>(options =>
        {
            var convert = new WavToRawConverter();
            convert.ProcessFiles(options, ".raw");
        })
        .WithParsed<ConcatOptions>(options =>
        {
            var rawConcater = new RawConcater();
            rawConcater.ProcessFiles(options, ".raw");
        });
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"{ex.Message} {ex.FileName}");
}
catch (ArgumentException ex)
{
    Console.WriteLine($"{ex.Message}");
}