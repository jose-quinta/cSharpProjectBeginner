using file_encryption_decryption.Models;

namespace file_encryption_decryption.Services;

public class MenuService
{
    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║     FILE ENCRYPTION/DECRYPTION       ║");
        Console.WriteLine(@"║    Cifrado AES-256 para archivos     ║");
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
        Console.WriteLine(" [1] Cifrar archivo (.txt)");
        Console.WriteLine(" [2] Descifrar archivo (.enc)");
        Console.WriteLine(" [3] Historial");
        Console.WriteLine(" [4] Limpiar historial");
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
                "c" => "1",
                "d" => "2",
                "h" => "3",
                "l" => "4",
                "s" => "5",
                _ => ""
            }
        };
    }

    public void ShowResult(bool success, string message, string fileName, long fileSize)
    {
        Console.WriteLine();
        if (success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($" ✓ {message}");
            Console.ResetColor();
            Console.WriteLine($"   Archivo: {fileName}");
            Console.WriteLine($"   Tamaño:  {FormatSize(fileSize)}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($" ✗ {message}");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    public void ShowHistory(List<FileOperation> records)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         HISTORIAL                    ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        if (records.Count == 0)
        {
            Console.WriteLine(" (No hay operaciones previas)");
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < records.Count; i++)
        {
            var r = records[i];
            string icon = r.Success ? "✓" : "✗";
            string op = r.Operation == OperationType.Encrypt ? "CIFRAR" : "DESCIFRAR";
            string file = r.FileName.Length > 30 ? r.FileName[..27] + "..." : r.FileName;
            Console.WriteLine($"  [{i + 1}] {icon} {op,-8} {file,-30} {FormatSize(r.FileSize),8}  {r.Timestamp:dd/MM HH:mm}");
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

    private static string FormatSize(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        return $"{bytes / (1024.0 * 1024.0):F1} MB";
    }
}
