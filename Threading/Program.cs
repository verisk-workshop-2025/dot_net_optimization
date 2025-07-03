using System.Diagnostics;

namespace ExampleThreading
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main Thread Starts...\n");

            Console.WriteLine("=== Single Thread Execution ===");
            SingleThreadExecution();

            Console.WriteLine("\n=== Multi Thread Execution ===");
            MultiThreadExecution();

            Console.WriteLine("\nMain Thread Ends...");
        }

        public static void SingleThreadExecution()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            DoWork("Task 1 - Main");
            DoWork("Task 2 - Main");

            stopwatch.Stop();
            Console.WriteLine($"\nTime taken (Single Thread): {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void MultiThreadExecution()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            Thread thread1 = new Thread(() => DoWork("Task 1 - Worker"));
            Thread thread2 = new Thread(() => DoWork("Task 2 - Worker"));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            stopwatch.Stop();
            Console.WriteLine($"\nTime taken (Multi Thread): {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void DoWork(string taskName)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"{taskName} - Step {i + 1}");
                Thread.Sleep(200); // Simulating time-consuming work....
            }
        }

    }
}