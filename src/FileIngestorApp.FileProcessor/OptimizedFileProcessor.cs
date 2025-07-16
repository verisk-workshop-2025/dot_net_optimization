using FileIngestorApp.Core.Contracts;
using FileIngestorApp.Core.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        // Step 1: Input Collection
        Console.WriteLine("[Step 1] Input Collection: Gathering branch product files...");
        var productFiles = Directory.GetFiles(inputDirectory, "*_products.jl");

        // Step 2: Batch Formation (Each branch is considered a batch)
        var branchCodes = productFiles
            .Select(f => Path.GetFileName(f).Split('_')[0])
            .Distinct()
            .ToList();      
        Console.WriteLine("[Step 2] Batch Formation: Total Batches = " + branchCodes.Count);

        // Step 3: Job Scheduling -- We are processing it immedately here, but in a real-world scenario, this would be different. 
        Console.WriteLine("[Step 3] Job Scheduling: Scheduling tasks with max threads = " + Environment.ProcessorCount);


        int maxThreads = Environment.ProcessorCount; //Environment.ProcessorCount: Returns number of logical CPU cores (e.g., 4, 8).
        var semaphore = new SemaphoreSlim(maxThreads); //SemaphoreSlim: Limits how many tasks can run at the same time. Think of it like giving only N keys to allow entry.
        var tasks = new List<Task<BranchProcessingResult>>(); //tasks: to collect all the async tasks.

        foreach (var branchCode in branchCodes)
        {
            //Before starting work for each branch, we WaitAsync() on the semaphore.
            //If the semaphore is already at its max, the code waits until a slot is free.
            await semaphore.WaitAsync(); //WaitAsync() ensures concurrency limit is respected.

            var task = Task.Run(() =>
            {
                var sw = Stopwatch.StartNew();
                var result = new BranchProcessingResult { BranchCode = branchCode };
                try
                {
                   
                    // Step 4: Processing
                    var output = new ProcessBatch().Execute(branchCode, inputDirectory, outputDirectory);
                    result.SummaryText = output;                    
                }
                catch (Exception ex)
                {
                    // step 5: Error Handling
                    result.ErrorMessage = ex.Message;
                }
                finally
                {
                    sw.Stop();
                    result.ProcessingTime = sw.Elapsed;
                    semaphore.Release();
                }
                return result;
            });

            tasks.Add(task);
        }

        var results = await Task.WhenAll(tasks);

        // Step 6: Output Generation
        Console.WriteLine("[Step 6] Output Generation: Writing summaries...");
        Directory.CreateDirectory(outputDirectory);
        foreach (var result in results)
        {
            var path = Path.Combine(outputDirectory, $"{result.BranchCode}_summary.txt");
            File.WriteAllText(path, result.Success ? result.SummaryText : $"ERROR: {result.ErrorMessage}");
            Console.WriteLine($"[Step 4] Branch {result.BranchCode} processed in {result.ProcessingTime.TotalMilliseconds} ms");
        }

        // Step 7: Post-Processing
        Console.WriteLine("[Step 7] Post-Processing: All branches processed.");

    }



}
