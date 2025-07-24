using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIngestorApp.Core.Models
{
    public class Transaction
    {
        public string TransactionID { get; set; }
        public string Timestamp { get; set; }
        public string BranchCode { get; set; }
        public string CashierID { get; set; }
        public List<Item> Items { get; set; }
        public string PaymentMode { get; set; }
        public double DiscountApplied { get; set; }
        public string CustomerType { get; set; }
        public double TransactionTotal { get; set; }
        public DateTime TransactionDate
        {
            get
            {
                return DateTime.Parse(Timestamp);
            }
        }
    }
}
