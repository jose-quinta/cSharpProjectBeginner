namespace morse_code_translator.Models;

public class TranslationRecord
{
    public string Input { get; set; } = string.Empty;
    public string Output { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
