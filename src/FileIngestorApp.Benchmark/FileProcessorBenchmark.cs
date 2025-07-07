using BenchmarkDotNet.Attributes;
using FileIngestorApp.FileProcessor;
using System.IO;

namespace FileIngestorApp.Benchmark
{
    [MemoryDiagnoser]
    [SimpleJob]
    public class FileProcessorBenchmark
    {
        [Params(100, 10000)]
        public int N;

        private readonly int maxPrice = 3000;// including this one
        private readonly string filePath = @"C:\SuperMarketFilesWorkShop";
        private readonly string outputFilePathLeg = @"C:\SuperMarketFilesWorkShop\ResultLegacy";
        private readonly string outputFilePathOpt = @"C:\SuperMarketFilesWorkShop\ResultOptimized";

        private readonly LegacyFileProcessor legacyProcessor = new();
        private readonly OptimizedFileProcessor optimizedProcessor = new();
        private readonly FileGenerator fileGenerator = new();

        [GlobalSetup]
        public void Setup()
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    fileGenerator.GenerateFile(N, $"SM0{i}", filePath);
            //}
            Parallel.For(0, 10, i =>
            {
                fileGenerator.GenerateFile(N, $"SM0{i}", filePath);
            });
        }

        [Benchmark(Baseline = true)]
        public void Legacy() => legacyProcessor.ProcessBranchesData(filePath, outputFilePathLeg);

        [Benchmark]
        public void Optimized() => optimizedProcessor.ProcessBranchesData(filePath, outputFilePathOpt);
    }
}
