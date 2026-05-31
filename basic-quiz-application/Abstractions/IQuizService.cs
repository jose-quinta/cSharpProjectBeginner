using basic_quiz_application.Models;

namespace basic_quiz_application.Abstractions;

public interface IQuizService
{
    QuizSession CreateSession(List<Question> questions, string category);
    Question? GetCurrentQuestion(QuizSession session);
    bool SubmitAnswer(QuizSession session, int answerIndex);
    void NextQuestion(QuizSession session);
    bool IsFinished(QuizSession session);
    (int score, int total, double percentage, TimeSpan duration, List<(Question question, int? userAnswer)> mistakes) GetResult(QuizSession session);
    List<Question> FilterByCategory(List<Question> questions, string category);
}
