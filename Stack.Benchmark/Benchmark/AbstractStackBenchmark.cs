using BenchmarkDotNet.Attributes;
using Stack.Benchmark.Benchmark.Config;
using Stack.Core.Stack;

namespace Stack.Benchmark.Benchmark;

[Config(typeof(BenchmarkConfig))]
public abstract class AbstractStackBenchmark : IDisposable
{
    protected IStack<int> Stack;

    public abstract void Setup();

    [Benchmark]
    public void PushPopOperations()
    {
        for (int i = 0; i < 1000; i++)
        {
            Stack.Push(i);
        }
        for (int i = 0; i < 1000; i++)
        {
            Stack.Pop();
        }
    }

    public virtual void Dispose()
    {
        Stack.Dispose();
    }
}