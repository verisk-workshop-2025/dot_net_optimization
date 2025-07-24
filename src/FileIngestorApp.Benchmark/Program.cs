using FileIngestorApp.Benchmark;

#if DEBUG

var summary1 = new FileProcessorBenchmark();


Console.WriteLine("Generating Files..");
summary1.Setup();


//Console.WriteLine("Calling.. Legacy");
//summary1.Legacy();

//Console.WriteLine("Calling..  Optimized");
//summary1.Optimized();

#else

using BenchmarkDotNet.Running;
var summary = BenchmarkRunner.Run<FileProcessorBenchmark>();

#endif