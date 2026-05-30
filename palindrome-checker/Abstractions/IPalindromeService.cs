using palindrome_checker.Models;

namespace palindrome_checker.Abstractions;

public interface IPalindromeService
{
    AnalysisResult Check(string text, string category);
    AnalysisResult CheckAll(string text);
    string GetReversed(string text);
    string Normalize(string text);
    PalindromeStats GetStats(List<AnalysisResult> history);
}
