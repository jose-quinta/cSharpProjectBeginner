using alarm_clock.Abstractions;
using alarm_clock.Models;
using alarm_clock.Services;

namespace alarm_clock;

public class Program
{
    private static volatile Alarm? _pendingAlarm;
    private static readonly object _lock = new object();

    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        IAlarmService alarmService = new AlarmService();
        MenuService menu = new MenuService();

        alarmService.AlarmTriggered += OnAlarmTriggered;
        alarmService.StartMonitoring();

        while (true)
        {
            HandlePendingNotification(alarmService, menu);

            Console.Clear();
            menu.ShowBanner();
            menu.ShowMainMenu();

            string choice = menu.GetChoice();
            Console.WriteLine(choice);

            switch (choice)
            {
                case "1":
                    ShowAlarms(alarmService, menu);
                    break;
                case "2":
                    AddAlarm(alarmService, menu);
                    break;
                case "3":
                    ToggleAlarm(alarmService, menu);
                    break;
                case "4":
                    DeleteAlarm(alarmService, menu);
                    break;
                case "5":
                    alarmService.StopMonitoring();
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

    private static void OnAlarmTriggered(Alarm alarm)
    {
        lock (_lock)
        {
            _pendingAlarm = alarm;
        }
    }

    private static void HandlePendingNotification(IAlarmService alarmService, MenuService menu)
    {
        Alarm? alarm;
        lock (_lock)
        {
            alarm = _pendingAlarm;
            if (alarm == null) return;
            _pendingAlarm = null;
        }

        bool dismissed = false;
        bool snoozed = false;

        while (!dismissed && !snoozed)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(@"  ╔══════════════════════════════════════╗");
            Console.WriteLine(@"  ║          ⏰  ALARMA  ⏰             ║");
            Console.WriteLine(@"  ╚══════════════════════════════════════╝");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  {alarm.Title}");
            Console.ResetColor();
            Console.WriteLine($"  {alarm.AlarmTime:HH:mm}");
            Console.WriteLine();
            Console.WriteLine("  [Enter] Detener  |  [S] Snooze (5 min)");
            Console.Beep(800, 200);

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    dismissed = true;
                }
                else if (key.Key == ConsoleKey.S)
                {
                    snoozed = true;
                }
            }
            else
            {
                Thread.Sleep(500);
            }
        }

        if (dismissed)
        {
            if (alarm.IsRecurring)
            {
                alarm.HasFired = false;
                var nextDay = alarm.RecurringDays
                    .Select(d => NextWeekday(d, alarm.AlarmTime))
                    .Where(d => d > DateTime.Now)
                    .OrderBy(d => d)
                    .FirstOrDefault();

                if (nextDay != default)
                    alarm.AlarmTime = nextDay;
            }
            menu.ShowSuccess($"Alarma '{alarm.Title}' detenida.");
        }
        else if (snoozed)
        {
            alarm.HasFired = false;
            alarm.AlarmTime = DateTime.Now.AddMinutes(5);
            menu.ShowSuccess($"Alarma '{alarm.Title}' pospuesta 5 min.");
        }

        menu.Pause();
    }

    private static DateTime NextWeekday(DayOfWeek day, DateTime reference)
    {
        int diff = ((int)day - (int)reference.DayOfWeek + 7) % 7;
        return reference.Date.AddDays(diff == 0 ? 7 : diff).Add(reference.TimeOfDay);
    }

    private static void ShowAlarms(IAlarmService alarmService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║           LISTA DE ALARMAS           ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var alarms = alarmService.GetAll();
        menu.ShowAlarmList(alarms);
        menu.Pause();
    }

    private static void AddAlarm(IAlarmService alarmService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║          AGREGAR ALARMA              ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        string title = menu.GetTitle();
        if (string.IsNullOrEmpty(title))
        {
            menu.ShowError("El nombre no puede estar vacío.");
            menu.Pause();
            return;
        }

        DateTime alarmTime = menu.GetAlarmTime();
        bool isRecurring = menu.GetIsRecurring();
        DayOfWeek[] recurringDays = isRecurring ? menu.GetRecurringDays() : Array.Empty<DayOfWeek>();

        alarmService.Add(title, alarmTime, isRecurring, recurringDays);
        menu.ShowSuccess($"Alarma '{title}' configurada para {alarmTime:dd/MM/yy HH:mm}.");

        menu.Pause();
    }

    private static void ToggleAlarm(IAlarmService alarmService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║     ACTIVAR / DESACTIVAR ALARMA      ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var alarms = alarmService.GetAll();
        menu.ShowAlarmList(alarms);

        Guid id = menu.GetId("ID de la alarma");
        if (alarmService.Toggle(id))
        {
            var toggled = alarmService.GetById(id);
            string state = toggled?.IsEnabled == true ? "activada" : "desactivada";
            menu.ShowSuccess($"Alarma {state}.");
        }
        else
        {
            menu.ShowError("Alarma no encontrada.");
        }

        menu.Pause();
    }

    private static void DeleteAlarm(IAlarmService alarmService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║          ELIMINAR ALARMA             ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var alarms = alarmService.GetAll();
        menu.ShowAlarmList(alarms);

        Guid id = menu.GetId("ID de la alarma a eliminar");
        if (alarmService.Delete(id))
            menu.ShowSuccess("Alarma eliminada.");
        else
            menu.ShowError("Alarma no encontrada.");

        menu.Pause();
    }
}
