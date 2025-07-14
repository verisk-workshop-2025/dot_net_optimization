using SalesAnalyzer.Lib.Services;
using System.Diagnostics;

var watch = new Stopwatch();
watch.Start();

var processor = new SalesProcessor();

Console.WriteLine("Starting..");

processor.Initialize();

var result = string.Empty;
foreach (var branch in processor.Branches)
{
    var lowestSellingCountItem = processor.FindLowestSellingCountItem(branch.Key);
    var highestSellingCountItem = processor.FindHighestSellingCountItem(branch.Key);

    result += $"{Environment.NewLine}{Environment.NewLine}";

    result += $"Branch: {branch.Key} {Environment.NewLine}";
    result += $"Lowest Selling Count Item: {lowestSellingCountItem} {Environment.NewLine}";
    result += $"Highest Selling Count Item: {highestSellingCountItem} {Environment.NewLine}";
}

File.WriteAllText("Result.txt", result);

Console.WriteLine(result);

Console.WriteLine("Completed..");

Console.WriteLine($"Elapsed time: {watch.ElapsedMilliseconds}ms");
watch.Stop();

Console.ReadLine();