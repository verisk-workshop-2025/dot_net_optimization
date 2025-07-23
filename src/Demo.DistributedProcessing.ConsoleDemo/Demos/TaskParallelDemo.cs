using System.Diagnostics;
namespace Demo.DistributedProcessing.ConsoleDemo.Demos;

internal class TaskParallelDemo
{
    public static void Run()
    {
        Stopwatch stopwatch = new();
        //var option = new ParallelOptions
        //{
        //    MaxDegreeOfParallelism = 2
        //};
        stopwatch.Start();
        Parallel.For(0, 10, index =>
        {
            int value = Compute(index);
            Console.WriteLine(value);
        });
        stopwatch.Stop();
        Console.WriteLine("Time taken: {0}", stopwatch.Elapsed);
    }

    private static int Compute(int index)
    {
        Thread.Sleep(1000); // simulate long task
        return index;
    }
}
/* Sample output:
    1
    0
    3
    9
    2
    4
    5
    6
    7
    8
*/
