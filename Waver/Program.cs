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
            resample.ResampleFiles(options);
        })
        .WithParsed<ConvertRawToWavOptions>(options =>
        {
            var convert = new Converter();
            convert.ConvertPcmToWavFiles(options);
        })
        .WithParsed<ConvertWavToRawOptions>(options =>
        {
            var convert = new Converter();
            convert.ConvertWavToPcmFiles(options);
        })
        .WithParsed<ConcatOptions>(options =>
        {
            var dummy = new Concater();
            dummy.ConcatFiles(options);
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