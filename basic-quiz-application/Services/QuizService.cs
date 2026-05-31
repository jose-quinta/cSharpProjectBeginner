using basic_quiz_application.Abstractions;
using basic_quiz_application.Models;

namespace basic_quiz_application.Services;

public class QuizService : IQuizService
{
    private static readonly Random Rng = new Random();

    public QuizSession CreateSession(List<Question> questions, string category)
    {
        List<Question> filtered = FilterByCategory(questions, category);
        List<Question> shuffled = filtered.OrderBy(_ => Rng.Next()).ToList();

        return new QuizSession
        {
            Questions = shuffled,
            CurrentIndex = 0,
            Score = 0,
            TotalQuestions = shuffled.Count,
            Answers = Enumerable.Repeat<int?>(null, shuffled.Count).ToList(),
            StartTime = DateTime.Now,
            Category = category,
            IsFinished = false
        };
    }

    public Question? GetCurrentQuestion(QuizSession session)
    {
        if (session.CurrentIndex < 0 || session.CurrentIndex >= session.TotalQuestions)
            return null;

        return session.Questions[session.CurrentIndex];
    }

    public bool SubmitAnswer(QuizSession session, int answerIndex)
    {
        Question? q = GetCurrentQuestion(session);
        if (q == null) return false;

        int zeroBased = answerIndex - 1;
        session.Answers[session.CurrentIndex] = zeroBased;

        bool isCorrect = zeroBased == q.CorrectIndex;
        if (isCorrect)
            session.Score++;

        return isCorrect;
    }

    public void NextQuestion(QuizSession session)
    {
        if (session.CurrentIndex < session.TotalQuestions - 1)
        {
            session.CurrentIndex++;
        }
        else
        {
            session.IsFinished = true;
        }
    }

    public bool IsFinished(QuizSession session)
    {
        return session.IsFinished;
    }

    public (int score, int total, double percentage, TimeSpan duration, List<(Question question, int? userAnswer)> mistakes) GetResult(QuizSession session)
    {
        TimeSpan duration = DateTime.Now - session.StartTime;
        double percentage = session.TotalQuestions > 0
            ? (double)session.Score / session.TotalQuestions * 100
            : 0;

        var mistakes = new List<(Question question, int? userAnswer)>();

        for (int i = 0; i < session.TotalQuestions; i++)
        {
            int? userAnswer = session.Answers[i];
            if (userAnswer != session.Questions[i].CorrectIndex)
            {
                mistakes.Add((session.Questions[i], userAnswer));
            }
        }

        return (session.Score, session.TotalQuestions, Math.Round(percentage, 1), duration, mistakes);
    }

    public List<Question> FilterByCategory(List<Question> questions, string category)
    {
        if (category == "Todas")
            return new List<Question>(questions);

        return questions.Where(q => q.Category == category).ToList();
    }
}
