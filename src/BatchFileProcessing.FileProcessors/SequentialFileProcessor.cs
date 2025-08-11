using BatchFileProcessing.Core.Contracts;

namespace BatchFileProcessing.FileProcessors;

public class SequentialFileProcessor : IFileProcessor
{
    public void ProcessBranchesData(string inputDirectory, string outputDirectory)
    {
        //var sw = Stopwatch.StartNew();
        ProcessBranchesDataWithSingleThread(inputDirectory, outputDirectory);
        //sw.Stop();
        //Console.WriteLine($"Total time With Single Thread: {sw.ElapsedMilliseconds} ms");
    }

    private void ProcessBranchesDataWithSingleThread(string inputDirectory, string outputDirectory)
    {
        string[] productFiles = Directory.GetFiles(inputDirectory, "*_products.jl");
        List<string> branchCodes = productFiles
            .Select(f => Path.GetFileName(f).Split('_')[0])
            .Distinct()
            .ToList();

        foreach (string? branchCode in branchCodes)
        {
            string result = string.Empty;
            try
            {
                DateTime start = DateTime.Now;
                result = new ProcessBatch().Execute(branchCode, inputDirectory, outputDirectory);
                Directory.CreateDirectory(outputDirectory);
                string path = Path.Combine(outputDirectory, $"{branchCode}_summary.txt");
                File.WriteAllText(path, result);
                TimeSpan timeTaken = DateTime.Now - start;
                Console.WriteLine($"Processed branch {branchCode} in {timeTaken.TotalMilliseconds} ms");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing branch {branchCode}: {ex.Message}");
            }
        }
    }
}




