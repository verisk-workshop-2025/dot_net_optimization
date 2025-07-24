using BenchmarkDotNet.Attributes;
using FileIngestorApp.FileProcessor;
using System.IO;

namespace FileIngestorApp.Benchmark
{
    [SimpleJob]
    public class FileProcessorBenchmark
    {

        private readonly string filePath = @"C:\SuperMarketFilesWorkShop";
        private readonly string outputFilePathLeg = @"C:\SuperMarketFilesWorkShop\ResultLegacy";
        private readonly string outputFilePathOpt = @"C:\SuperMarketFilesWorkShop\ResultOptimized";

        private readonly LegacyFileProcessor legacyProcessor = new();
        private readonly BatchFileProcessing optimizedProcessor = new();
        private readonly FileGenerator fileGenerator = new();

        [GlobalSetup]
        public void Setup()
        {
            for (int i = 0; i < 10; i++)
            {
                fileGenerator.GenerateFile(1000, $"SM0{i}", filePath);
            }
            // Possible place to optimize the file generation using parallel processing:
        }

        
        [Benchmark(Baseline = true)]
        public void Legacy() => legacyProcessor.ProcessBranchesData(filePath, outputFilePathLeg);
     
        [Benchmark]
        public void Optimized() => optimizedProcessor.ProcessBranchesData(filePath, outputFilePathOpt);
        
    }
}
