using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using palindrome_checker.Abstractions;
using palindrome_checker.Models;

namespace palindrome_checker.Services;

public class PalindromeService : IPalindromeService
{
    private static readonly Regex WordSplitter = new(@"\p{L}+", RegexOptions.Compiled);

    public AnalysisResult Check(string text, string category)
    {
        string cleaned = Normalize(text);
        string reversed = new string(text.Reverse().ToArray());
        string cleanedReversed = new string(cleaned.Reverse().ToArray());
        bool isPalindrome = cleaned.Length > 0 && cleaned == cleanedReversed;

        return new AnalysisResult
        {
            InputText = text,
            IsPalindrome = isPalindrome,
            ReversedText = reversed,
            CleanedText = cleaned,
            Length = text.Length,
            Category = category,
            Timestamp = DateTime.Now
        };
    }

    public AnalysisResult CheckAll(string text)
    {
        var found = new List<string>();
        MatchCollection matches = WordSplitter.Matches(text);

        foreach (Match match in matches)
        {
            string word = match.Value;
            AnalysisResult r = Check(word, "Palabra");
            if (r.IsPalindrome)
                found.Add(word);
        }

        return new AnalysisResult
        {
            InputText = text,
            IsPalindrome = found.Count > 0,
            ReversedText = new string(text.Reverse().ToArray()),
            CleanedText = Normalize(text),
            Length = text.Length,
            Category = "Texto",
            PalindromesFound = found,
            Timestamp = DateTime.Now
        };
    }

    public string GetReversed(string text)
    {
        return new string(text.Reverse().ToArray());
    }

    public string Normalize(string text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;

        string normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (char c in normalized)
        {
            UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);

            if (category != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        string result = sb.ToString().Normalize(NormalizationForm.FormC);
        result = result.ToLowerInvariant();
        result = result.Replace("ñ", "n");

        var filtered = new StringBuilder();
        foreach (char c in result)
        {
            if (char.IsLetter(c) || char.IsDigit(c))
                filtered.Append(c);
        }

        return filtered.ToString();
    }

    public PalindromeStats GetStats(List<AnalysisResult> history)
    {
        if (history.Count == 0)
            return new PalindromeStats();

        int total = history.Count;
        int palindromeCount = history.Count(h => h.IsPalindrome);

        string longest = history
            .Where(h => h.IsPalindrome)
            .OrderByDescending(h => h.CleanedText.Length)
            .Select(h => h.InputText)
            .FirstOrDefault() ?? string.Empty;

        return new PalindromeStats
        {
            TotalChecked = total,
            PalindromeCount = palindromeCount,
            LongestPalindrome = longest,
            LastChecked = history.Max(h => h.Timestamp)
        };
    }
}
