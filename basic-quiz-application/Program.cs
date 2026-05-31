using basic_quiz_application.Abstractions;
using basic_quiz_application.Models;
using basic_quiz_application.Services;

namespace basic_quiz_application;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        QuestionLoaderService questionLoader = new QuestionLoaderService();
        QuizService quizService = new QuizService();
        HighScoreService highScoreService = new HighScoreService();
        MenuService menu = new MenuService();

        List<Question> allQuestions = questionLoader.Load();

        while (true)
        {
            menu.ShowBanner();
            menu.ShowMainMenu();

            string choice = menu.GetChoice();
            Console.WriteLine(choice);

            switch (choice)
            {
                case "1":
                    RunQuiz(quizService, highScoreService, menu, allQuestions);
                    break;
                case "2":
                    menu.ShowHighScores(highScoreService.Load());
                    menu.Pause();
                    break;
                case "3":
                    menu.ShowQuestionBank(allQuestions);
                    menu.Pause();
                    break;
                case "4":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n ¡Hasta luego!");
                    Console.ResetColor();
                    return;
                default:
                    menu.ShowError("Opción no válida. Presione una tecla.");
                    continue;
            }
        }
    }

    private static void RunQuiz(IQuizService quizService, HighScoreService highScoreService, MenuService menu,
        List<Question> allQuestions)
    {
        List<string> categories = new QuestionLoaderService().GetCategories(allQuestions);
        string category = menu.SelectCategory(categories);

        QuizSession session = quizService.CreateSession(allQuestions, category);

        if (session.TotalQuestions == 0)
        {
            menu.ShowError("No hay preguntas disponibles para esta categoría.");
            menu.Pause();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n Comenzando quiz: {session.TotalQuestions} preguntas | categoría: {category}");
        Console.ResetColor();
        Console.WriteLine(" Presione cualquier tecla para comenzar...");
        Console.ReadKey(true);

        while (!quizService.IsFinished(session))
        {
            Question? q = quizService.GetCurrentQuestion(session);
            if (q == null) break;

            Console.Clear();
            menu.ShowQuizHeader(session);
            menu.ShowQuestion(q);

            int answer = menu.GetAnswer(q.Options.Count);

            if (answer == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" ⚠ Pregunta saltada.");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($" Respuesta correcta: {q.Options[q.CorrectIndex]}");
                Console.ResetColor();
                Console.WriteLine();
            }
            else
            {
                bool correct = quizService.SubmitAnswer(session, answer);
                menu.ShowFeedback(correct, q, answer);
            }

            quizService.NextQuestion(session);

            if (!quizService.IsFinished(session))
            {
                Console.Write(" Presione cualquier tecla para continuar...");
                Console.ReadKey(true);
            }
        }

        Console.Clear();
        var result = quizService.GetResult(session);
        menu.ShowResult(result.score, result.total, result.percentage, result.duration, result.mistakes);

        if (result.total > 0)
        {
            string name = menu.GetPlayerName();
            highScoreService.AddScore(new HighScoreRecord
            {
                PlayerName = name,
                Score = result.score,
                Total = result.total,
                Percentage = result.percentage,
                Category = category,
                Date = DateTime.Now
            });
        }

        menu.Pause();
    }
}
