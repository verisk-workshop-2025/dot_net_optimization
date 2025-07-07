using FileIngestorApp.Core.Contracts;
using FileIngestorApp.Core.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace FileIngestorApp.FileProcessor;

public class LegacyFileProcessor : IFileProcessor
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
        var productFiles = Directory.GetFiles(inputDirectory, "*_products.jl");
        var branchCodes = productFiles
            .Select(f => Path.GetFileName(f).Split('_')[0])
            .Distinct()
            .ToList();

        foreach (var branchCode in branchCodes)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                new ProcessBatch().Execute(branchCode, inputDirectory, outputDirectory);
                sw.Stop();
                Console.WriteLine($"Processed branch {branchCode} in {sw.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing branch {branchCode}: {ex.Message}");
            }
        }
    }
}




