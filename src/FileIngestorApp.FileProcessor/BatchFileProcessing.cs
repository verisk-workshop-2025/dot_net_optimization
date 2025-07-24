using FileIngestorApp.Core.Contracts;
using FileIngestorApp.Core.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace FileIngestorApp.FileProcessor;
public class BatchFileProcessing : IFileProcessor
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


        // Step 2: Batch Formation (Each branch is considered a batch)


        // Step 3: Job Scheduling


        // Step 4 & 5 : Processing and Error Handling


        // Step 6: Output Generation


        // Step 7: Post-Processing
       
    }



}
