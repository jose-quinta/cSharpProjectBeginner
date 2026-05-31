using basic_quiz_application.Models;

namespace basic_quiz_application.Services;

public class MenuService
{
    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║        BASIC QUIZ APPLICATION        ║");
        Console.WriteLine(@"║     Pon a prueba tus conocimientos   ║");
        Console.WriteLine(@"╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" MENÚ PRINCIPAL");
        Console.ResetColor();
        Console.WriteLine(" ────────────────────────────");
        Console.WriteLine(" [1] Iniciar quiz");
        Console.WriteLine(" [2] Puntajes altos");
        Console.WriteLine(" [3] Ver banco de preguntas");
        Console.WriteLine(" [4] Salir");
        Console.WriteLine();
        Console.Write(" Seleccione una opción: ");
    }

    public string GetChoice()
    {
        var key = Console.ReadKey(true);
        return key.Key.ToString() switch
        {
            "D1" or "NumPad1" => "1",
            "D2" or "NumPad2" => "2",
            "D3" or "NumPad3" => "3",
            "D4" or "NumPad4" => "4",
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "i" => "1",
                "p" => "2",
                "b" => "3",
                "s" => "4",
                _ => ""
            }
        };
    }

    public string SelectCategory(List<string> categories)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" Seleccione una categoría:");
        Console.ResetColor();
        Console.WriteLine(" ────────────────────────────");
        Console.WriteLine(" [0] Todas");
        for (int i = 0; i < categories.Count; i++)
            Console.WriteLine($" [{i + 1}] {categories[i]}");
        Console.WriteLine();

        while (true)
        {
            Console.Write(" Categoría: ");
            string? input = Console.ReadLine()?.Trim();
            if (input == "0") return "Todas";
            if (int.TryParse(input, out int idx) && idx >= 1 && idx <= categories.Count)
                return categories[idx - 1];
            ShowError("Opción inválida.");
        }
    }

    public void ShowQuizHeader(QuizSession session)
    {
        Question? q = session.Questions.ElementAtOrDefault(session.CurrentIndex);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($" Pregunta {session.CurrentIndex + 1}/{session.TotalQuestions}");
        Console.ResetColor();
        Console.WriteLine($" Aciertos: {session.Score}  |  Categoría: {q?.Category ?? "?"}  |  Dificultad: {q?.Difficulty ?? "?"}");
        Console.WriteLine($" {new string('═', 50)}");
        Console.WriteLine();
    }

    public void ShowQuestion(Question q)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($" {q.Text}");
        Console.ResetColor();
        Console.WriteLine();

        for (int i = 0; i < q.Options.Count; i++)
        {
            Console.WriteLine($" [{i + 1}] {q.Options[i]}");
        }
        Console.WriteLine();
        Console.WriteLine(" [0] Saltar pregunta");
        Console.WriteLine();
    }

    public int GetAnswer(int maxOption)
    {
        while (true)
        {
            Console.Write(" Respuesta (0-{0}): ", maxOption);
            string? input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out int value) && value >= 0 && value <= maxOption)
                return value;
            ShowError($"Debe ingresar un número entre 0 y {maxOption}.");
        }
    }

    public void ShowFeedback(bool correct, Question q, int chosenIndex)
    {
        Console.WriteLine();
        if (correct)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" ✓ ¡Correcto!");
            Console.ResetColor();
        }
        else if (chosenIndex == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" ⚠ Pregunta saltada.");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($" Respuesta correcta: {q.Options[q.CorrectIndex]}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" ✗ Incorrecto.");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($" Respuesta correcta: {q.Options[q.CorrectIndex]}");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    public void ShowResult(int score, int total, double percentage, TimeSpan duration, List<(Question question, int? userAnswer)> mistakes)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         RESULTADOS                   ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        Console.ForegroundColor = percentage >= 80 ? ConsoleColor.Green :
                                   percentage >= 50 ? ConsoleColor.Yellow :
                                   ConsoleColor.Red;
        Console.WriteLine($" Puntaje: {score}/{total} ({percentage}%)");
        Console.ResetColor();
        Console.WriteLine($" Tiempo:  {duration.Minutes}:{duration.Seconds:D2} min");
        Console.WriteLine();

        if (mistakes.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" Preguntas incorrectas:");
            Console.ResetColor();
            for (int i = 0; i < mistakes.Count; i++)
            {
                var (q, userAnswer) = mistakes[i];
                string userText = userAnswer.HasValue && userAnswer.Value >= 0 && userAnswer.Value < q.Options.Count
                    ? q.Options[userAnswer.Value]
                    : "(saltada)";
                Console.WriteLine($"  {i + 1}. \"{q.Text}\"");
                Console.WriteLine($"     Tu respuesta: {userText}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"     Correcta:     {q.Options[q.CorrectIndex]}");
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        string grade = percentage >= 90 ? "¡Excelente!" :
                       percentage >= 70 ? "¡Muy bien!" :
                       percentage >= 50 ? "Bien" :
                       "Sigue practicando";
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($" {grade}");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowHighScores(List<HighScoreRecord> records)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         PUNTAJES ALTOS               ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        if (records.Count == 0)
        {
            Console.WriteLine(" (No hay puntajes registrados)");
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < records.Count; i++)
        {
            var r = records[i];
            Console.WriteLine($"  #{i + 1,-3} {r.PlayerName,-15} {r.Score,2}/{r.Total,-3} ({r.Percentage,5:F1}%)  {r.Category,-12} {r.Date:dd/MM/yy}");
        }
        Console.WriteLine();
    }

    public void ShowQuestionBank(List<Question> questions)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         BANCO DE PREGUNTAS           ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var grouped = questions.GroupBy(q => q.Category);
        foreach (var group in grouped)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" ─── {group.Key} ───");
            Console.ResetColor();

            int i = 1;
            foreach (var q in group)
            {
                string correct = q.Options[q.CorrectIndex];
                Console.WriteLine($"  {i}. {q.Text} [{q.Difficulty}]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"     → {correct}");
                Console.ResetColor();
                i++;
            }
            Console.WriteLine();
        }
    }

    public string GetPlayerName()
    {
        Console.Write(" Ingrese su nombre: ");
        string? name = Console.ReadLine()?.Trim();
        return string.IsNullOrEmpty(name) ? "Anónimo" : name;
    }

    public bool ConfirmAction(string prompt)
    {
        Console.Write($" {prompt} (s/n): ");
        return Console.ReadLine()?.Trim().ToLower() == "s";
    }

    public void Pause()
    {
        Console.Write("\n Presione cualquier tecla para continuar...");
        Console.ReadKey(true);
        Console.WriteLine();
    }

    public void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" Error: {message}");
        Console.ResetColor();
    }
}
