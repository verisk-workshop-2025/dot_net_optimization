﻿using SalesAnalyzer.Lib.Models;
using System.Globalization;

namespace SalesAnalyzer.Lib.Services
{
    public class SalesProcessor(string dataPath = "Data")
    {
        public Dictionary<string, Branch> Branches { get; } = new Dictionary<string, Branch>(StringComparer.Ordinal);

        public void Initialize()
        {
            foreach (var file in Directory.GetFiles(dataPath))
            {
                var content = File.ReadAllText(file);
                var lines = content.Split('\n');

                foreach (var line in lines[1..])
                {
                    var lineCols = line.Split(',');

                    if (lineCols.Length < 10)
                        continue;

                    var trade = new Sale
                    {
                        Branch = lineCols[0],
                        CustomerId = string.Equals(lineCols[1], "NULL", StringComparison.OrdinalIgnoreCase) ? null : lineCols[1],
                        BillTo = lineCols[2],
                        ItemName = lineCols[3],
                        Category = lineCols[4],
                        Price = decimal.Parse(lineCols[5]),
                        Quantity = int.Parse(lineCols[6]),
                        Total = decimal.Parse(lineCols[7]),
                        Status = lineCols[8],
                        BillDate = DateTime.ParseExact(lineCols[9].Trim(), "yyyyMMdd", CultureInfo.InvariantCulture)
                    };

                    if (!Branches.ContainsKey(lineCols[0]))
                    {
                        Branches[lineCols[0]] = new Branch(lineCols[0]);
                    }

                    Branches[lineCols[0]].Sales.Add(trade);
                }
            }
        }

        public void InitializeV2()
        {
            foreach (var file in Directory.GetFiles(dataPath))
            {
                using var reader = new StreamReader(File.Open(file, FileMode.Open, FileAccess.Read));
                string? line = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    var lineCols = line.Split(',');
                    var sale = new Sale
                    {
                        //Branch = lineCols[0],
                        //CustomerId = string.Equals(lineCols[1], "NULL", StringComparison.OrdinalIgnoreCase) ? null : lineCols[1],
                        //BillTo = lineCols[2],
                        ItemName = lineCols[3],
                        //Category = lineCols[4],
                        //Price = decimal.Parse(lineCols[5], CultureInfo.InvariantCulture),
                        Quantity = int.Parse(lineCols[6]),
                        //Total = decimal.Parse(lineCols[7], CultureInfo.InvariantCulture),
                        Status = lineCols[8],
                        //BillDate = DateTime.ParseExact(lineCols[9].Trim(), "yyyyMMdd", CultureInfo.InvariantCulture)
                    };

                    if (!Branches.ContainsKey(lineCols[0]))
                        Branches.Add(lineCols[0], new Branch(lineCols[0]));

                    Branches[lineCols[0]].Sales.Add(sale);
                }
            }
        }

        public void InitializeV3()
        {
            foreach (var file in Directory.GetFiles(dataPath))
            {
                using var reader = new StreamReader(file);
                reader.ReadLine();

                while (reader.ReadLine() is string line)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    int startIndex = 0;
                    int endIndex = 0;
                    string branchName = string.Empty;
                    string itemName = string.Empty;
                    string quantitiy = string.Empty;
                    string status = string.Empty;

                    for (int column = 0; column < 10; column++)
                    {
                        endIndex = line.IndexOf(',', startIndex);

                        if (column == 0)
                            branchName = line[startIndex..endIndex];

                        if (column == 3)
                            itemName = line[startIndex..endIndex];

                        if (column == 6)
                            quantitiy = line[startIndex..endIndex];

                        if (column == 8)
                            status = line[startIndex..endIndex];

                        startIndex = endIndex + 1;
                    }

                    var sale = new Sale
                    {
                        //Branch = lineCols[0],
                        //CustomerId = string.Equals(lineCols[1], "NULL", StringComparison.OrdinalIgnoreCase) ? null : lineCols[1],
                        //BillTo = lineCols[2],
                        ItemName = itemName,
                        //Category = lineCols[4],
                        //Price = decimal.Parse(lineCols[5], CultureInfo.InvariantCulture),
                        Quantity = int.Parse(quantitiy),
                        //Total = decimal.Parse(lineCols[7], CultureInfo.InvariantCulture),
                        Status = status,
                        //BillDate = DateTime.ParseExact(lineCols[9].Trim(), "yyyyMMdd", CultureInfo.InvariantCulture)
                    };

                    if (!Branches.ContainsKey(branchName))
                        Branches.Add(branchName, new Branch(branchName));

                    Branches[branchName].Sales.Add(sale);
                }
            }
        }

        public string FindLowestSellingCountItem(string branch)
        {
            var itemsWithTotalSoldCount = Branches[branch].Sales
                .Where(o => o.Status.Equals("COMPLETED"))
                .GroupBy(o => new { o.ItemName })
                .Select(g => new
                {
                    ItemName = g.Key.ItemName,
                    TotalSales = g.Sum(o => o.Quantity)
                });

            var lowestSellingCountItem = itemsWithTotalSoldCount.OrderBy(o => o.TotalSales).FirstOrDefault();

            return lowestSellingCountItem?.ItemName ?? "No sales data";
        }

        public string FindHighestSellingCountItem(string branch)
        {
            var itemsWithTotalSoldCount = Branches[branch].Sales
                .Where(o => o.Status.Equals("COMPLETED"))
                .GroupBy(o => new { o.ItemName })
                .Select(g => new
                {
                    ItemName = g.Key.ItemName,
                    TotalSales = g.Sum(o => o.Quantity)
                });

            var highestSellingCountItem = itemsWithTotalSoldCount.OrderByDescending(o => o.TotalSales).FirstOrDefault();

            return highestSellingCountItem?.ItemName ?? "No sales data";
        }

        public (string highestSellingCountItem, string lowestSellingItemCountItem) GetReportsForHighestAndLowestSellingCount(string branch)
        {
            var itemsWithTotalSoldCountSortedDesc = Branches[branch].Sales
                .Where(o => o.Status.Equals("COMPLETED"))
                .GroupBy(o => new { o.ItemName })
                .Select(g => new
                {
                    ItemName = g.Key.ItemName,
                    TotalSales = g.Sum(o => o.Quantity)
                })
                .OrderByDescending(o => o.TotalSales);

            string highestSellingCountItem = itemsWithTotalSoldCountSortedDesc.FirstOrDefault()?.ItemName ?? "No sales data";
            string lowestSellingCountItem = itemsWithTotalSoldCountSortedDesc.LastOrDefault()?.ItemName ?? "No sales data";

            return (highestSellingCountItem, highestSellingCountItem);
        }
    }
}
