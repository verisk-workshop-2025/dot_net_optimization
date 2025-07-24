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
        public string Execute(string branchCode, string inputDirectory, string outputDirectory)
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

            var writer = new System.Text.StringBuilder(); 
            writer.AppendLine($"Branch: {branchCode}");
            writer.AppendLine($"Total Sales: {totalSales:F2}");
            writer.AppendLine($"Total Items Sold: {totalItemsSold}");
            writer.AppendLine($"Most Sold Product: {mostSoldProduct}");
            writer.AppendLine($"Most Sold Category: {mostSoldCategory}");
            writer.AppendLine("Top 2 Cashiers:");
            foreach (var cashier in topCashiers)
            {
                writer.AppendLine($" - {cashier.CashierID}: {cashier.Transactions} transactions");
            }
            writer.AppendLine($"Most Profitable Product: {mostProfitableProduct}");
            writer.AppendLine($"Most Profitable Category: {mostProfitableCategory}");
            //writer.AppendLine($"Peak Sales Hour: {peakSalesHour}:00");
            writer.AppendLine($"Least Sold Product: {leastSoldProduct}");
            writer.AppendLine($"Average Transaction Value: {averageTransactionValue:F2}");
            writer.AppendLine($"Number of Transactions: {numberOfTransactions}");
            writer.AppendLine($"Number of Unique Products Sold: {uniqueProductsSold}");
            writer.AppendLine($"Perishable Products Sold Count: {perishableProductsSoldCount}");
            writer.AppendLine($"Discount Eligible Sales Count: {discountEligibleSalesCount}");
            return writer.ToString();
        }
    }
}
