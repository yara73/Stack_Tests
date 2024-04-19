using Stack.Core.CompressedArray;

namespace Stack.Tests;

public class CompressedArrayTests : IDisposable
{
    private readonly string _testFilePath;
    private readonly string _indexDirectoryPath;

    public CompressedArrayTests()
    {
        var guid = Guid.NewGuid().ToString();
        _testFilePath = Path.Combine("CompressedArrayTests", $"unitTest_{guid}.bin");
        _indexDirectoryPath = Path.Combine("CompressedArrayTests", $"indexDirectory_{guid}");
        
        Directory.CreateDirectory(_indexDirectoryPath);
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
        if (Directory.Exists(_indexDirectoryPath))
            Directory.Delete(_indexDirectoryPath, true);
    }

    [Fact]
    public void WriteAndRead_ShouldRetrieveSameValue()
    {
        using var array = new CompressedArray<int>(_testFilePath, _indexDirectoryPath);
        int testValue = 12345;
        long testIndex = 0;

        array[testIndex] = testValue;
        int result = array[testIndex];
        
        result.Should().Be(testValue);
    }

    [Fact]
    public void Read_InvalidIndex_ShouldThrowIndexOutOfRangeException()
    {
        using var array = new CompressedArray<int>(_testFilePath, _indexDirectoryPath);
        
        Action act = () => { var result = array[1]; };
        
        act.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void WriteAndRead_MultipleValues_ShouldRetrieveCorrectValues()
    {
        using var array = new CompressedArray<int>(_testFilePath, _indexDirectoryPath);
        int[] values = { 1, 2, 3, 4, 5 };

        for (int i = 0; i < values.Length; i++)
            array[i] = values[i];

        for (int i = 0; i < values.Length; i++)
            array[i].Should().Be(values[i]);
    }

    [Fact]
    public void Dispose_ShouldReleaseAllResources()
    {
        var array = new CompressedArray<int>(_testFilePath, _indexDirectoryPath);
        array[0] = 1;
        
        Action act = array.Dispose;

        act.Should().NotThrow();

        File.Exists(_testFilePath).Should().BeTrue();
        File.Delete(_testFilePath);
    }
}
