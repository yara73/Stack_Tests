using BenchmarkDotNet.Attributes;
using Stack.Benchmark.Benchmark.Config;

namespace Stack.Benchmark.Benchmark;

[Config(typeof(BenchmarkConfig))]
public class StackBenchmark : IDisposable
{
    protected Stack<int> Stack;

    [GlobalSetup]
    public void Setup()
    {
        Stack = new Stack<int>();
    }
    
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
        Stack.Clear();
    }
}