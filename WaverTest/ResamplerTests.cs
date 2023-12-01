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
        Assert.That(ResampleBase.ContainsWildcard(null!), Is.False);
        Assert.That(ResampleBase.ContainsWildcard(""), Is.False);
        Assert.That(ResampleBase.ContainsWildcard("output.pcm"), Is.False);
        Assert.That(ResampleBase.ContainsWildcard("test_*.pcm"), Is.True);
        Assert.Throws<ArgumentException>(() => ResampleBase.ContainsWildcard("test_*_*.pcm"));

        Assert.Throws<ArgumentException>(() => ResampleBase.GetInputFileNames(""));
        Assert.Throws<ArgumentException>(() => ResampleBase.GetInputFileNames(null!));
        Assert.Throws<FileNotFoundException>(() => ResampleBase.GetInputFileNames(_tempEmptyInputDir));
        Assert.Throws<FileNotFoundException>(() => ResampleBase.GetInputFileNames(Path.Combine(_tempInputDir, "*.mp3")));

        Assert.That(ResampleBase.GetInputFileNames(_tempInputFile1).Count(), Is.EqualTo(1));
        Assert.That(ResampleBase.GetInputFileNames(Path.GetTempPath()).Count(), Is.GreaterThan(1));
        Assert.That(ResampleBase.GetInputFileNames(_tempInputDir).Count(), Is.EqualTo(2));
        Assert.That(ResampleBase.GetInputFileNames(Path.Combine(_tempInputDir, "*.wav")).Count(), Is.EqualTo(2));

        Assert.Throws<ArgumentException>(() => ResampleBase.GetOutputFileName("", "any.wav", "*.wav"));
        Assert.Throws<ArgumentException>(() => ResampleBase.GetOutputFileName(null!, "any.wav", "*.wav"));

        Assert.Throws<FileNotFoundException>(() =>
            ResampleBase.GetOutputFileName(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName(), ".wav"), "*.pcm", "*.wav"));

        Assert.That(ResampleBase.GetOutputFileName(_tempInputFile1, "output.pcm", ".wav"), Is.EqualTo("output.wav"));
        Assert.That(ResampleBase.GetOutputFileName(_tempInputFile1, _tempOutputDir, ".wav"),
            Is.EqualTo(Path.Combine(_tempOutputDir, $"{Path.GetFileNameWithoutExtension(_tempInputFile1)}.wav")));
        Assert.That(ResampleBase.GetOutputFileName(_tempInputFile1, "*.pcm", ".wav"),
            Is.EqualTo(Path.GetFileName(_tempInputFile1).Replace(".tmp", ".pcm")));
        Assert.That(ResampleBase.GetOutputFileName(_tempInputFile1, Path.Combine(_tempOutputDir, "*.pcm"), ".wav"),
            Is.EqualTo(Path.Combine(_tempOutputDir, Path.GetFileName(_tempInputFile1).Replace(".tmp", ".pcm"))));
    }
}