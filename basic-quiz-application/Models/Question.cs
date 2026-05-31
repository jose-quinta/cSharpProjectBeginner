namespace basic_quiz_application.Models;

public class Question
{
    public string Text { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new List<string>();
    public int CorrectIndex { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
}
