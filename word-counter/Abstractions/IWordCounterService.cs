using word_counter.Models;

namespace word_counter.Abstractions;

public interface IWordCounterService
{
    TextAnalysisResult Analyze(string text, string sourceName, bool ignoreStopWords);
    List<WordFrequency> GetTopWords(string text, int count, bool ignoreStopWords);
    string DetectLanguage(string text);
}
