using temperature_converter.Models;

namespace temperature_converter.Services;

public class MenuService
{
    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║      TEMPERATURE CONVERTER           ║");
        Console.WriteLine(@"║    Convertidor de temperaturas       ║");
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
        Console.WriteLine(" [1] Celsius → Fahrenheit");
        Console.WriteLine(" [2] Fahrenheit → Celsius");
        Console.WriteLine(" [3] Celsius → Kelvin");
        Console.WriteLine(" [4] Comparar todas");
        Console.WriteLine(" [5] Historial");
        Console.WriteLine(" [6] Salir");
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
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "c" => "1",
                "f" => "2",
                "k" => "3",
                "t" => "4",
                "h" => "5",
                "s" => "6",
                _ => ""
            }
        };
    }

    public double GetTemperature()
    {
        while (true)
        {
            Console.Write(" Ingrese la temperatura: ");
            string? input = Console.ReadLine()?.Trim().Replace(',', '.');
            if (double.TryParse(input, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out double value))
            {
                return value;
            }
            ShowError("Debe ingresar un número válido.");
        }
    }

    public void ShowResult(double inputValue, TemperatureUnit from, double outputValue, TemperatureUnit to, string formula)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine($" {inputValue:F2}°{UnitSymbol(from)} = {outputValue:F2}°{UnitSymbol(to)}");
        Console.ResetColor();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($" Fórmula: {formula}");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowAllConversions(double value, TemperatureUnit from, Dictionary<TemperatureUnit, double> results)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n ─── CONVERSIONES ───");
        Console.ResetColor();
        Console.WriteLine();

        string sourceLabel = $"Origen: {value:F2}°{UnitSymbol(from)}";
        Console.WriteLine($" {sourceLabel}");
        Console.WriteLine();

        foreach (var kvp in results)
        {
            if (kvp.Key == from) continue;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" {UnitSymbol(kvp.Key),4}");
            Console.ResetColor();
            Console.WriteLine($"  →  {kvp.Value,10:F2}°");
        }
        Console.WriteLine();
    }

    public void ShowHistory(List<ConversionRecord> records)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         HISTORIAL DE CONVERSIONES    ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        if (records.Count == 0)
        {
            Console.WriteLine(" (No hay conversiones previas)");
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < records.Count; i++)
        {
            var r = records[i];
            Console.WriteLine($" [{i + 1}] {r.InputValue,8:F2}°{UnitSymbol(r.InputUnit)}  →  {r.OutputValue,8:F2}°{UnitSymbol(r.OutputUnit)}  ({r.Timestamp:dd/MM HH:mm})");
        }
        Console.WriteLine();
    }

    public void ShowConversionDetail(ConversionRecord r)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine($" {r.InputValue:F2}°{UnitSymbol(r.InputUnit)} = {r.OutputValue:F2}°{UnitSymbol(r.OutputUnit)}");
        Console.ResetColor();
        Console.WriteLine($" Fecha:  {r.Timestamp:dd/MM/yyyy HH:mm:ss}");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($" Fórmula: {r.Formula}");
        Console.ResetColor();
        Console.WriteLine();
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

    private static string UnitSymbol(TemperatureUnit unit)
    {
        return unit switch
        {
            TemperatureUnit.Celsius => "C",
            TemperatureUnit.Fahrenheit => "F",
            TemperatureUnit.Kelvin => "K",
            _ => "?"
        };
    }
}
