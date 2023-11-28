using CommandLine;
using Waver;

try
{
    Parser.Default.ParseArguments<ResampleOptions, ConvertOptions, ConcatOptions>(args)
        .WithParsed<ResampleOptions>(options =>
        {
            var resample = new Resampler();
            resample.ResampleFiles(options);
        })
        .WithParsed<ConvertOptions>(options =>
        {
            var convert = new Converter();
            convert.ConvertFiles(options);
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