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

        public (string highestSellingCountItem, string lowestSellingItemCountItem) FindLowestAndHighestSellingCountItems(string branch)
        {
            const string NoSalesData = "No sales data";

            var sales = Branches[branch].Sales.Where(o => o.Status.Equals("COMPLETED"));

            if (!sales.Any())
                return (NoSalesData, NoSalesData);

            // Using Dictionary for count accumulation
            var dictSalesTotalByItemName = new Dictionary<string, int>();
            foreach (var sale in sales)
            {
                if (dictSalesTotalByItemName.ContainsKey(sale.ItemName))
                    dictSalesTotalByItemName[sale.ItemName] += sale.Quantity;
                else
                    dictSalesTotalByItemName[sale.ItemName] = sale.Quantity;
            }

            string lowestSellingCountItem = NoSalesData, highestSellingCountItem = NoSalesData;
            int lowest = int.MaxValue, highest = int.MinValue;

            foreach (var kvp in dictSalesTotalByItemName)
            {
                if (kvp.Value < lowest)
                {
                    lowest = kvp.Value;
                    lowestSellingCountItem = kvp.Key;
                }

                if (kvp.Value > highest)
                {
                    highest = kvp.Value;
                    highestSellingCountItem = kvp.Key;
                }
            }

            return (highestSellingCountItem, lowestSellingCountItem);
        }
    }
}
