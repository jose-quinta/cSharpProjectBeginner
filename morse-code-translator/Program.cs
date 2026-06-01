using morse_code_translator.Abstractions;
using morse_code_translator.Models;
using morse_code_translator.Services;

namespace morse_code_translator;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var morseService = new MorseService();
        var menu = new MenuService();

        while (true)
        {
            Console.Clear();
            menu.ShowBanner();
            menu.ShowMainMenu();

            string choice = menu.GetChoice();
            Console.WriteLine(choice);

            switch (choice)
            {
                case "1":
                    TranslateTextToMorse(morseService, menu);
                    break;
                case "2":
                    TranslateMorseToText(morseService, menu);
                    break;
                case "3":
                    ShowChart(morseService, menu);
                    break;
                case "4":
                    ShowHistory(morseService, menu);
                    break;
                case "5":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n ¡Hasta luego!");
                    Console.ResetColor();
                    return;
                default:
                    menu.ShowError("Opción no válida.");
                    menu.Pause();
                    continue;
            }
        }
    }

    private static void TranslateTextToMorse(IMorseService morseService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║       TEXTO  →  MORSE               ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        string input = menu.GetInput("Ingrese el texto a traducir:");
        if (string.IsNullOrEmpty(input))
        {
            menu.ShowError("El texto no puede estar vacío.");
            menu.Pause();
            return;
        }

        if (!morseService.IsValidText(input))
        {
            menu.ShowError("El texto contiene caracteres no soportados. Use solo letras, números y puntuación básica.");
            menu.Pause();
            return;
        }

        string output = morseService.TextToMorse(input);
        menu.ShowTranslationResult(input, output, "Texto → Morse");

        morseService.AddToHistory(new TranslationRecord
        {
            Input = input, Output = output, Direction = "Texto → Morse"
        });

        menu.Pause();
    }

    private static void TranslateMorseToText(IMorseService morseService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║       MORSE  →  TEXTO               ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        string input = menu.GetInput("Ingrese el código Morse (use espacios entre letras, ' / ' entre palabras):");
        if (string.IsNullOrEmpty(input))
        {
            menu.ShowError("El código Morse no puede estar vacío.");
            menu.Pause();
            return;
        }

        if (!morseService.IsValidMorse(input))
        {
            menu.ShowError("El código Morse contiene caracteres inválidos. Use solo . - espacios y /.");
            menu.Pause();
            return;
        }

        string output = morseService.MorseToText(input);
        menu.ShowTranslationResult(input, output, "Morse → Texto");

        morseService.AddToHistory(new TranslationRecord
        {
            Input = input, Output = output, Direction = "Morse → Texto"
        });

        menu.Pause();
    }

    private static void ShowChart(IMorseService morseService, MenuService menu)
    {
        Console.Clear();
        var chart = morseService.GetMorseChart();
        menu.ShowMorseChart(chart);
        menu.Pause();
    }

    private static void ShowHistory(IMorseService morseService, MenuService menu)
    {
        Console.Clear();
        var history = morseService.GetHistory();
        menu.ShowHistory(history);
        menu.Pause();
    }
}
