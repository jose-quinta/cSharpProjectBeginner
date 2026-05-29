using guess_number.Abstractions;
using guess_number.Models;

namespace guess_number.Services;

public class MenuService
{
    private const string TitleColor = "\u001b[33m";
    private const string ResetColor = "\u001b[0m";
    private const string Green = "\u001b[32m";
    private const string RedColor = "\u001b[31m";
    private const string Cyan = "\u001b[36m";
    private const string Magenta = "\u001b[35m";

    public (int min, int max, int maxAttempts, string name) ShowDifficultyMenu()
    {
        Console.Clear();
        Console.WriteLine($"{TitleColor}=== ADIVINA EL N\u00daMERO ==={ResetColor}\n");
        Console.WriteLine("Elige dificultad:");
        Console.WriteLine($"{Green}1.{ResetColor} F\u00e1cil  (1-50,  15 intentos)");
        Console.WriteLine($"{Cyan}2.{ResetColor} Medio   (1-100, 10 intentos)");
        Console.WriteLine($"{Magenta}3.{ResetColor} Dif\u00edcil (1-500, 20 intentos)\n");

        while (true)
        {
            Console.Write("Opci\u00f3n (1-3): ");
            string input = Console.ReadLine() ?? "";
            switch (input.Trim())
            {
                case "1": return (1, 50, 15, "F\u00e1cil");
                case "2": return (1, 100, 10, "Medio");
                case "3": return (1, 500, 20, "Dif\u00edcil");
                default: Console.WriteLine("Opci\u00f3n inv\u00e1lida."); break;
            }
        }
    }

    public int GetGuess(IReadOnlyList<int> history)
    {
        while (true)
        {
            if (history.Count > 0)
            {
                Console.Write($"Intento #{history.Count + 1} (previos: {string.Join(", ", history)}): ");
            }
            else
            {
                Console.Write("Ingresa tu n\u00famero: ");
            }

            string input = Console.ReadLine() ?? "";
            if (!int.TryParse(input.Trim(), out int guess))
            {
                Console.WriteLine("Por favor, ingresa un n\u00famero v\u00e1lido.");
                continue;
            }

            if (history.Contains(guess))
            {
                Console.WriteLine("Ya intentaste ese n\u00famero. Prueba otro.");
                continue;
            }

            return guess;
        }
    }

    public void ShowHint(GuessResult result, string temperature, ConsoleColor color, int guess, int secretNumber, int attempts, int maxAttempts)
    {
        Console.ForegroundColor = color;
        switch (result)
        {
            case GuessResult.Lower:
                Console.WriteLine($"{guess} -> El n\u00famero es MAYOR  | {temperature}");
                break;
            case GuessResult.Higher:
                Console.WriteLine($"{guess} -> El n\u00famero es MENOR | {temperature}");
                break;
            case GuessResult.GameOver:
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n\u00a1Game Over! El n\u00famero era {secretNumber}. Usaste {attempts}/{maxAttempts} intentos.");
                break;
        }
        Console.ResetColor();
    }

    public void ShowWin(int attempts, int secretNumber, TimeSpan elapsed, int maxAttempts)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n\u00a1Felicidades! Adivinaste el {secretNumber} en {attempts} intentos.");
        Console.WriteLine($"Tiempo: {elapsed.Minutes}m {elapsed.Seconds}s ({elapsed.TotalSeconds:F0}s)");
        Console.ResetColor();
    }

    public void ShowAttempts(int attempts, int maxAttempts)
    {
        int remaining = maxAttempts - attempts;
        Console.ForegroundColor = remaining <= 3 ? ConsoleColor.Red : ConsoleColor.DarkGray;
        Console.WriteLine($"[Intento {attempts}/{maxAttempts} - Restan {remaining}]\n");
        Console.ResetColor();
    }

    public void ShowHighScores(List<HighScoreRecord> scores, bool isNewRecord)
    {
        Console.WriteLine("\n--- MEJORES PUNTAJES ---");

        if (scores.Count == 0)
        {
            Console.WriteLine("(sin registros)");
            return;
        }

        for (int i = 0; i < scores.Count; i++)
        {
            var s = scores[i];
            string prefix = (isNewRecord && i == 0 && s.Attempts == scores.Min(x => x.Attempts))
                ? "\u2b50 "
                : $"{i + 1}. ";
            Console.WriteLine($"{prefix}{s.PlayerName} - {s.Attempts} intentos (nro {s.SecretNumber}, {s.Date:dd/MM/yy})");
        }
    }

    public string GetPlayerName()
    {
        Console.Write("Ingresa tu nombre (m\u00e1x 15 caracteres): ");
        string name = Console.ReadLine() ?? "";
        if (name.Length > 15) name = name[..15];
        if (string.IsNullOrWhiteSpace(name)) name = "An\u00f3nimo";
        return name.Trim();
    }

    public bool AskPlayAgain()
    {
        Console.Write("\n\u00bfJugar de nuevo? [S/N]: ");
        var key = Console.ReadKey(true);
        Console.WriteLine(key.KeyChar);
        return key.KeyChar is 's' or 'S';
    }

    public bool AskClearScores()
    {
        Console.Write("\n\u00bfBorrar todos los r\u00e9cords? [S/N]: ");
        var key = Console.ReadKey(true);
        Console.WriteLine(key.KeyChar);
        return key.KeyChar is 's' or 'S';
    }

    public void ShowStats(int totalGames, int totalAttempts, double average, int bestScore, string bestPlayer)
    {
        Console.WriteLine($"\n--- ESTAD\u00cdSTICAS ---");
        Console.WriteLine($"Partidas jugadas: {totalGames}");
        Console.WriteLine($"Total intentos:   {totalAttempts}");
        Console.WriteLine($"Promedio:         {average:F1} intentos/partida");
        if (bestScore > 0)
            Console.WriteLine($"Mejor puntaje:    {bestScore} intentos ({bestPlayer})");
        else
            Console.WriteLine($"Mejor puntaje:    (a\u00fan no hay)");
    }

    public bool AskShowStats()
    {
        Console.Write("\n\u00bfVer estad\u00edsticas? [S/N]: ");
        var key = Console.ReadKey(true);
        Console.WriteLine(key.KeyChar);
        return key.KeyChar is 's' or 'S';
    }
}
