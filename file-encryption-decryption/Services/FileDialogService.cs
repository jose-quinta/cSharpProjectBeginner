using file_encryption_decryption.Models;

namespace file_encryption_decryption.Services;

public class FileDialogService
{
    public List<string> ListTextFiles()
    {
        return Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt")
            .Select(Path.GetFileName)
            .ToList()!;
    }

    public List<string> ListEncFiles()
    {
        return Directory.GetFiles(Directory.GetCurrentDirectory(), "*.enc")
            .Select(Path.GetFileName)
            .ToList()!;
    }

    public string? SelectFile(List<string> files, string prompt)
    {
        if (files.Count == 0)
            return null;

        Console.WriteLine($" {prompt}");
        for (int i = 0; i < files.Count; i++)
            Console.WriteLine($" [{i + 1}] {files[i]}");

        Console.WriteLine();
        Console.Write(" Seleccione un archivo (n\u00famero) o escriba la ruta completa: ");
        string input = Console.ReadLine()?.Trim() ?? "";

        if (int.TryParse(input, out int idx) && idx >= 1 && idx <= files.Count)
            return Path.Combine(Directory.GetCurrentDirectory(), files[idx - 1]);

        if (File.Exists(input))
            return Path.GetFullPath(input);

        return null;
    }

    public string GetPassword()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Ingrese la contrase\u00f1a: ");
        Console.ResetColor();

        string password = ReadPassword();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Confirme la contrase\u00f1a: ");
        Console.ResetColor();

        string confirm = ReadPassword();
        Console.WriteLine();

        if (password != confirm)
            throw new InvalidOperationException("Las contrase\u00f1as no coinciden.");

        if (string.IsNullOrEmpty(password))
            throw new InvalidOperationException("La contrase\u00f1a no puede estar vac\u00eda.");

        return password;
    }

    public string GetPasswordForDecrypt()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(" Ingrese la contrase\u00f1a: ");
        Console.ResetColor();

        string password = ReadPassword();
        Console.WriteLine();

        if (string.IsNullOrEmpty(password))
            throw new InvalidOperationException("La contrase\u00f1a no puede estar vac\u00eda.");

        return password;
    }

    public string GetOutputPath(string inputPath, OperationType op)
    {
        string dir = Path.GetDirectoryName(inputPath) ?? Directory.GetCurrentDirectory();
        string name = Path.GetFileNameWithoutExtension(inputPath);

        return op switch
        {
            OperationType.Encrypt => Path.Combine(dir, name + ".enc"),
            OperationType.Decrypt => Path.Combine(dir, name.Replace(".enc", "") + ".decrypted.txt"),
            _ => throw new ArgumentException("Operaci\u00f3n inv\u00e1lida.")
        };
    }

    private static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[..^1];
                Console.Write("\b \b");
            }
            else if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        return password;
    }
}
