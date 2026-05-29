namespace guess_number.Models;

public class GameStats
{
    public int TotalGames { get; set; }
    public int TotalAttempts { get; set; }
    public int BestScore { get; set; }
    public string BestPlayer { get; set; } = string.Empty;
}
