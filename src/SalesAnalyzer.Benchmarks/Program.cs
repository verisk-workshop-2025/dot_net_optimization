using BenchmarkDotNet.Running;
using SalesAnalyzer.Benchmarks;

//BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();

BenchmarkRunner.Run<SalesProcessorBenchmarks>();
//BenchmarkRunner.Run<SpanExampleBenchmarks>();
//BenchmarkRunner.Run<StringBuilderExampleBenchmarks>();

