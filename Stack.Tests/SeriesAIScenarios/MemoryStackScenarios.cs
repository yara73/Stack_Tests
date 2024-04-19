using Stack.Core.Stack;

namespace Stack.Tests.SeriesAIScenarios;

public class MemoryStackScenarios
{
    [Fact]
    public void Sequence_0123_ShouldBeValid()
    {
        var stack = new MemoryStack<int>();
        
        stack.Push(0);
        stack.Pop().Should().Be(0);
        stack.Push(1);
        stack.Pop().Should().Be(1);
        stack.Push(2);
        stack.Pop().Should().Be(2);
        stack.Push(3);
        stack.Pop().Should().Be(3);
        
        stack.IsEmpty().Should().BeTrue();
    }

    [Fact]
    public void Sequence_0312_ShouldBeInvalid()
    {
        var stack = new MemoryStack<int>();
        
        stack.Push(0);
        stack.Pop().Should().Be(0);
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        stack.Pop().Should().Be(3);

        Action act = () => stack.Pop().Should().Be(1);
        
        act.Should().Throw<XunitException>();
    }
}