namespace SalesAnalyzer.Lib.Models
{
    public class Branch(string branch)
    {
        public IList<Sale> Sales { get; } = new List<Sale>();

        public string LowestSellingCountItem { get; set; } = string.Empty;
        public string HighestSellingCountItem { get; set; } = string.Empty;
    }
}
