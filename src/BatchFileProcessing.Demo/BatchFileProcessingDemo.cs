using BatchFileProcessing.FileProcessors;

namespace BatchFileProcessing.Demo;

public class BatchFileProcessingDemo
{
    private readonly string filePath = @"C:\SuperMarketFilesWorkShop";
    private readonly string outputFilePathLeg = @"C:\SuperMarketFilesWorkShop\ResultSequential";
    private readonly string outputFilePathOpt = @"C:\SuperMarketFilesWorkShop\ResultParallel";

    private readonly SequentialFileProcessor sequentialFileProcessor = new();
    private readonly ParallelFileProcessor parallelFileProcessor = new();
    private readonly FileGenerator fileGenerator = new();

    public void GenerateFiles()
    {
        for (int i = 0; i < 10; i++)
        {
            fileGenerator.GenerateFile(1000, $"SM0{i}", filePath);
        }
        // Possible place to optimize the file generation using parallel processing:
    }

    public void ExecuteSequential() => sequentialFileProcessor.ProcessBranchesData(filePath, outputFilePathLeg);

    public void ExecuteParallel() => parallelFileProcessor.ProcessBranchesData(filePath, outputFilePathOpt);

}
