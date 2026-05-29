using System.Text.Json;
using guess_number.Models;

namespace guess_number.Services;

public class StatsService
{
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "stats.json");

    public GameStats Load()
    {
        if (!File.Exists(FilePath))
            return new GameStats();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<GameStats>(json) ?? new GameStats();
    }

    public void Save(GameStats stats)
    {
        string json = JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public void RecordGame(int attempts, string playerName, ref GameStats stats)
    {
        stats.TotalGames++;
        stats.TotalAttempts += attempts;

        if (stats.BestScore == 0 || attempts < stats.BestScore)
        {
            stats.BestScore = attempts;
            stats.BestPlayer = playerName;
        }
    }

    public void Clear(ref GameStats stats)
    {
        stats = new GameStats();
    }
}
