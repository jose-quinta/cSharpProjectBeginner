namespace word_counter.Models;

public class TextAnalysisResult
{
    public int WordCount { get; set; }
    public int CharacterCount { get; set; }
    public int CharacterCountNoSpaces { get; set; }
    public int LineCount { get; set; }
    public int SentenceCount { get; set; }
    public int ParagraphCount { get; set; }
    public double AverageWordLength { get; set; }
    public string LongestWord { get; set; } = string.Empty;
    public string ShortestWord { get; set; } = string.Empty;
    public double ReadingTimeMinutes { get; set; }
    public List<WordFrequency> TopWords { get; set; } = new();
    public string Language { get; set; } = "Desconocido";
    public string SourceName { get; set; } = string.Empty;
    public DateTime AnalyzedAt { get; set; } = DateTime.Now;
}
