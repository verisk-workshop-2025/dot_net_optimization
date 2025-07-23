using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


            //step:1
            //Call below functions in multiple thread:
            //DoWork("Task 1 - Worker");
            //DoWork("Task 2 - Worker");


            //step:2
            //Start the threads:



            //step:3
            //Join the main thread to wait for completion:


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