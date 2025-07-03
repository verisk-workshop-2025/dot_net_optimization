namespace BBSMLogGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;

   public class Program
   {
        static readonly List<string> PaymentModes = new() { "Cash", "Card", "eSewa", "Khalti" };
        static readonly List<string> CustomerTypes = new() { "Regular", "Member" };
        static readonly Random random = new();

        static readonly List<Product> Products = new()
        {
        // Personal Care
        new("P1001", "Dove Soap", "Personal Care", 70, 90),
        new("P1002", "Closeup Toothpaste", "Personal Care", 80, 100),
        new("P1003", "Dettol Handwash", "Personal Care", 120, 150),
        new("P1004", "Lifebuoy Soap", "Personal Care", 60, 80),
        new("P1005", "Fair & Lovely Cream", "Personal Care", 150, 180),
        new("P1006", "Clinic Plus Shampoo", "Personal Care", 100, 130),

        // Beverages
        new("P2001", "Red Bull", "Beverages", 120, 160),
        new("P2002", "Pepsi", "Beverages", 50, 70),
        new("P2003", "Coke", "Beverages", 50, 70),
        new("P2004", "Fanta", "Beverages", 50, 70),
        new("P2005", "Slice Mango Drink", "Beverages", 40, 60),
        new("P2006", "Real Juice", "Beverages", 80, 110),

        // Grocery
        new("P3001", "Rice Bag", "Grocery", 1200, 1600),
        new("P3002", "Atta (Wheat Flour)", "Grocery", 900, 1300),
        new("P3003", "Sugar", "Grocery", 100, 140),
        new("P3004", "Lentils", "Grocery", 120, 150),
        new("P3005", "Mustard Oil", "Grocery", 250, 300),
        new("P3006", "Ghee", "Grocery", 600, 800),

        // Cleaning
        new("P4001", "Harpic Toilet Cleaner", "Cleaning", 90, 120),
        new("P4002", "Vim Bar", "Cleaning", 20, 40),
        new("P4003", "Sunlight Detergent", "Cleaning", 60, 100),
        new("P4004", "Surf Excel", "Cleaning", 90, 130),
        new("P4005", "Phenyl", "Cleaning", 70, 100),
        new("P4006", "Colin Spray", "Cleaning", 110, 140),

        // Snacks
        new("P5001", "Snickers", "Snacks", 80, 100),
        new("P5002", "Lays Chips", "Snacks", 40, 60),
        new("P5003", "Bingo Mad Angles", "Snacks", 50, 70),
        new("P5004", "Oreos", "Snacks", 100, 120),
        new("P5005", "Momo Instant Noodles", "Snacks", 30, 50),
        new("P5006", "Maggi Noodles", "Snacks", 25, 45),
        };

        static void Main()
        {
            Console.Write("Enter number of branches: ");
            int branchCount = int.Parse(Console.ReadLine()!);

            Console.Write("Enter number of transactions per file: ");
            int transactionsPerFile = int.Parse(Console.ReadLine()!);


            string basePath = @"C:\TestFiles";
            FileInfo file = new(basePath);
            if (file.Directory?.Exists == false)
            {
                file.Directory.Create();
                //return;
            }

            Directory.CreateDirectory("Output");

            for (int i = 1; i <= branchCount; i++)
            {
                string branchCode = $"BBS{i:00}";
                string filePath = Path.Combine(basePath, $"{branchCode}.json");

                using StreamWriter writer = new(filePath);
                for (int j = 1; j <= transactionsPerFile; j++)
                {
                    var transaction = GenerateTransaction(j, branchCode);
                    string jsonLine = JsonSerializer.Serialize(transaction);
                    writer.WriteLine(jsonLine);
                }

                Console.WriteLine($"Generated file: {filePath}");
            }

            Console.WriteLine("✅ Done generating files.");
        }

        static object GenerateTransaction(int txnNumber, string branchCode)
        {
            string txnId = $"TXN{txnNumber:D6}";
            DateTime timestamp = DateTime.Today.AddMinutes(random.Next(0, 1440)); // Random time in the day
            string cashierId = $"CSH{random.Next(1, 10):000}";

            int itemCount = random.Next(1, 5); // 1 to 4 items per transaction
            List<object> items = new();
            double total = 0;

            for (int i = 0; i < itemCount; i++)
            {
                var product = Products[random.Next(Products.Count)];
                int quantity = random.Next(1, 5);
                double unitPrice = Math.Round(product.MinPrice + random.NextDouble() * (product.MaxPrice - product.MinPrice), 2);
                double itemTotal = quantity * unitPrice;
                total += itemTotal;

                items.Add(new
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Category = product.Category,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = Math.Round(itemTotal, 2)
                });
            }

            double discount = Math.Round(random.NextDouble() * 20, 2); // random discount
            double transactionTotal = total - discount;

            return new
            {
                TransactionID = txnId,
                Timestamp = timestamp.ToString("yyyy-MM-ddTHH:mm:ss"),
                BranchCode = branchCode,
                CashierID = cashierId,
                Items = items,
                PaymentMode = PaymentModes[random.Next(PaymentModes.Count)],
                DiscountApplied = discount,
                CustomerType = CustomerTypes[random.Next(CustomerTypes.Count)],
                TransactionTotal = Math.Round(transactionTotal, 2)
            };
        }

        class Product
        {
            public string ProductID { get; }
            public string ProductName { get; }
            public string Category { get; }
            public double MinPrice { get; }
            public double MaxPrice { get; }

            public Product(string id, string name, string category, double minPrice, double maxPrice)
            {
                ProductID = id;
                ProductName = name;
                Category = category;
                MinPrice = minPrice;
                MaxPrice = maxPrice;
            }
        }
    }

}
