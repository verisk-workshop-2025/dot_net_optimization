using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIngestorApp.Core.Models
{
    public class Item
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
}
