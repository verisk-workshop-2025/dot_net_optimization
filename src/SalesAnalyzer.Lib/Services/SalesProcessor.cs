using SalesAnalyzer.Lib.Models;
using System.Globalization;

namespace SalesAnalyzer.Lib.Services
{
    public class SalesProcessor(string dataPath = "Data")
    {
        public Dictionary<string, Branch> Branches { get; } = new Dictionary<string, Branch>(StringComparer.Ordinal);

        public void InitializeUnoptimized()
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

            var lowestSellingItem = itemsWithTotalSoldCount.OrderBy(o => o.TotalSales).FirstOrDefault();

            return lowestSellingItem?.ItemName ?? "No sales data";
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

            var highestSellingItem = itemsWithTotalSoldCount.OrderByDescending(o => o.TotalSales).FirstOrDefault();

            return highestSellingItem?.ItemName ?? "No sales data";
        }
    }
}
