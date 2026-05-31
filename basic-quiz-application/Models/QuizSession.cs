namespace basic_quiz_application.Models;

public class QuizSession
{
    public List<Question> Questions { get; set; } = new List<Question>();
    public int CurrentIndex { get; set; }
    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public List<int?> Answers { get; set; } = new List<int?>();
    public DateTime StartTime { get; set; }
    public string Category { get; set; } = "Todas";
    public bool IsFinished { get; set; }
}
