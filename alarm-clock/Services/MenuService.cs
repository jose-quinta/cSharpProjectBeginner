using alarm_clock.Models;

namespace alarm_clock.Services;

public class MenuService
{
    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║          ALARM CLOCK                 ║");
        Console.WriteLine(@"╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowMainMenu()
    {
        Console.WriteLine($" Hora actual: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" MENÚ PRINCIPAL");
        Console.ResetColor();
        Console.WriteLine(" ────────────────────────────");
        Console.WriteLine(" [1] Ver alarmas");
        Console.WriteLine(" [2] Agregar alarma");
        Console.WriteLine(" [3] Activar/Desactivar alarma");
        Console.WriteLine(" [4] Eliminar alarma");
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
                "v" => "1",
                "a" => "2",
                "c" => "3",
                "e" => "4",
                "s" => "5",
                _ => ""
            }
        };
    }

    public void ShowAlarmList(List<Alarm> alarms)
    {
        if (alarms.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" (No hay alarmas configuradas)");
            Console.ResetColor();
            Console.WriteLine();
            return;
        }

        int active = alarms.Count(a => a.IsEnabled);

        Console.WriteLine($" Total: {alarms.Count}  |  Activas: {active}");
        Console.WriteLine();

        for (int i = 0; i < alarms.Count; i++)
        {
            var a = alarms[i];
            string status = a.IsEnabled ? "ON " : "OFF";
            string recurring = a.IsRecurring
                ? $" [{string.Join(",", a.RecurringDays.Select(d => d.ToString()[..3]))}]"
                : "";

            Console.ForegroundColor = a.IsEnabled ? ConsoleColor.Green : ConsoleColor.DarkGray;
            Console.WriteLine($" [{status}] {a.Title}{recurring}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"      └ ID: {a.Id}  |  Hora: {a.AlarmTime:dd/MM/yy HH:mm}");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    public string GetTitle()
    {
        Console.Write(" Nombre de la alarma: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public DateTime GetAlarmTime()
    {
        while (true)
        {
            Console.Write(" Fecha y hora (dd/MM/yyyy HH:mm): ");
            string? input = Console.ReadLine()?.Trim();
            if (DateTime.TryParseExact(input, "dd/MM/yyyy HH:mm", null,
                System.Globalization.DateTimeStyles.None, out DateTime dt))
            {
                if (dt > DateTime.Now)
                    return dt;

                ShowError("La fecha debe ser futura.");
            }
            else
            {
                ShowError("Formato inválido. Use dd/MM/yyyy HH:mm (ej: 02/06/2026 08:00).");
            }
        }
    }

    public bool GetIsRecurring()
    {
        Console.Write(" ¿Repetir semanalmente? (s/n): ");
        return Console.ReadLine()?.Trim().ToLower() == "s";
    }

    public DayOfWeek[] GetRecurringDays()
    {
        Console.WriteLine(" Días de repetición (separados por coma, ej: 1,2,3,4,5 para lun-vie):");
        Console.WriteLine(" 0=Dom  1=Lun  2=Mar  3=Mie  4=Jue  5=Vie  6=Sab");
        Console.Write(" Días: ");
        string? input = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(input))
            return new[] { DateTime.Now.DayOfWeek };

        return input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => int.TryParse(s, out int d) && d >= 0 && d <= 6 ? (DayOfWeek)d : (DayOfWeek?)null)
            .Where(d => d.HasValue)
            .Select(d => d!.Value)
            .Distinct()
            .ToArray();
    }

    public Guid GetId(string prompt)
    {
        while (true)
        {
            Console.Write($" {prompt}: ");
            string? input = Console.ReadLine()?.Trim();
            if (Guid.TryParse(input, out Guid id))
                return id;
            ShowError("ID inválido.");
        }
    }

    public void ShowSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {message}");
        Console.ResetColor();
    }

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
