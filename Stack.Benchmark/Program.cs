using BenchmarkDotNet.Running;
using Stack.Benchmark.Benchmark;

Console.WriteLine("Running benchmarks!");

Console.WriteLine("Running .Net Stack Benchmark!");
var stackBenchmark = BenchmarkRunner.Run<StackBenchmark>();

Console.WriteLine("Running MemoryStackBenchmark!");
var memoryStackSummary = BenchmarkRunner.Run<MemoryStackBenchmark>();

Console.WriteLine("Running UnsafeMemoryStackBenchmark!");
var unsafeMemoryStackSummary = BenchmarkRunner.Run<UnsafeMemoryStackBenchmark>();

Console.WriteLine("Running FileStackBenchmark!");
var fileStackSummary = BenchmarkRunner.Run<FileStackBenchmark>();

Console.WriteLine("Benchmarks completed!");
Console.ReadKey();