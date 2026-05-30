namespace palindrome_checker.Models;

public class AnalysisResult
{
    public string InputText { get; set; } = string.Empty;
    public bool IsPalindrome { get; set; }
    public string ReversedText { get; set; } = string.Empty;
    public string CleanedText { get; set; } = string.Empty;
    public int Length { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> PalindromesFound { get; set; } = new();
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
