namespace BatchFileProcessing.Core.Models;

public class BranchProcessingResult
{
    public string BranchCode { get; set; }
    public string SummaryText { get; set; }
    public TimeSpan ProcessingTime { get; set; }
    public string ErrorMessage { get; set; }
    public bool Success => string.IsNullOrEmpty(ErrorMessage);
}
