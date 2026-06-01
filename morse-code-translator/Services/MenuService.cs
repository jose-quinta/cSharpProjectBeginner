using morse_code_translator.Models;

namespace morse_code_translator.Services;

public class MenuService
{
    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║       MORSE CODE TRANSLATOR          ║");
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
        Console.WriteLine(" [1] Texto → Morse");
        Console.WriteLine(" [2] Morse → Texto");
        Console.WriteLine(" [3] Ver tabla Morse");
        Console.WriteLine(" [4] Historial de traducciones");
        Console.WriteLine(" [5] Salir");
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
            "D5" or "NumPad5" => "5",
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "m" => "1",
                "t" => "2",
                "v" => "3",
                "h" => "4",
                "s" => "5",
                _ => ""
            }
        };
    }

    public string GetInput(string prompt)
    {
        Console.WriteLine($" {prompt}");
        Console.WriteLine(" (Escriba su texto y presione Enter)");
        Console.WriteLine();
        Console.Write(" > ");
        return Console.ReadLine() ?? string.Empty;
    }

    public void ShowTranslationResult(string input, string output, string direction)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($" [{direction}]");
        Console.ResetColor();
        Console.WriteLine($"  Entrada:  {input}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  Salida:   {output}");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowMorseChart(Dictionary<char, string> chart)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║           TABLA MORSE                ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var letters = chart.Where(kv => char.IsLetter(kv.Key)).ToList();
        var digits = chart.Where(kv => char.IsDigit(kv.Key)).ToList();
        var punct = chart.Where(kv => !char.IsLetterOrDigit(kv.Key) && kv.Key != ' ').ToList();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" Letras:");
        Console.ResetColor();
        for (int i = 0; i < letters.Count; i++)
        {
            var kv = letters[i];
            Console.Write($"  {kv.Key} → {kv.Value,-8}");
            if ((i + 1) % 4 == 0) Console.WriteLine();
        }
        if (letters.Count % 4 != 0) Console.WriteLine();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" Números:");
        Console.ResetColor();
        for (int i = 0; i < digits.Count; i++)
        {
            var kv = digits[i];
            Console.Write($"  {kv.Key} → {kv.Value,-8}");
            if ((i + 1) % 5 == 0) Console.WriteLine();
        }
        if (digits.Count % 5 != 0) Console.WriteLine();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" Puntuación:");
        Console.ResetColor();
        foreach (var kv in punct)
        {
            string display = kv.Key == ' ' ? "' '" : kv.Key.ToString();
            Console.WriteLine($"  {display} → {kv.Value}");
        }

        Console.WriteLine();
        Console.WriteLine("  Espacio entre letras: ' '");
        Console.WriteLine("  Espacio entre palabras: ' / '");
        Console.WriteLine();
    }

    public void ShowHistory(List<TranslationRecord> records)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║        HISTORIAL DE TRADUCCIONES     ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        if (records.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" (No hay traducciones en el historial)");
            Console.ResetColor();
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < records.Count; i++)
        {
            var r = records[i];
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  #{i + 1}  [{r.Direction}]  {r.Timestamp:HH:mm:ss}");
            Console.ResetColor();
            Console.WriteLine($"       In:  {Truncate(r.Input, 50)}");
            Console.WriteLine($"       Out: {Truncate(r.Output, 50)}");
            Console.WriteLine();
        }
    }

    private static string Truncate(string s, int max) =>
        s.Length <= max ? s : s[..(max - 3)] + "...";

    public void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" Error: {message}");
        Console.ResetColor();
    }

    public void Pause()
    {
        Console.Write("\n Presione cualquier tecla para continuar...");
        Console.ReadKey(true);
        Console.WriteLine();
    }
}
