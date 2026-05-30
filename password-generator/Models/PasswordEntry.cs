namespace password_generator.Models;

public class PasswordEntry
{
    public string Password { get; set; } = string.Empty;
    public int Length { get; set; }
    public bool HasUpper { get; set; }
    public bool HasLower { get; set; }
    public bool HasDigit { get; set; }
    public bool HasSymbol { get; set; }
    public string Strength { get; set; } = string.Empty;
    public double Entropy { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
