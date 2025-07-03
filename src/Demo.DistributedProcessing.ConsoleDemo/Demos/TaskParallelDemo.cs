namespace Demo.DistributedProcessing.ConsoleDemo.Demos;

internal class TaskParallelDemo
{
    public static void Run()
    {
        Parallel.For(0, 10, index =>
        {
            var value = Compute(index);
            Console.WriteLine(value);
        });
    }

    private static int Compute(int index)
    {
        Thread.Sleep(500); // simulate long task
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
