using BenchmarkDotNet.Attributes;
using Stack.Core.Stack;

namespace Stack.Benchmark.Benchmark;

public class MemoryStackBenchmark : AbstractStackBenchmark
{
    [GlobalSetup]
    public override void Setup()
    {
        Stack = new MemoryStack<int>();
    }
}