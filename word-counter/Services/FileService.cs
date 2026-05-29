using System.Text.Json;
using word_counter.Models;

namespace word_counter.Services;

public class FileService
{
    private const int MaxHistory = 10;
    private static readonly string HistoryPath = Path.Combine(Directory.GetCurrentDirectory(), "analysis_history.json");

    public string? ReadTextFile(string path)
    {
        if (!File.Exists(path))
            return null;

        return File.ReadAllText(path);
    }

    public List<string> GetTextFiles(string directory)
    {
        if (!Directory.Exists(directory))
            return new List<string>();

        return Directory.GetFiles(directory, "*.txt")
            .Select(f => Path.GetFileName(f))
            .ToList();
    }

    public void ExportResult(TextAnalysisResult result)
    {
        string timestamp = result.AnalyzedAt.ToString("yyyyMMdd_HHmmss");
        string fileName = $"analysis_{result.SourceName.Replace(".txt", "")}_{timestamp}.txt";
        fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

        string content = FormatResultForExport(result);
        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), content);
    }

    public void SaveToHistory(TextAnalysisResult result)
    {
        List<TextAnalysisResult> history = LoadHistory();
        history.Insert(0, result);

        if (history.Count > MaxHistory)
            history = history.Take(MaxHistory).ToList();

        string json = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(HistoryPath, json);
    }

    public List<TextAnalysisResult> LoadHistory()
    {
        if (!File.Exists(HistoryPath))
            return new List<TextAnalysisResult>();

        string json = File.ReadAllText(HistoryPath);
        return JsonSerializer.Deserialize<List<TextAnalysisResult>>(json) ?? new List<TextAnalysisResult>();
    }

    private static string FormatResultForExport(TextAnalysisResult r)
    {
        var lines = new List<string>
        {
            "========================================",
            $"  WORD COUNTER - RESULTADO DE AN\u00c1LISIS",
            "========================================",
            $"",
            $"  Fuente:              {r.SourceName}",
            $"  Analizado:           {r.AnalyzedAt:dd/MM/yyyy HH:mm:ss}",
            $"  Idioma detectado:    {r.Language}",
            $"",
            $"  --- ESTAD\u00cdSTICAS ---",
            $"  Palabras:            {r.WordCount:N0}",
            $"  Caracteres:          {r.CharacterCount:N0}",
            $"  Caracteres (s/sp):   {r.CharacterCountNoSpaces:N0}",
            $"  L\u00edneas:             {r.LineCount:N0}",
            $"  Oraciones:           {r.SentenceCount:N0}",
            $"  P\u00e1rrafos:           {r.ParagraphCount:N0}",
            $"",
            $"  Palabra m\u00e1s larga:   \"{r.LongestWord}\" ({r.LongestWord.Length} letras)",
            $"  Palabra m\u00e1s corta:  \"{r.ShortestWord}\" ({r.ShortestWord.Length} letras)",
            $"  Promedio:            {r.AverageWordLength} letras/palabra",
            $"  Tiempo lectura:      {r.ReadingTimeMinutes} min",
            $"",
            $"  --- TOP 10 PALABRAS ---"
        };

        for (int i = 0; i < r.TopWords.Count; i++)
        {
            var w = r.TopWords[i];
            lines.Add($"  {i + 1,2}. \"{w.Word}\" -> {w.Count} veces");
        }

        lines.Add("========================================");
        return string.Join(Environment.NewLine, lines);
    }
}
