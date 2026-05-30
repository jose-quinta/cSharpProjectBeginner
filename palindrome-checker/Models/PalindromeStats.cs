namespace palindrome_checker.Models;

public class PalindromeStats
{
    public int TotalChecked { get; set; }
    public int PalindromeCount { get; set; }
    public string LongestPalindrome { get; set; } = string.Empty;
    public DateTime LastChecked { get; set; }
}
