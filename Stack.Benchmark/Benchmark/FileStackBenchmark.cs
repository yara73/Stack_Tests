using BenchmarkDotNet.Attributes;
using Stack.Core.Stack;

namespace Stack.Benchmark.Benchmark;

public class FileStackBenchmark : AbstractStackBenchmark
{
    private string _testFilePath;
    private string _indexDirectoryPath;

    [GlobalSetup]
    public override void Setup()
    {
        var guid = Guid.NewGuid().ToString();
        _testFilePath = Path.Combine("FileStackBenchmark", $"stackTest_{guid}.bin");
        _indexDirectoryPath = Path.Combine("FileStackBenchmark", $"indexDirectory_{guid}");

        Directory.CreateDirectory(_indexDirectoryPath);
        
        Stack = new FileStack<int>(_testFilePath);
    }
    
    public override void Dispose()
    {
        base.Dispose();
        
        if (File.Exists(_testFilePath))
            File.Delete(_testFilePath);
        if (Directory.Exists(_indexDirectoryPath))
            Directory.Delete(_indexDirectoryPath, true);
    }
}