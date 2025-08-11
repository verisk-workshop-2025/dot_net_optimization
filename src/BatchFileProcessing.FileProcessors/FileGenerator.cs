using BatchFileProcessing.Core.Models;
using Bogus;
using Newtonsoft.Json;

namespace BatchFileProcessing.FileProcessors;

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
        Dictionary<string, List<string>> categoryProductMap = new()
        {
            ["Grocery"] = ["Rice Bag", "Atta", "Lentils", "Sugar", "Mustard Oil", "Ghee"],
            ["Snacks"] = ["Snickers", "Lays Chips", "Oreo", "Maggi Noodles", "Momo Noodles"],
            ["Personal Care"] = ["Dove Soap", "Closeup Toothpaste", "Lifebuoy Soap", "Clinic Plus Shampoo", "Fair & Lovely"],
            ["Cleaning"] = ["Vim Bar", "Surf Excel", "Harpic", "Colin Spray", "Phenyl"],
            ["Beverages"] = ["Pepsi", "Coke", "Real Juice", "Fanta", "Red Bull"]
        };

        string[] brands = ["Dove", "PepsiCo", "HUL", "Colgate", "Unilever", "Nestle", "Patanjali"];
        string[] units = ["pcs", "kg", "ltr", "pack"];
        string[] suppliers = ["Central Warehouse", "Local Distributor", "Importer Pvt Ltd"];
        var flatList = categoryProductMap.SelectMany(kv => kv.Value.Select(p => new { ProductName = p, Category = kv.Key })).ToList();

        Faker faker = new();
        Directory.CreateDirectory(outputDirectory);

        string productFile = Path.Combine(outputDirectory, $"{branchCode}_products.jl");
        string transactionFile = Path.Combine(outputDirectory, $"{branchCode}_transactions.jl");
        List<Product> productList = [];

        using StreamWriter productWriter = new(productFile);
        for (int i = 0; i < 500; i++)
        {
            var entry = faker.PickRandom(flatList);
            double minPrice = faker.Random.Double(20, 200);
            double maxPrice = minPrice + faker.Random.Double(10, 100);

            Product product = new()
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

        using StreamWriter txnWriter = new(transactionFile);
        for (int i = 0; i < transactionsCount; i++)
        {
            int itemCount = _random.Next(1, 6);
            List<Product> selectedProducts = productList.OrderBy(_ => _random.Next()).Take(itemCount).ToList();

            List<Item> items = [];
            double transactionTotal = 0;

            foreach (Product? product in selectedProducts)
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

            Transaction transaction = new()
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
