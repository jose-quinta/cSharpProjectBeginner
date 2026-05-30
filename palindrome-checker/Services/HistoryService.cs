using System.Text.Json;
using palindrome_checker.Models;

namespace palindrome_checker.Services;

public class HistoryService
{
    private const int MaxRecords = 10;
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "palindrome_history.json");

    public void AddRecord(AnalysisResult record)
    {
        List<AnalysisResult> records = Load();
        records.Insert(0, record);
        if (records.Count > MaxRecords)
            records = records.Take(MaxRecords).ToList();
        Save(records);
    }

    public List<AnalysisResult> Load()
    {
        if (!File.Exists(FilePath))
            return new List<AnalysisResult>();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<AnalysisResult>>(json) ?? new List<AnalysisResult>();
    }

    public void Save(List<AnalysisResult> records)
    {
        string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
