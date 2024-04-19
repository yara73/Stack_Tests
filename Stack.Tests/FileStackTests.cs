using Stack.Core.Stack;

namespace Stack.Tests;

public class FileStackTests : IDisposable
{
    private readonly string _testFilePath;
    private readonly string _indexDirectoryPath;

    public FileStackTests()
    {
        var guid = Guid.NewGuid().ToString();
        _testFilePath = Path.Combine("FileStackTests", $"stackTest_{guid}.bin");
        _indexDirectoryPath = Path.Combine("FileStackTests", $"indexDirectory_{guid}");

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
    public void Push_ShouldIncreaseStackSize()
    {
        using var stack = new FileStack<int>(_testFilePath);
        stack.Push(10);

        stack.IsEmpty().Should().BeFalse();
    }

    [Fact]
    public void Pop_ShouldReturnLastPushedItem()
    {
        using var stack = new FileStack<int>(_testFilePath);
        stack.Push(20);
        stack.Push(30);

        var result = stack.Pop();
        result.Should().Be(30);
        stack.IsEmpty().Should().BeFalse();
    }

    [Fact]
    public void Pop_ShouldThrowWhenStackIsEmpty()
    {
        using var stack = new FileStack<int>(_testFilePath);

        Action act = () => stack.Pop();
        act.Should().Throw<InvalidOperationException>().WithMessage("Pop from an empty stack.");
    }

    [Fact]
    public void Print_ShouldOutputCorrectOrder()
    {
        using var stack = new FileStack<int>(_testFilePath);
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        using var sw = new StringWriter();
        Console.SetOut(sw);
        stack.Print();

        var result = sw.ToString().Trim().Split('\n').ToList();
        result.RemoveAt(0);
        
        result[0].Should().Contain("3");
        result[1].Should().Contain("2");
        result[2].Should().Contain("1");
    }

    [Fact]
    public void IsEmpty_ShouldReturnTrueForNewStack()
    {
        using var stack = new FileStack<int>(_testFilePath);
        stack.IsEmpty().Should().BeTrue();
    }
}
