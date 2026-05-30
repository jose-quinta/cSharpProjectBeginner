using password_generator.Models;

namespace password_generator.Services;

public class MenuService
{
    public int ConfigLength { get; set; } = 16;
    public bool ConfigUpper { get; set; } = true;
    public bool ConfigLower { get; set; } = true;
    public bool ConfigDigits { get; set; } = true;
    public bool ConfigSymbols { get; set; } = true;
    public bool ConfigExcludeSimilar { get; set; } = true;

    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║       PASSWORD GENERATOR             ║");
        Console.WriteLine(@"║    Generador de contraseñas seguras  ║");
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
        Console.WriteLine(" [1] Generar contraseña");
        Console.WriteLine(" [2] Generar múltiples");
        Console.WriteLine(" [3] Configuración");
        Console.WriteLine(" [4] Historial");
        Console.WriteLine(" [5] Exportar");
        Console.WriteLine(" [6] Limpiar historial");
        Console.WriteLine(" [7] Salir");
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
            "D6" or "NumPad6" => "6",
            "D7" or "NumPad7" => "7",
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "g" => "1",
                "m" => "2",
                "c" => "3",
                "h" => "4",
                "e" => "5",
                "l" => "6",
                "s" => "7",
                _ => ""
            }
        };
    }

    public int GetLength()
    {
        while (true)
        {
            Console.Write($" Longitud actual: {ConfigLength}");
            Console.Write("\n Nueva longitud (4-128): ");
            string? input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out int value) && value >= 4 && value <= 128)
                return value;
            ShowError("Debe ingresar un número entre 4 y 128.");
        }
    }

    public void ShowConfig()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         CONFIGURACIÓN                 ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($" Longitud:         {ConfigLength}");
        Console.WriteLine($" Mayúsculas:       {(ConfigUpper ? "✓" : "✗")}");
        Console.WriteLine($" Minúsculas:       {(ConfigLower ? "✓" : "✗")}");
        Console.WriteLine($" Dígitos:          {(ConfigDigits ? "✓" : "✗")}");
        Console.WriteLine($" Símbolos:         {(ConfigSymbols ? "✓" : "✗")}");
        Console.WriteLine($" Excluir similares: {(ConfigExcludeSimilar ? "✓" : "✗")}");
        Console.WriteLine();
    }

    public void ShowConfigMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" CONFIGURACIÓN");
        Console.ResetColor();
        Console.WriteLine(" ────────────────────────────");
        Console.WriteLine(" [1] Cambiar longitud");
        Console.WriteLine(" [2] Alternar mayúsculas");
        Console.WriteLine(" [3] Alternar minúsculas");
        Console.WriteLine(" [4] Alternar dígitos");
        Console.WriteLine(" [5] Alternar símbolos");
        Console.WriteLine(" [6] Alternar excluir similares");
        Console.WriteLine(" [7] Volver");
        Console.WriteLine();
        Console.Write(" Opción: ");
    }

    public string GetConfigChoice()
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
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "l" => "1",
                "m" => "2",
                "n" => "3",
                "d" => "4",
                "s" => "5",
                "e" => "6",
                "v" => "7",
                _ => ""
            }
        };
    }

    public int GetMultipleCount()
    {
        while (true)
        {
            Console.Write(" ¿Cuántas contraseñas generar? (1-50): ");
            string? input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out int value) && value >= 1 && value <= 50)
                return value;
            ShowError("Debe ingresar un número entre 1 y 50.");
        }
    }

    public void ShowPassword(PasswordEntry entry)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n ─── CONTRASEÑA GENERADA ───");
        Console.ResetColor();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"  {entry.Password}");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"  Longitud: {entry.Length}");
        Console.Write("  Tipos:    ");
        if (entry.HasUpper) Console.Write("A-Z ");
        if (entry.HasLower) Console.Write("a-z ");
        if (entry.HasDigit) Console.Write("0-9 ");
        if (entry.HasSymbol) Console.Write("!@#$%");
        Console.WriteLine();
        Console.Write("  Fortaleza: ");
        PrintStrength(entry.Strength);
        Console.WriteLine($"  Entropía:  {entry.Entropy} bits");
        Console.WriteLine();
    }

    public void ShowPasswords(List<PasswordEntry> entries)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n ─── {entries.Count} CONTRASEÑAS GENERADAS ───");
        Console.ResetColor();
        Console.WriteLine();

        for (int i = 0; i < entries.Count; i++)
        {
            var e = entries[i];
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  [{i + 1}] ");
            Console.ResetColor();
            Console.Write(e.Password);
            Console.Write("  ");
            PrintStrength(e.Strength);
        }
        Console.WriteLine();
    }

    public void ShowHistory(List<PasswordEntry> records)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         HISTORIAL                    ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        if (records.Count == 0)
        {
            Console.WriteLine(" (No hay contraseñas guardadas)");
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < records.Count; i++)
        {
            var r = records[i];
            string pwd = r.Password.Length > 30 ? r.Password[..27] + "..." : r.Password;
            Console.Write($"  [{i + 1}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{pwd,-30} ");
            Console.ResetColor();
            PrintStrength(r.Strength);
            Console.WriteLine($"       {r.Timestamp:dd/MM HH:mm}");
        }
        Console.WriteLine();
    }

    public void ShowClearHistory()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n Historial limpiado.");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowExportSuccess(string fileName)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" Exportado: {fileName}");
        Console.ResetColor();
    }

    public void ShowConfigUpdated(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {message}");
        Console.ResetColor();
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

    private void PrintStrength(string label)
    {
        Console.ForegroundColor = label switch
        {
            "Débil" => ConsoleColor.Red,
            "Media" => ConsoleColor.Yellow,
            "Fuerte" => ConsoleColor.Green,
            "Muy fuerte" => ConsoleColor.Magenta,
            _ => ConsoleColor.Gray
        };
        Console.Write($"[{label}]");
        Console.ResetColor();
    }
}
