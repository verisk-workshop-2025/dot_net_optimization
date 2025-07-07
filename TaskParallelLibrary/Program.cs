using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TaskParallelLibrary
{
    class Program
    {
        static void Main()
        {
            // 2 million
            var limit = 2_000_000;
            var numbers = Enumerable.Range(0, limit).ToList();

            var watch = Stopwatch.StartNew();
            var primeNumbersFromForeach = GetPrimeList(numbers);
            watch.Stop();

            var watchForParallel = Stopwatch.StartNew();
            var primeNumbersFromParallelForeach = GetPrimeListWithParallel(numbers);
            watchForParallel.Stop();

            Console.WriteLine($"Classical foreach loop | Total prime numbers : {primeNumbersFromForeach.Count} | Time Taken : {watch.ElapsedMilliseconds} ms.");
            Console.WriteLine($"Parallel.ForEach loop  | Total prime numbers : {primeNumbersFromParallelForeach.Count} | Time Taken : {watchForParallel.ElapsedMilliseconds} ms.");

            Console.WriteLine("Press 'Enter' to exit.");
            Console.ReadLine();
        }

        /// <summary>
        /// GetPrimeList returns Prime numbers by using sequential ForEach
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        private static IList<int> GetPrimeList(IList<int> numbers) => numbers.Where(IsPrime).ToList();

        /// <summary>
        /// GetPrimeListWithParallel returns Prime numbers by using Parallel.ForEach
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        private static IList<int> GetPrimeListWithParallel(IList<int> numbers)
        {

            // Sequential version
            //var primeNumbers = new List<int>();

            /* NOTE:::::
             Multiple threads may try to add to the collection at the same time. 
             A standard List<T> is not thread-safe, and using it in this way could cause:
                * Race conditions
                * Data corruption
                * Exceptions (e.g., InvalidOperationException)
             */

            //foreach (var number in numbers)
            //{
            //    if (IsPrime(number))
            //    {
            //        primeNumbers.Add(number);
            //    }
            //}


            // Parallel equivalent

            /* NOTE:: Using ConcurrentBag<T> to safely add items from multiple threads.
            ConcurrentBag<T> is specifically designed for safe, lock-free access by multiple threads. 
            Internally, it uses thread-local storage and fine-grained locking to maximize performance and correctness in concurrent scenarios.
            */

            var primeNumbers = new ConcurrentBag<int>();


            Parallel.ForEach(numbers, number =>
            {
                if (IsPrime(number))
                {
                    primeNumbers.Add(number);
                }
            });

            return primeNumbers.ToList();

            /* Final Note:::::
             Parallel.ForEach partitions the input IEnumerable<T> (numbers) into chunks.
             It creates tasks for each chunk.
             These tasks run in parallel on multiple threads (using the ThreadPool).
             The delegate (number => { ... }) is executed concurrently across threads.
             Results are written into a thread-safe ConcurrentBag<int>.
             */
        }

        /// <summary>
        /// IsPrime returns true if number is Prime, else false.(https://en.wikipedia.org/wiki/Prime_number)
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static bool IsPrime(int number)
        {
            if (number < 2)
            {
                return false;
            }

            for (var divisor = 2; divisor <= Math.Sqrt(number); divisor++)
            {
                if (number % divisor == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}