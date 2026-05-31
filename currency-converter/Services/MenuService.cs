using System.Globalization;
using currency_converter.Models;

namespace currency_converter.Services;

public class MenuService
{
    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║       CURRENCY CONVERTER             ║");
        Console.WriteLine(@"║    Conversor de divisas (Mock API)   ║");
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
        Console.WriteLine(" [1] Convertir moneda");
        Console.WriteLine(" [2] Ver tasas actuales");
        Console.WriteLine(" [3] Cambiar moneda base");
        Console.WriteLine(" [4] Actualizar tasas");
        Console.WriteLine(" [5] Historial");
        Console.WriteLine(" [6] Exportar tasas");
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
                "c" => "1",
                "t" => "2",
                "b" => "3",
                "a" => "4",
                "h" => "5",
                "e" => "6",
                "s" => "7",
                _ => ""
            }
        };
    }

    public decimal GetAmount()
    {
        while (true)
        {
            Console.Write(" Ingrese el monto: ");
            string? input = Console.ReadLine()?.Trim().Replace(',', '.');
            if (decimal.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal value) && value > 0)
                return value;
            ShowError("Debe ingresar un monto válido mayor a cero.");
        }
    }

    public CurrencyCode GetCurrency(string prompt)
    {
        Console.WriteLine($" {prompt}");
        Console.WriteLine(" ────────────────────────────");
        var codes = Enum.GetValues<CurrencyCode>();
        foreach (var code in codes)
        {
            Console.WriteLine($" [{(int)code}] {CurrencySymbol(code)} ({code}) - {CurrencyName(code)}");
        }
        Console.WriteLine();

        while (true)
        {
            Console.Write(" Código (1-12): ");
            string? input = Console.ReadLine()?.Trim();
            if (int.TryParse(input, out int idx) && idx >= 1 && idx <= 12)
                return (CurrencyCode)idx;
            ShowError("Debe ingresar un número entre 1 y 12.");
        }
    }

    public void ShowConversionResult(ConversionRecord record)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine();
        Console.WriteLine($" {record.Amount:N2} {CurrencySymbol(record.From)} ({record.From})");
        Console.WriteLine($"   =  {record.Result:N2} {CurrencySymbol(record.To)} ({record.To})");
        Console.ResetColor();
        Console.WriteLine($" Tasa: 1 {record.From} = {record.Rate:F6} {record.To}");
        Console.WriteLine();
    }

    public void ShowRateTable(ExchangeRate rates)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n Tasas base: {CurrencySymbol(rates.BaseCurrency)} ({rates.BaseCurrency})");
        Console.WriteLine($" Actualizado: {rates.LastUpdated:dd/MM/yyyy HH:mm:ss}");
        Console.ResetColor();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($" {"#",3} {"Código",-6} {"Moneda",-24} {"Tasa",14}");
        Console.ResetColor();
        Console.WriteLine($" {"─",3} {"──────",-6} {"────────────────────────",-24} {"──────────────",14}");

        int i = 1;
        foreach (var kvp in rates.Rates.OrderBy(r => (int)r.Key))
        {
            string symbol = CurrencySymbol(kvp.Key);
            string name = CurrencyName(kvp.Key);
            Console.WriteLine($" {i,3} {kvp.Key,-6} {symbol,-3} {name,-21} {kvp.Value,14:F6}");
            i++;
        }
        Console.WriteLine();
    }

    public void ShowRefreshMessage(DateTime timestamp)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" Tasas actualizadas: {timestamp:dd/MM/yyyy HH:mm:ss}");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowHistory(List<ConversionRecord> records)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         HISTORIAL                    ║");
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
            Console.WriteLine($"  [{i + 1}] {r.Amount,10:N2} {CurrencySymbol(r.From)} → {r.Result,10:N2} {CurrencySymbol(r.To)}  ({r.Timestamp:dd/MM HH:mm})");
        }
        Console.WriteLine();
    }

    public void ShowExportSuccess(string fileName)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" Exportado: {fileName}");
        Console.ResetColor();
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

    public static string CurrencySymbol(CurrencyCode code)
    {
        return code switch
        {
            CurrencyCode.USD => "$",
            CurrencyCode.EUR => "€",
            CurrencyCode.GBP => "£",
            CurrencyCode.JPY => "¥",
            CurrencyCode.MXN => "$",
            CurrencyCode.BRL => "R$",
            CurrencyCode.ARS => "$",
            CurrencyCode.CAD => "$",
            CurrencyCode.AUD => "$",
            CurrencyCode.CHF => "Fr",
            CurrencyCode.CNY => "¥",
            CurrencyCode.INR => "₹",
            _ => "?"
        };
    }

    public static string CurrencyName(CurrencyCode code)
    {
        return code switch
        {
            CurrencyCode.USD => "Dólar estadounidense",
            CurrencyCode.EUR => "Euro",
            CurrencyCode.GBP => "Libra esterlina",
            CurrencyCode.JPY => "Yen japonés",
            CurrencyCode.MXN => "Peso mexicano",
            CurrencyCode.BRL => "Real brasileño",
            CurrencyCode.ARS => "Peso argentino",
            CurrencyCode.CAD => "Dólar canadiense",
            CurrencyCode.AUD => "Dólar australiano",
            CurrencyCode.CHF => "Franco suizo",
            CurrencyCode.CNY => "Yuan chino",
            CurrencyCode.INR => "Rupia india",
            _ => "?"
        };
    }
}
