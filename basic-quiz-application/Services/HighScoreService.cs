using System.Text.Json;
using basic_quiz_application.Models;

namespace basic_quiz_application.Services;

public class HighScoreService
{
    private const int MaxRecords = 10;
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "highscores-quiz.json");

    public void AddScore(HighScoreRecord record)
    {
        List<HighScoreRecord> records = Load();
        records.Add(record);
        records = records.OrderByDescending(r => r.Percentage).ThenByDescending(r => r.Score).Take(MaxRecords).ToList();
        Save(records);
    }

    public List<HighScoreRecord> Load()
    {
        if (!File.Exists(FilePath))
            return new List<HighScoreRecord>();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<HighScoreRecord>>(json) ?? new List<HighScoreRecord>();
    }

    public void Save(List<HighScoreRecord> records)
    {
        string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public void Clear()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}
