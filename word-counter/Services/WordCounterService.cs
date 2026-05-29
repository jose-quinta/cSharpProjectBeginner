using System.Text.RegularExpressions;
using word_counter.Abstractions;
using word_counter.Models;

namespace word_counter.Services;

public class WordCounterService : IWordCounterService
{
    private static readonly Regex WordPattern = new(@"\p{L}+", RegexOptions.Compiled);
    private static readonly Regex SentencePattern = new(@"[.!?](?:\s|$)", RegexOptions.Compiled);

    private static readonly HashSet<string> SpanishStopWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "el", "la", "los", "las", "de", "del", "y", "o", "a", "ante", "bajo",
        "con", "contra", "desde", "en", "entre", "hacia", "hasta", "para",
        "por", "segun", "sin", "sobre", "tras", "que", "es", "no", "su",
        "al", "lo", "como", "mas", "pero", "sus", "le", "ya", "este", "esta",
        "entre", "porque", "era", "son", "han", "puede", "habia", "tambien",
        "fue", "todo", "muy", "pero", "si", "cada", "ella", "ello", "ellos",
        "nos", "nosotros", "vos", "vosotros", "tu", "te", "ti", "mi", "me",
        "se", "le", "les", "uno", "dos", "tres", "un", "una", "unas", "unos",
        "otro", "otra", "otros", "otras", "mismo", "misma", "tanto", "tan",
        "bien", "cuando", "donde", "quien", "cual", "cuanto", "aquel",
        "aunque", "asi", "aun", "aqui", "alli", "alla", "acerca",
        "ahora", "antes", "despues", "durante", "finalmente", "pronto",
        "siempre", "tarde", "temprano", "todavia", "ya"
    };

    private static readonly HashSet<string> EnglishStopWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "the", "and", "of", "to", "a", "in", "is", "it", "you", "that",
        "he", "was", "for", "on", "are", "as", "with", "his", "they", "at",
        "be", "this", "from", "or", "have", "an", "by", "not", "but", "we",
        "which", "she", "do", "their", "if", "will", "would", "about", "up",
        "all", "can", "her", "has", "been", "could", "its", "more",
        "some", "there", "than", "been", "each", "may", "most", "other",
        "into", "also", "after", "did", "made", "only", "over", "such",
        "very", "when", "where", "how", "what", "who", "why", "no", "so",
        "just", "because", "out", "them", "then", "these", "those", "through",
        "me", "my", "am", "were", "been", "being", "own", "same", "too"
    };

    public TextAnalysisResult Analyze(string text, string sourceName, bool ignoreStopWords)
    {
        if (string.IsNullOrEmpty(text))
        {
            return new TextAnalysisResult
            {
                SourceName = sourceName,
                AnalyzedAt = DateTime.Now,
                Language = DetectLanguage(text ?? "")
            };
        }

        MatchCollection wordMatches = WordPattern.Matches(text);
        List<string> words = wordMatches.Select(m => m.Value.ToLower()).ToList();

        int wordCount = words.Count;
        int charCount = text.Length;
        int charCountNoSpaces = text.Count(c => !char.IsWhiteSpace(c));
        int lineCount = text.Split('\n').Length;
        int sentenceCount = SentencePattern.Matches(text).Count;
        if (sentenceCount == 0 && wordCount > 0) sentenceCount = 1;
        int paragraphCount = text.Split(new[] { "\n\n", "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length;
        if (paragraphCount == 0 && wordCount > 0) paragraphCount = 1;

        double avgWordLength = wordCount > 0 ? words.Average(w => (double)w.Length) : 0;
        string longest = wordCount > 0 ? words.OrderByDescending(w => w.Length).First() : "";
        string shortest = wordCount > 0 ? words.Where(w => w.Length > 0).OrderBy(w => w.Length).First() : "";

        List<WordFrequency> topWords = GetTopWords(text, 10, ignoreStopWords);

        string language = DetectLanguage(text);

        return new TextAnalysisResult
        {
            WordCount = wordCount,
            CharacterCount = charCount,
            CharacterCountNoSpaces = charCountNoSpaces,
            LineCount = lineCount,
            SentenceCount = sentenceCount,
            ParagraphCount = paragraphCount,
            AverageWordLength = Math.Round(avgWordLength, 2),
            LongestWord = longest,
            ShortestWord = shortest,
            ReadingTimeMinutes = Math.Round(wordCount / 200.0, 2),
            TopWords = topWords,
            Language = language,
            SourceName = sourceName,
            AnalyzedAt = DateTime.Now
        };
    }

    public List<WordFrequency> GetTopWords(string text, int count, bool ignoreStopWords)
    {
        if (string.IsNullOrEmpty(text))
            return new List<WordFrequency>();

        MatchCollection wordMatches = WordPattern.Matches(text);
        HashSet<string>? stopWords = null;

        if (ignoreStopWords)
        {
            string lang = DetectLanguage(text);
            stopWords = lang == "Ingl\u00e9s" ? EnglishStopWords : SpanishStopWords;
        }

        var frequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (Match match in wordMatches)
        {
            string word = match.Value.ToLower();
            if (stopWords?.Contains(word) == true)
                continue;
            frequency.TryGetValue(word, out int current);
            frequency[word] = current + 1;
        }

        return frequency
            .OrderByDescending(kvp => kvp.Value)
            .ThenBy(kvp => kvp.Key)
            .Take(count)
            .Select(kvp => new WordFrequency { Word = kvp.Key, Count = kvp.Value })
            .ToList();
    }

    public string DetectLanguage(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "Desconocido";

        MatchCollection wordMatches = WordPattern.Matches(text);
        int spanishScore = 0;
        int englishScore = 0;

        foreach (Match match in wordMatches)
        {
            string word = match.Value.ToLower();
            if (SpanishStopWords.Contains(word)) spanishScore++;
            if (EnglishStopWords.Contains(word)) englishScore++;
        }

        if (spanishScore == 0 && englishScore == 0)
            return "Desconocido";

        if (spanishScore > englishScore * 1.2)
            return "Espa\u00f1ol";
        if (englishScore > spanishScore * 1.2)
            return "Ingl\u00e9s";

        return "Indeterminado (mixto)";
    }
}
