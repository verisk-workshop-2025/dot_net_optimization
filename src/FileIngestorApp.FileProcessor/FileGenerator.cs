using Bogus;
using FileIngestorApp.Core.Models;
using Newtonsoft.Json;
using System;
using System.Reflection.Emit;

namespace FileIngestorApp.FileProcessor;

public class FileGenerator
{
     private readonly Random _random = new();
    public void GenerateFile(int transactionPerBranch, string branchCode, string path)
    {
        FileInfo file = new(path);
        if (file.Directory?.Exists == false)
        {
            file.Directory.Create();
        }

        GenerateData(branchCode, transactionPerBranch, path);
    }


    public void GenerateData(string branchCode, int transactionsCount, string outputDirectory)
    {
        var categoryProductMap = new Dictionary<string, List<string>>
        {
            ["Grocery"] = new() { "Rice Bag", "Atta", "Lentils", "Sugar", "Mustard Oil", "Ghee" },
            ["Snacks"] = new() { "Snickers", "Lays Chips", "Oreo", "Maggi Noodles", "Momo Noodles" },
            ["Personal Care"] = new() { "Dove Soap", "Closeup Toothpaste", "Lifebuoy Soap", "Clinic Plus Shampoo", "Fair & Lovely" },
            ["Cleaning"] = new() { "Vim Bar", "Surf Excel", "Harpic", "Colin Spray", "Phenyl" },
            ["Beverages"] = new() { "Pepsi", "Coke", "Real Juice", "Fanta", "Red Bull" }
        };

        var brands = new[] { "Dove", "PepsiCo", "HUL", "Colgate", "Unilever", "Nestle", "Patanjali" };
        var units = new[] { "pcs", "kg", "ltr", "pack" };
        var suppliers = new[] { "Central Warehouse", "Local Distributor", "Importer Pvt Ltd" };
        var flatList = categoryProductMap.SelectMany(kv => kv.Value.Select(p => new { ProductName = p, Category = kv.Key })).ToList();

        var faker = new Bogus.Faker();
        Directory.CreateDirectory(outputDirectory);

        var productFile = Path.Combine(outputDirectory, $"{branchCode}_products.jl");
        var transactionFile = Path.Combine(outputDirectory, $"{branchCode}_transactions.jl");
        var productList = new List<Product>();

        using var productWriter = new StreamWriter(productFile);
        for (int i = 0; i < 500; i++)
        {
            var entry = faker.PickRandom(flatList);
            var minPrice = faker.Random.Double(20, 200);
            var maxPrice = minPrice + faker.Random.Double(10, 100);

            var product = new Product
            {
                ProductID = Guid.NewGuid().ToString(),
                SKU = faker.Commerce.Ean13(),
                ProductName = entry.ProductName,
                Category = entry.Category,
                Brand = faker.PickRandom(brands),
                Unit = faker.PickRandom(units),
                Supplier = faker.PickRandom(suppliers),
                MinPrice = Math.Round(minPrice, 2),
                MaxPrice = Math.Round(maxPrice, 2),
                IsPerishable = faker.Random.Bool(0.3f),
                ShelfLifeInDays = faker.Random.Int(30, 365),
                LaunchDate = faker.Date.Past(2),
                DiscountEligible = faker.Random.Bool(),
                ReorderLevel = faker.Random.Int(10, 100),
                GSTPercent = faker.Random.Double(5, 18),
                PopularityScore = Math.Round(faker.Random.Double(1, 10), 2)
            };

            productList.Add(product);
            productWriter.WriteLine(JsonConvert.SerializeObject(product));
        }

        using var txnWriter = new StreamWriter(transactionFile);
        for (int i = 0; i < transactionsCount; i++)
        {
            int itemCount = _random.Next(1, 6);
            var selectedProducts = productList.OrderBy(_ => _random.Next()).Take(itemCount).ToList();

            var items = new List<Item>();
            double transactionTotal = 0;

            foreach (var product in selectedProducts)
            {
                int quantity = _random.Next(1, 6);
                double price = Math.Round(_random.NextDouble() * (product.MaxPrice - product.MinPrice) + product.MinPrice, 2);
                double totalPrice = quantity * price;

                items.Add(new Item
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Category = product.Category,
                    Quantity = quantity,
                    UnitPrice = price,
                    TotalPrice = totalPrice
                });

                transactionTotal += totalPrice;
            }

            var transaction = new Transaction
            {
                TransactionID = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now.AddMinutes(-_random.Next(0, 1440)).ToString("o"),
                BranchCode = branchCode,
                CashierID = $"CASH{_random.Next(100, 999)}",
                Items = items,
                PaymentMode = faker.PickRandom("Cash", "Card", "Digital Wallet"),
                DiscountApplied = Math.Round(transactionTotal * (_random.NextDouble() * 0.15), 2),
                CustomerType = faker.PickRandom("Regular", "Member", "First-time"),
                TransactionTotal = Math.Round(transactionTotal, 2)
            };

            txnWriter.WriteLine(JsonConvert.SerializeObject(transaction));
        }

    }

}
