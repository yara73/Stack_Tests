using Stack.Core.Stack;

namespace Stack.Tests.SeriesAIScenarios;

public class FileStackScenarios : IDisposable
{
    private readonly string _testFilePath;
    private readonly string _indexDirectoryPath;

    public FileStackScenarios()
    {
        var guid = Guid.NewGuid().ToString();
        _testFilePath = Path.Combine("FileStackTests_Scenarios", $"stackTest_{guid}.bin");
        _indexDirectoryPath = Path.Combine("FileStackTests_Scenarios", $"indexDirectory_{guid}");

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
    public void Sequence_0123_ShouldBeValid()
    {
        using var stack = new FileStack<int>(_testFilePath);
        
        stack.Push(0);
        Assert.Equal(0, stack.Pop());
        stack.Push(1);
        Assert.Equal(1, stack.Pop());
        stack.Push(2);
        Assert.Equal(2, stack.Pop());
        stack.Push(3);
        Assert.Equal(3, stack.Pop());
        
        Assert.True(stack.IsEmpty());
    }

    [Fact]
    public void Sequence_0312_ShouldBeInvalid()
    {
        using var stack = new FileStack<int>(_testFilePath);
        
        stack.Push(0);
        Assert.Equal(0, stack.Pop());
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        Assert.Equal(3, stack.Pop());
        Action act = () => Assert.Equal(1, stack.Pop());
        
        act.Should().Throw<EqualException>();
    }
}