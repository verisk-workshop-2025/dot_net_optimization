using BenchmarkDotNet.Running;
using SalesAnalyzer.Benchmarks;

BenchmarkRunner.Run<SalesProcessorBenchmarks>();

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();
