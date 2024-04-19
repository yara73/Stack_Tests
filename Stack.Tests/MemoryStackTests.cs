using Stack.Core.Stack;

namespace Stack.Tests;

public class MemoryStackTests
{
    [Fact]
    public void Push_ShouldAddItemToStack()
    {
        var stack = new MemoryStack<int>();

        stack.Push(10);

        stack.IsEmpty().Should().BeFalse();
    }

    [Fact]
    public void Pop_ShouldReturnLastPushedItem()
    {
        var stack = new MemoryStack<int>();
        stack.Push(20);
        stack.Push(30);
        
        var item = stack.Pop();
        
        item.Should().Be(30);
        stack.IsEmpty().Should().BeFalse();
    }

    [Fact]
    public void Pop_ShouldThrowWhenStackIsEmpty()
    {
        var stack = new MemoryStack<int>();

        var act = new Action(() => stack.Pop());

        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Stack is empty");
    }

    [Fact]
    public void Push_ShouldResizeStackWhenCapacityExceeded()
    {
        var initialCapacity = 2;
        var stack = new MemoryStack<int>(initialCapacity, 2.0f);

        for (int i = 0; i < initialCapacity + 1; i++) // Push more items than initial capacity
        {
            stack.Push(i);
        }

        stack.IsEmpty().Should().BeFalse();
    }

    [Fact]
    public void Print_ShouldOutputCorrectOrder()
    {
        var stack = new MemoryStack<int>();
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
        var stack = new MemoryStack<int>();

        stack.IsEmpty().Should().BeTrue();
    }
    
    [Fact]
    public void Push_ShouldIncreaseStackSize()
    {
        var stack = new MemoryStack<int>();
        stack.Push(10);
        stack.IsEmpty().Should().BeFalse();
    }
}