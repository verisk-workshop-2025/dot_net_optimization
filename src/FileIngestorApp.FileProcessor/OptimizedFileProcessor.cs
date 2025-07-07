using FileIngestorApp.Core.Contracts;
using FileIngestorApp.Core.Models;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace FileIngestorApp.FileProcessor;
public class OptimizedFileProcessor : IFileProcessor
{
    public void ProcessBranchesData(string inputDirectory, string outputDirectory)
    {
        //var sw = Stopwatch.StartNew();
        ProcessBranchesDataAsync(inputDirectory, outputDirectory).Wait();
        //sw.Stop();
        //Console.WriteLine($"Total time with multiple thread: {sw.ElapsedMilliseconds} ms");
             
    }

    private async Task ProcessBranchesDataAsync(string inputDirectory, string outputDirectory)
    {
        var productFiles = Directory.GetFiles(inputDirectory, "*_products.jl");
        var branchCodes = productFiles
            .Select(f => Path.GetFileName(f).Split('_')[0])
            .Distinct()
            .ToList();

        int maxThreads = Environment.ProcessorCount; 
        var semaphore = new SemaphoreSlim(maxThreads);
        var tasks = new List<Task>();
        var logs = new ConcurrentBag<string>();

        foreach (var branchCode in branchCodes)
        {
            await semaphore.WaitAsync();

            var task = Task.Run(() =>
            {
                try
                {
                    var sw = Stopwatch.StartNew();
                    new ProcessBatch().Execute(branchCode, inputDirectory, outputDirectory);
                    sw.Stop();
                    logs.Add($"Processed branch {branchCode} in {sw.ElapsedMilliseconds} ms");
                }
                catch (Exception ex)
                {
                    logs.Add($"Error processing branch {branchCode}: {ex.Message}");
                }
                finally
                {
                    semaphore.Release();
                }
            });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        foreach (var log in logs.OrderBy(l => l))
        {
            Console.WriteLine(log);
        }
    }



}
