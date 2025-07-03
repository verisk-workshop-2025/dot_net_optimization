namespace Demo.DistributedProcessing.ConsoleDemo.Demos;

internal class ParallelLinqDemo
{
    public static void Run()
    {
        List<int> items = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        items.AsParallel()
            .ForAll(index =>
            {
                Thread.Sleep(500); // cpu heavy task
                Console.WriteLine("Current index: {0}", index);
            });
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