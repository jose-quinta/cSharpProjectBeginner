using System.Timers;
using alarm_clock.Abstractions;
using alarm_clock.Models;
using Timer = System.Timers.Timer;

namespace alarm_clock.Services;

public class AlarmService : IAlarmService
{
    private readonly List<Alarm> _alarms = new List<Alarm>();
    private Timer? _timer;
    private readonly object _lock = new object();

    public event Action<Alarm>? AlarmTriggered;

    public void Add(string title, DateTime alarmTime, bool isRecurring, DayOfWeek[] recurringDays)
    {
        var alarm = new Alarm
        {
            Title = title,
            AlarmTime = alarmTime,
            IsRecurring = isRecurring,
            RecurringDays = recurringDays
        };

        lock (_lock)
        {
            _alarms.Add(alarm);
        }
    }

    public List<Alarm> GetAll()
    {
        lock (_lock)
        {
            return _alarms.OrderBy(a => a.AlarmTime).ToList();
        }
    }

    public Alarm? GetById(Guid id)
    {
        lock (_lock)
        {
            return _alarms.FirstOrDefault(a => a.Id == id);
        }
    }

    public bool Toggle(Guid id)
    {
        lock (_lock)
        {
            Alarm? alarm = _alarms.FirstOrDefault(a => a.Id == id);
            if (alarm == null) return false;
            alarm.IsEnabled = !alarm.IsEnabled;
            alarm.HasFired = false;
            return true;
        }
    }

    public bool Delete(Guid id)
    {
        lock (_lock)
        {
            Alarm? alarm = _alarms.FirstOrDefault(a => a.Id == id);
            if (alarm == null) return false;
            _alarms.Remove(alarm);
            return true;
        }
    }

    public List<Alarm> GetDueAlarms()
    {
        var now = DateTime.Now;
        var due = new List<Alarm>();

        lock (_lock)
        {
            foreach (var alarm in _alarms)
            {
                if (!alarm.IsEnabled || alarm.HasFired)
                    continue;

                bool shouldFire = false;

                if (alarm.IsRecurring)
                {
                    if (alarm.RecurringDays.Contains(now.DayOfWeek) &&
                        alarm.AlarmTime.Hour == now.Hour &&
                        alarm.AlarmTime.Minute == now.Minute &&
                        now.Second == 0)
                    {
                        shouldFire = true;
                    }
                }
                else
                {
                    if (now >= alarm.AlarmTime && now < alarm.AlarmTime.AddMinutes(1))
                    {
                        shouldFire = true;
                    }
                }

                if (shouldFire)
                {
                    alarm.HasFired = true;
                    due.Add(alarm);
                }
            }
        }

        return due;
    }

    public void StartMonitoring()
    {
        _timer = new Timer(1000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Start();
    }

    public void StopMonitoring()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        var due = GetDueAlarms();
        foreach (var alarm in due)
        {
            AlarmTriggered?.Invoke(alarm);
        }
    }
}
