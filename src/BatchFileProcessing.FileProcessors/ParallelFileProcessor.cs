using BatchFileProcessing.Core.Contracts;
using BatchFileProcessing.Core.Models;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace BatchFileProcessing.FileProcessors;
public class ParallelFileProcessor : IFileProcessor
{
    public void ProcessBranchesData(string inputDirectory, string outputDirectory)
    {
        var sw = Stopwatch.StartNew();

        // Step 1: Input Collection

        // Step 2: Batch Formation(Each branch is considered a batch)

        // Step 3: Job Scheduling

        // Step 4: Processing

        // step 5: Error Handling

        // Step 6: Output Generation

        // Step 7: Post - Processing

        sw.Stop();
        Console.WriteLine($"Total time With Multiple Thread: {sw.ElapsedMilliseconds} ms");

    }

}
