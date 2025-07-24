using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIngestorApp.Core.Models
{
    public class Product
    {
        public string ProductID { get; set; }
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Unit { get; set; }
        public string Supplier { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public bool IsPerishable { get; set; }
        public int ShelfLifeInDays { get; set; }
        public DateTime LaunchDate { get; set; }
        public bool DiscountEligible { get; set; }
        public int ReorderLevel { get; set; }
        public double GSTPercent { get; set; }
        public double PopularityScore { get; set; }

    }

}
