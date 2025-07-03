﻿using System.Threading.Tasks.Dataflow;

namespace Demo.DistributedProcessing.ConsoleDemo.Demos;

// Source: https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/walkthrough-creating-a-dataflow-pipeline
internal class DataflowDemo
{
    // Demonstrates how to create a basic dataflow pipeline.
    // This program downloads the book "The Iliad of Homer" by Homer from the Web
    // and finds all reversed words that appear in that book.
    public static async Task Run()
    {
        //
        // Create the members of the pipeline.
        //

        // Downloads the requested resource as a string.
        var downloadString = new TransformBlock<string, string>(async uri =>
        {
            Console.WriteLine($"Downloading '{uri}'...");

            return await new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip }).GetStringAsync(uri);
        });

        // Separates the specified text into an array of words.
        var createWordList = new TransformBlock<string, string[]>(text =>
        {
            Console.WriteLine("Creating word list...");

            // Remove common punctuation by replacing all non-letter characters
            // with a space character.
            char[] tokens = text.Select(c => char.IsLetter(c) ? c : ' ').ToArray();
            text = new string(tokens);

            // Separate the text into an array of words.
            return text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        });

        // Removes short words and duplicates.
        var filterWordList = new TransformBlock<string[], string[]>(words =>
        {
            Console.WriteLine("Filtering word list...");

            return words
               .Where(word => word.Length > 3)
               .Distinct()
               .ToArray();
        });

        // Finds all words in the specified collection whose reverse also
        // exists in the collection.
        var findReversedWords = new TransformManyBlock<string[], string>(words =>
        {
            Console.WriteLine("Finding reversed words...");

            var wordsSet = new HashSet<string>(words);

            return from word in words.AsParallel()
                   let reverse = new string(word.Reverse().ToArray())
                   where word != reverse && wordsSet.Contains(reverse)
                   select word;
        });

        // Prints the provided reversed words to the console.
        var printReversedWords = new ActionBlock<string>(reversedWord =>
        {
            Console.WriteLine($"Found reversed words {reversedWord}/{new string(reversedWord.Reverse().ToArray())}");
        });

        //
        // Connect the dataflow blocks to form a pipeline.
        //

        var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

        downloadString.LinkTo(createWordList, linkOptions);
        createWordList.LinkTo(filterWordList, linkOptions);
        filterWordList.LinkTo(findReversedWords, linkOptions);
        findReversedWords.LinkTo(printReversedWords, linkOptions);

        // Process "The Iliad of Homer" by Homer.
        downloadString.Post("http://www.gutenberg.org/cache/epub/16452/pg16452.txt");

        // Mark the head of the pipeline as complete.
        downloadString.Complete();

        // Wait for the last block in the pipeline to process all messages.
        // printReversedWords.Completion.Wait(); // for running synchronously
        await printReversedWords.Completion;
    }
}
/* Sample output:
   Downloading 'http://www.gutenberg.org/cache/epub/16452/pg16452.txt'...
   Creating word list...
   Filtering word list...
   Finding reversed words...
   Found reversed words doom/mood
   Found reversed words draw/ward
   Found reversed words aera/area
   Found reversed words seat/taes
   Found reversed words live/evil
   Found reversed words port/trop
   Found reversed words sleek/keels
   Found reversed words area/aera
   Found reversed words tops/spot
   Found reversed words evil/live
   Found reversed words mood/doom
   Found reversed words speed/deeps
   Found reversed words moor/room
   Found reversed words trop/port
   Found reversed words spot/tops
   Found reversed words spots/stops
   Found reversed words stops/spots
   Found reversed words reed/deer
   Found reversed words keels/sleek
   Found reversed words deeps/speed
   Found reversed words deer/reed
   Found reversed words taes/seat
   Found reversed words room/moor
   Found reversed words ward/draw
*/

