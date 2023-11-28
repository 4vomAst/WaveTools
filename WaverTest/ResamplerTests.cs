namespace WaverTest;

[TestFixture]
public class ResamplerTests
{
    private string _tempEmptyInputDir;
    private string _tempInputDir;
    private string _tempOutputDir;
    private string _tempInputFile1;
    private string _tempInputFile2;
    private string _tempInputDummyWaveFile1;
    private string _tempInputDummyWaveFile2;

    [SetUp]
    public void Setup()
    {
        Assert.That(Path.Exists(Path.GetTempPath()), Is.True);

        _tempInputDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        _tempEmptyInputDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        _tempOutputDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        Assert.That(Path.Exists(_tempInputDir), Is.False);
        Assert.That(Path.Exists(_tempEmptyInputDir), Is.False);
        Assert.That(Path.Exists(_tempOutputDir), Is.False);

        Directory.CreateDirectory(_tempInputDir);
        Directory.CreateDirectory(_tempEmptyInputDir);
        Directory.CreateDirectory(_tempOutputDir);

        Assert.That(Path.Exists(_tempInputDir), Is.True);
        Assert.That(Path.Exists(_tempEmptyInputDir), Is.True);
        Assert.That(Path.Exists(_tempOutputDir), Is.True);

        _tempInputFile1 = Path.GetTempFileName();
        _tempInputFile2 = Path.GetTempFileName();

        Assert.That(File.Exists(_tempInputFile1), Is.True);
        Assert.That(File.Exists(_tempInputFile2), Is.True);

        _tempInputDummyWaveFile1 =
            Path.Combine(_tempInputDir, Path.GetFileName(_tempInputFile1).Replace(".tmp", ".wav"));

        _tempInputDummyWaveFile2 =
            Path.Combine(_tempInputDir, Path.GetFileName(_tempInputFile2).Replace(".tmp", ".wav"));

        File.Copy(_tempInputFile1,
            _tempInputDummyWaveFile1);
        File.Copy(_tempInputFile2,
            _tempInputDummyWaveFile2);

        Assert.That(File.Exists(_tempInputDummyWaveFile1), Is.True);
        Assert.That(File.Exists(_tempInputDummyWaveFile2), Is.True);
    }

    [TearDown]
    public void Cleanup()
    {
        Assert.That(File.Exists(_tempInputFile1), Is.True);
        Assert.That(File.Exists(_tempInputFile2), Is.True);

        File.Delete(_tempInputFile1);
        File.Delete(_tempInputFile2);

        File.Delete(_tempInputDummyWaveFile1);
        File.Delete(_tempInputDummyWaveFile2);

        Assert.That(File.Exists(_tempInputFile1), Is.False);
        Assert.That(File.Exists(_tempInputFile2), Is.False);

        Assert.That(Path.Exists(_tempInputDir), Is.True);
        Assert.That(Path.Exists(_tempEmptyInputDir), Is.True);
        Assert.That(Path.Exists(_tempOutputDir), Is.True);

        Directory.Delete(_tempInputDir);
        Directory.Delete(_tempEmptyInputDir);
        Directory.Delete(_tempOutputDir);

        Assert.That(Path.Exists(_tempInputDir), Is.False);
        Assert.That(Path.Exists(_tempEmptyInputDir), Is.False);
        Assert.That(Path.Exists(_tempOutputDir), Is.False);

        Assert.That(Path.Exists(Path.GetTempPath()), Is.True);
    }

    [Test]
    public void FileNameTests()
    {
        Assert.That(Resampler.ContainsWildcard(null!), Is.False);
        Assert.That(Resampler.ContainsWildcard(""), Is.False);
        Assert.That(Resampler.ContainsWildcard("output.pcm"), Is.False);
        Assert.That(Resampler.ContainsWildcard("test_*.pcm"), Is.True);
        Assert.Throws<ArgumentException>(() => Resampler.ContainsWildcard("test_*_*.pcm"));

        Assert.Throws<ArgumentException>(() => Resampler.GetInputFileNames(""));
        Assert.Throws<ArgumentException>(() => Resampler.GetInputFileNames(null!));
        Assert.Throws<FileNotFoundException>(() => Resampler.GetInputFileNames(_tempEmptyInputDir));
        Assert.Throws<FileNotFoundException>(() => Resampler.GetInputFileNames(Path.Combine(_tempInputDir, "*.mp3")));

        Assert.That(Resampler.GetInputFileNames(_tempInputFile1).Count(), Is.EqualTo(1));
        Assert.That(Resampler.GetInputFileNames(Path.GetTempPath()).Count(), Is.GreaterThan(1));
        Assert.That(Resampler.GetInputFileNames(_tempInputDir).Count(), Is.EqualTo(2));
        Assert.That(Resampler.GetInputFileNames(Path.Combine(_tempInputDir, "*.wav")).Count(), Is.EqualTo(2));

        Assert.Throws<ArgumentException>(() => Resampler.GetOutputFileName("", "any.wav"));
        Assert.Throws<ArgumentException>(() => Resampler.GetOutputFileName(null!, "any.wav"));
        Assert.Throws<ArgumentException>(() => Resampler.GetOutputFileName(_tempInputFile1, ""));
        Assert.Throws<ArgumentException>(() => Resampler.GetOutputFileName(_tempInputFile1, null!));

        Assert.Throws<FileNotFoundException>(() =>
            Resampler.GetOutputFileName(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), ".wav"), "*.pcm"));

        Assert.That(Resampler.GetOutputFileName(_tempInputFile1, "output.pcm"), Is.EqualTo("output.pcm"));
        Assert.That(Resampler.GetOutputFileName(_tempInputFile1, _tempOutputDir),
            Is.EqualTo(Path.Combine(_tempOutputDir, Path.GetFileName(_tempInputFile1))));
        Assert.That(Resampler.GetOutputFileName(_tempInputFile1, "*.pcm"),
            Is.EqualTo(Path.GetFileName(_tempInputFile1).Replace(".tmp", ".pcm")));
        Assert.That(Resampler.GetOutputFileName(_tempInputFile1, Path.Combine(_tempOutputDir, "*.pcm")),
            Is.EqualTo(Path.Combine(_tempOutputDir, Path.GetFileName(_tempInputFile1).Replace(".tmp", ".pcm"))));
    }
}