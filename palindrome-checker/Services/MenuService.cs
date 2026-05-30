using palindrome_checker.Models;

namespace palindrome_checker.Services;

public class MenuService
{
    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║       PALINDROME CHECKER             ║");
        Console.WriteLine(@"║   Detector de palíndromos            ║");
        Console.WriteLine(@"╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" MENÚ PRINCIPAL");
        Console.ResetColor();
        Console.WriteLine(" ────────────────────────────");
        Console.WriteLine(" [1] Verificar palabra/frase");
        Console.WriteLine(" [2] Verificar número");
        Console.WriteLine(" [3] Buscar palíndromos en texto");
        Console.WriteLine(" [4] Revertir texto");
        Console.WriteLine(" [5] Historial");
        Console.WriteLine(" [6] Estadísticas");
        Console.WriteLine(" [7] Exportar historial");
        Console.WriteLine(" [8] Salir");
        Console.WriteLine();
        Console.Write(" Seleccione una opción (número o letra): ");
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
            "D6" or "NumPad6" => "6",
            "D7" or "NumPad7" => "7",
            "D8" or "NumPad8" => "8",
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "v" => "1",
                "n" => "2",
                "b" => "3",
                "r" => "4",
                "h" => "5",
                "e" => "6",
                "x" => "7",
                "s" => "8",
                _ => ""
            }
        };
    }

    public string GetText(string prompt)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {prompt}");
        Console.ResetColor();
        Console.Write(" Ingrese: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public long GetNumber()
    {
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" Ingrese el número:");
            Console.ResetColor();
            Console.Write(" Número: ");
            string? input = Console.ReadLine()?.Trim();
            if (long.TryParse(input, out long value))
                return value;
            ShowError("Debe ingresar un número válido.");
        }
    }

    public void ShowResult(AnalysisResult r)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Texto original: ");
        Console.ResetColor();
        Console.WriteLine(r.InputText);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Texto limpio:   ");
        Console.ResetColor();
        Console.WriteLine(string.IsNullOrEmpty(r.CleanedText) ? "(vacío)" : r.CleanedText);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Al revés:       ");
        Console.ResetColor();
        Console.WriteLine(r.ReversedText);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Longitud:       ");
        Console.ResetColor();
        Console.WriteLine($"{r.Length} caracteres");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Categoría:      ");
        Console.ResetColor();
        Console.WriteLine(r.Category);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Resultado:      ");
        if (r.IsPalindrome)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("¡SÍ es un palíndromo!");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("NO es un palíndromo");
        }
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowPalindromesFound(AnalysisResult r)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Texto analizado: ");
        Console.ResetColor();
        Console.WriteLine(r.InputText);

        if (r.PalindromesFound.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" No se encontraron palíndromos en el texto.");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" Se encontraron {r.PalindromesFound.Count} palíndromo(s):");
            Console.ResetColor();
            for (int i = 0; i < r.PalindromesFound.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. \"{r.PalindromesFound[i]}\"");
            }
        }
        Console.WriteLine();
    }

    public void ShowReversed(string original, string reversed)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Original: ");
        Console.ResetColor();
        Console.WriteLine(original);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Invertido: ");
        Console.ResetColor();
        Console.WriteLine(reversed);
        Console.WriteLine();
    }

    public void ShowHistory(List<AnalysisResult> records)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         HISTORIAL DE VERIFICACIONES  ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        if (records.Count == 0)
        {
            Console.WriteLine(" (No hay verificaciones previas)");
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < records.Count; i++)
        {
            var r = records[i];
            string icon = r.IsPalindrome ? "✓" : "✗";
            string text = r.InputText.Length > 35
                ? r.InputText[..35] + "..."
                : r.InputText;
            Console.WriteLine($" [{i + 1}] {icon} {text,-38} {r.Category,-8} {r.Timestamp:dd/MM HH:mm}");
        }
        Console.WriteLine();
    }

    public void ShowDetail(AnalysisResult r)
    {
        ShowResult(r);
        if (r.Category == "Texto" && r.PalindromesFound.Count > 0)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" Palíndromos encontrados:");
            Console.ResetColor();
            foreach (string p in r.PalindromesFound)
                Console.WriteLine($"  • \"{p}\"");
            Console.WriteLine();
        }
    }

    public void ShowStats(PalindromeStats stats)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         ESTADÍSTICAS                 ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        if (stats.TotalChecked == 0)
        {
            Console.WriteLine(" (No hay datos para mostrar)");
            Console.WriteLine();
            return;
        }

        double pct = (double)stats.PalindromeCount / stats.TotalChecked * 100;

        Console.WriteLine($" Total verificaciones:  {stats.TotalChecked}");
        Console.WriteLine($" Palíndromos:           {stats.PalindromeCount} ({pct:F1}%)");
        Console.WriteLine($" Palíndromo más largo:  \"{stats.LongestPalindrome}\"");
        Console.WriteLine($" Última verificación:   {stats.LastChecked:dd/MM/yyyy HH:mm:ss}");
        Console.WriteLine();
    }

    public void ShowExportSuccess(string fileName)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" Historial exportado: {fileName}");
        Console.ResetColor();
    }

    public bool ConfirmExit()
    {
        Console.Write(" ¿Salir? (s/n): ");
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
