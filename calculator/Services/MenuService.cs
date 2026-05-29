namespace calculator.Services;

public class MenuService
{
    private static readonly ConsoleColor ColorMenu = ConsoleColor.Cyan;
    private static readonly ConsoleColor ColorResultado = ConsoleColor.Green;
    private static readonly ConsoleColor ColorError = ConsoleColor.Red;
    private static readonly ConsoleColor ColorHistorial = ConsoleColor.Yellow;

    public void ShowMenu()
    {
        Console.ForegroundColor = ColorMenu;
        Console.WriteLine("\n=== CALCULADORA ===");
        Console.WriteLine(" 1.  Sumar");
        Console.WriteLine(" 2.  Restar");
        Console.WriteLine(" 3.  Multiplicar");
        Console.WriteLine(" 4.  Dividir");
        Console.WriteLine(" 5.  Potencia");
        Console.WriteLine(" 6.  M\u00F3dulo");
        Console.WriteLine(" 7.  \u221A (Ra\u00EDz cuadrada)");
        Console.WriteLine(" 8.  MC (Limpiar memoria)");
        Console.WriteLine(" 9.  MR (Recuperar memoria)");
        Console.WriteLine("10.  MS (Guardar en memoria)");
        Console.WriteLine("11.  M+ (Sumar a memoria)");
        Console.WriteLine("12.  M- (Restar de memoria)");
        Console.WriteLine("13.  Historial");
        Console.WriteLine("14.  Ingresar expresi\u00F3n (ej: 5 + 3)");
        Console.WriteLine("15.  Sen(x)");
        Console.WriteLine("16.  Cos(x)");
        Console.WriteLine("17.  Tan(x)");
        Console.WriteLine("18.  Log10(x)");
        Console.WriteLine("19.  Abs(x)");
        Console.WriteLine("20.  n! (Factorial)");
        Console.WriteLine("21.  Salir");
        Console.Write("Elija opci\u00F3n (n\u00FAmero o letra): ");
        Console.ResetColor();
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
            "D9" or "NumPad9" => "9",
            "D0" or "NumPad0" => "10",
            "OemPlus" or "Add" => "9",
            "OemMinus" or "Subtract" => "10",
            "Escape" => "21",
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "s" => "1",
                "r" => "2",
                "m" => "3",
                "d" => "4",
                "p" => "5",
                "u" => "6",
                "c" => "7",
                "l" => "8",
                "v" => "9",
                "g" => "10",
                "h" => "13",
                "e" or "=" => "14",
                "n" => "15",
                "o" => "16", // cos
                "t" => "17",
                // "g" => "18", // log - conflicto con "g" de guardar memoria
                "a" => "19",
                "f" => "20",
                "x" => "21",
                _ => ""
            }
        };
    }

    public bool ConfirmUseLastResult(double r)
    {
        Console.Write($"\u00BFUsar resultado anterior ({r}) como primer n\u00FAmero? [S/N]: ");
        var key = Console.ReadKey(intercept: true);
        Console.WriteLine(key.KeyChar);
        return key.KeyChar.ToString().ToLower() == "s";
    }

    public (bool success, double a) GetSingleNumber()
    {
        Console.Write("Ingrese el n\u00FAmero: ");
        if (!double.TryParse(Console.ReadLine(), out double number))
        {
            Console.WriteLine("N\u00FAmero no v\u00E1lido.");
            return (false, 0);
        }

        return (true, number);
    }

    public (bool success, double a, double b) GetNumbers(double? r = null)
    {
        double a;
        if (r.HasValue && ConfirmUseLastResult(r.Value))
        {
            a = r.Value;
            Console.WriteLine($"Primer n\u00FAmero: {a}");
        }
        else
        {
            Console.Write("Ingrese el primer n\u00FAmero: ");
            if (!double.TryParse(Console.ReadLine(), out a))
            {
                Console.WriteLine("N\u00FAmero no v\u00E1lido.");
                return (false, 0, 0);
            }
        }

        Console.Write("Ingrese el segundo n\u00FAmero: ");
        if (!double.TryParse(Console.ReadLine(), out double b))
        {
            Console.WriteLine("N\u00FAmero no v\u00E1lido.");
            return (false, 0, 0);
        }

        return (true, a, b);
    }

    public static bool IsConfirmKey(char key)
        => key.ToString().ToLower() == "s";

    public static (bool success, double a, string op, double b) ParseExpressionString(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 3)
            return (false, 0, "", 0);

        if (!double.TryParse(parts[0], out double a) || !double.TryParse(parts[2], out double b))
            return (false, 0, "", 0);

        return (true, a, parts[1], b);
    }

    public (bool success, double a, string op, double b) ParseExpression()
    {
        Console.Write("Ingrese expresi\u00F3n (ej: 5 + 3): ");
        string input = (Console.ReadLine() ?? "").Trim();
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var result = ParseExpressionString(input);

        if (!result.success)
        {
            if (parts.Length != 3)
                ShowError("Formato inv\u00E1lido. Use n\u00FAmero operador n\u00FAmero");
            else
                ShowError("N\u00FAmero no v\u00E1lido.");
        }

        return result;
    }

    public void ShowResultBanner(double r)
    {
        Console.ForegroundColor = ColorResultado;
        Console.WriteLine($"[ Resultado actual: {r} ]");
        Console.ResetColor();
    }

    public void ShowResult(double a, string op, double r, string? msg = null)
    {
        Console.ForegroundColor = ColorResultado;
        if (double.IsNaN(r))
            ShowError(msg ?? "La operaci\u00F3n no pudo completarse.");
        else
            Console.WriteLine($"{op}{a} = {r}");
        Console.ResetColor();
    }

    public void ShowResult(double a, string op, double b, double r, string? msg = null)
    {
        Console.ForegroundColor = ColorResultado;
        if (double.IsNaN(r))
            ShowError(msg ?? "La operaci\u00F3n no pudo completarse.");
        else
            Console.WriteLine($"Resultado: {a} {op} {b} = {r}");
        Console.ResetColor();
    }

    public void ShowHistory(List<string> history)
    {
        Console.ForegroundColor = ColorHistorial;
        Console.WriteLine("\n--- HISTORIAL DE OPERACIONES ---");
        if (history.Count == 0)
        {
            Console.WriteLine("(vac\u00EDo)");
        }
        else
        {
            for (int i = 0; i < history.Count; i++)
                Console.WriteLine($"  {i + 1}. {history[i]}");
        }
        Console.WriteLine("---------------------------------");
        Console.ResetColor();
    }

    public void ShowMemoryStatus(double? memory)
    {
        Console.ForegroundColor = ColorHistorial;
        if (!memory.HasValue)
            Console.WriteLine("No hay valor en memoria.");
        else
            Console.WriteLine($"Valor en memoria: {memory}");
        Console.ResetColor();
    }

    public void ShowMemoryStatus(string msg)
    {
        Console.ForegroundColor = ColorHistorial;
        Console.WriteLine(msg);
        Console.ResetColor();
    }

    public void ShowError(string msg)
    {
        Console.ForegroundColor = ColorError;
        Console.WriteLine($"Error: {msg}");
        Console.ResetColor();
    }
}
