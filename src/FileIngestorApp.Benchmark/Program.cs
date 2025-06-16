using FileIngestorApp.Benchmark;

#if DEBUG

var summary1 = new FileProcessorBenchmark();
var x = summary1.Legacy();
Console.WriteLine("Legacy: " + x);

var y = summary1.Optimized();
Console.WriteLine("Optimized: " + y);

#else

using BenchmarkDotNet.Running;
var summary = BenchmarkRunner.Run<FileProcessorBenchmark>();

#endif