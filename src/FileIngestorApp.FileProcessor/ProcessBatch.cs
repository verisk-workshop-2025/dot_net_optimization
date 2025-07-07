using FileIngestorApp.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileIngestorApp.FileProcessor
{
    public class ProcessBatch
    {
        public void Execute(string branchCode, string inputDirectory, string outputDirectory)
        {
            var productFile = Path.Combine(inputDirectory, $"{branchCode}_products.jl");
            var transactionFile = Path.Combine(inputDirectory, $"{branchCode}_transactions.jl");
            if (!File.Exists(productFile))
                throw new FileNotFoundException($"Product file missing for branch {branchCode}", productFile);
            if (!File.Exists(transactionFile))
                throw new FileNotFoundException($"Transaction file missing for branch {branchCode}", transactionFile);

            var productMap = File.ReadLines(productFile)
                .Select(line => JsonConvert.DeserializeObject<Product>(line))
                .ToDictionary(p => p.ProductID);

            var transactions = File.ReadLines(transactionFile)
                .Select(line => JsonConvert.DeserializeObject<Transaction>(line))
                .ToList();

            Directory.CreateDirectory(outputDirectory);
            var resultPath = Path.Combine(outputDirectory, $"{branchCode}_summary.txt");

            var totalSales = transactions.Sum(t => t.TransactionTotal);
            var totalItemsSold = transactions.SelectMany(t => t.Items).Sum(i => i.Quantity);
            var mostSoldProduct = transactions
                .SelectMany(t => t.Items)
                .GroupBy(i => i.ProductName)
                .OrderByDescending(g => g.Sum(x => x.Quantity))
                .First().Key;

            var mostSoldCategory = transactions
                .SelectMany(t => t.Items)
                .GroupBy(i => i.Category)
                .OrderByDescending(g => g.Sum(x => x.Quantity))
                .First().Key;

            var topCashiers = transactions
                .GroupBy(t => t.CashierID)
                .OrderByDescending(g => g.Count())
                .Take(2)
                .Select(g => new { CashierID = g.Key, Transactions = g.Count() });

            var mostProfitableProduct = transactions
                .SelectMany(t => t.Items)
                .GroupBy(i => i.ProductName)
                .OrderByDescending(g => g.Sum(i => i.Quantity * (productMap.FirstOrDefault(p => p.Value.ProductName == g.Key).Value?.MaxPrice ?? 0)))
                .First().Key;

            var mostProfitableCategory = transactions
                .SelectMany(t => t.Items)
                .GroupBy(i => i.Category)
                .OrderByDescending(g => g.Sum(i => i.Quantity * (productMap.FirstOrDefault(p => p.Value.Category == g.Key).Value?.MaxPrice ?? 0)))
                .First().Key;

            var peakSalesHour = transactions
                .GroupBy(t => t.TransactionDate.Hour)
                .OrderByDescending(g => g.Count())
                .First().Key;

            var leastSoldProduct = transactions
                .SelectMany(t => t.Items)
                .GroupBy(i => i.ProductName)
                .Where(g => g.Sum(x => x.Quantity) > 0)
                .OrderBy(g => g.Sum(x => x.Quantity))
                .First().Key;

            var averageTransactionValue = transactions.Average(t => t.TransactionTotal);
            var numberOfTransactions = transactions.Count;
            var uniqueProductsSold = transactions.SelectMany(t => t.Items).Select(i => i.ProductID).Distinct().Count();

            var perishableProductsSoldCount = transactions
                .SelectMany(t => t.Items)
                .Count(i => productMap.TryGetValue(i.ProductID, out var prod) && prod.IsPerishable);

            var discountEligibleSalesCount = transactions
                .SelectMany(t => t.Items)
                .Count(i => productMap.TryGetValue(i.ProductID, out var prod) && prod.DiscountEligible);

            using var writer = new StreamWriter(resultPath);
            writer.WriteLine($"Branch: {branchCode}");
            writer.WriteLine($"Total Sales: {totalSales:F2}");
            writer.WriteLine($"Total Items Sold: {totalItemsSold}");
            writer.WriteLine($"Most Sold Product: {mostSoldProduct}");
            writer.WriteLine($"Most Sold Category: {mostSoldCategory}");
            writer.WriteLine("Top 2 Cashiers:");
            foreach (var cashier in topCashiers)
            {
                writer.WriteLine($" - {cashier.CashierID}: {cashier.Transactions} transactions");
            }
            writer.WriteLine($"Most Profitable Product: {mostProfitableProduct}");
            writer.WriteLine($"Most Profitable Category: {mostProfitableCategory}");
            //writer.WriteLine($"Peak Sales Hour: {peakSalesHour}:00");
            writer.WriteLine($"Least Sold Product: {leastSoldProduct}");
            writer.WriteLine($"Average Transaction Value: {averageTransactionValue:F2}");
            writer.WriteLine($"Number of Transactions: {numberOfTransactions}");
            writer.WriteLine($"Number of Unique Products Sold: {uniqueProductsSold}");
            writer.WriteLine($"Perishable Products Sold Count: {perishableProductsSoldCount}");
            writer.WriteLine($"Discount Eligible Sales Count: {discountEligibleSalesCount}");
        }
    }
}
