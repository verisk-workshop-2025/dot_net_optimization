using System.Collections.Concurrent;

namespace BlockingCollection
{
    class Program
    {
        private static BlockingCollection<int> _queue = new();

        static async Task Main()
        {
            // Start producer and consumer
            var producer = Task.Run(() => Produce());
            var consumer = Task.Run(() => Consume());

            await Task.WhenAll(producer, consumer);
            Console.WriteLine("Processing completed.");
        }

        static void Produce()
        {
            for (int i = 1; i <= 20; i++)
            {
                Console.WriteLine($"[Producer] Produced: {i}");
                _queue.Add(i); // Add item to the queue
                Task.Delay(100).Wait(); // Simulate delay
            }

            _queue.CompleteAdding(); // Signal that no more items will be added
        }

        static void Consume()
        {
            foreach (var number in _queue.GetConsumingEnumerable())
            {
                if (IsPrime(number))
                {
                    Console.WriteLine($"    [Consumer] {number} is prime.");
                }
                else
                {
                    Console.WriteLine($"    [Consumer] {number} is not prime.");
                }

                Task.Delay(150).Wait(); // Simulate processing time
            }
        }

        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
                if (number % i == 0) return false;
            return true;
        }
    }
