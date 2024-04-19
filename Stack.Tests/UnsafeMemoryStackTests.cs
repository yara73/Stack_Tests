using Stack.Core.Stack;

namespace Stack.Tests;

public class UnsafeMemoryStackTests
{
    [Fact]
    public void Push_IncreasesCount()
    {
        using var stack = new HighPerformanceMemoryStack<int>(2);
        stack.Push(1);

        Assert.False(stack.IsEmpty());
        Assert.Equal(1, stack.Pop());
    }

    [Fact]
    public void Pop_DecreasesCount_ReturnsLastPushedItem()
    {
        using var stack = new HighPerformanceMemoryStack<int>(2);
        stack.Push(1);
        stack.Push(2);

        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
        Assert.True(stack.IsEmpty());
    }

    [Fact]
    public void Pop_FromEmptyStack_ThrowsInvalidOperationException()
    {
        using var stack = new HighPerformanceMemoryStack<int>(1);

        var ex = Assert.Throws<InvalidOperationException>(() => stack.Pop());
        Assert.Equal("Stack is empty", ex.Message);
    }

    [Fact]
    public void Push_ResizeStack_WhenCapacityExceeded()
    {
        using var stack = new HighPerformanceMemoryStack<int>(1);
        stack.Push(1);
        stack.Push(2);

        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
        Assert.True(stack.IsEmpty());
    }

    [Fact]
    public void Print_ShouldPrintAllItemsInReverseOrder()
    {
        using var stack = new HighPerformanceMemoryStack<int>(3);
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
    public void Dispose_ReleasesMemory()
    {
        var stack = new HighPerformanceMemoryStack<int>(1);
        stack.Push(1);
        stack.Dispose();

        var ex = Assert.Throws<NullReferenceException>(() => stack.Push(2));
        Assert.Contains("Object reference not set to an instance of an object.", ex.Message);
    }
}