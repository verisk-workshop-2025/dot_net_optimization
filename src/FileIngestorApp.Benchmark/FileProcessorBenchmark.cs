using BenchmarkDotNet.Attributes;
using FileIngestorApp.FileProcessor;

namespace FileIngestorApp.Benchmark
{
    [MemoryDiagnoser]
    [SimpleJob]
    public class FileProcessorBenchmark
    {
        [Params(100, 10000)]
        public int N;

        private readonly int highSalary = 3000;// including this one
        private readonly string filePath = @"C:\TestFiles\test.jl";

        private readonly LegacyFileProcessor legacyProcessor = new();
        private readonly OptimizedFileProcessor optimizedProcessor = new();
        private readonly FileGenerator fileGenerator = new();

        [GlobalSetup]
        public void Setup()
        {
            fileGenerator.GenerateFile(N, filePath);
        }

        [Benchmark(Baseline = true)]
        public int Legacy() => legacyProcessor.GetHighEarnersCount(filePath, highSalary);

        [Benchmark]
        public int Optimized() => optimizedProcessor.GetHighEarnersCount(filePath, highSalary);
    }
}
