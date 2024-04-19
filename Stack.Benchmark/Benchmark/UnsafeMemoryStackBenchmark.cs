using BenchmarkDotNet.Attributes;
using Stack.Core.Stack;

namespace Stack.Benchmark.Benchmark;

public class UnsafeMemoryStackBenchmark : AbstractStackBenchmark
{
    [GlobalSetup]
    public override void Setup()
    {
        Stack = new HighPerformanceMemoryStack<int>();
    }
}