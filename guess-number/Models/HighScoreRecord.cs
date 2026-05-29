namespace guess_number.Models;

public class HighScoreRecord
{
    public string PlayerName { get; set; } = string.Empty;
    public int Attempts { get; set; }
    public int SecretNumber { get; set; }
    public DateTime Date { get; set; }
}
