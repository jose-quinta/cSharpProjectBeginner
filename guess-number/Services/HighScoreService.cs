using System.Text.Json;
using guess_number.Models;

namespace guess_number.Services;

public class HighScoreService
{
    private const int MaxRecords = 5;
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "highscores.json");

    public static readonly string FileName = "highscores.json";

    public List<HighScoreRecord> Load()
    {
        if (!File.Exists(FilePath))
            return new List<HighScoreRecord>();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<HighScoreRecord>>(json) ?? new List<HighScoreRecord>();
    }

    public void Save(List<HighScoreRecord> scores)
    {
        string json = JsonSerializer.Serialize(scores, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public bool IsHighScore(int attempts, List<HighScoreRecord> scores)
    {
        if (scores.Count < MaxRecords)
            return true;
        return attempts < scores.Max(s => s.Attempts);
    }

    public List<HighScoreRecord> AddHighScore(string name, int attempts, int secretNumber, List<HighScoreRecord> scores)
    {
        scores.Add(new HighScoreRecord
        {
            PlayerName = name,
            Attempts = attempts,
            SecretNumber = secretNumber,
            Date = DateTime.Now
        });

        return scores.OrderBy(s => s.Attempts).ThenBy(s => s.Date).Take(MaxRecords).ToList();
    }

    public void Clear()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}
