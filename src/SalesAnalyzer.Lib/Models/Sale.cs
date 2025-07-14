namespace SalesAnalyzer.Lib.Models
{
    public class Sale
    {
        public string Branch { get; set; }
        public string? CustomerId { get; set; }
        public string BillTo { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public DateTime BillDate { get; set; }
    }
}
