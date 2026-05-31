namespace basic_quiz_application.Models;

public class HighScoreRecord
{
    public string PlayerName { get; set; } = string.Empty;
    public int Score { get; set; }
    public int Total { get; set; }
    public double Percentage { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
}
