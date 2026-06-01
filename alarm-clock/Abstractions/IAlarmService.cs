using alarm_clock.Models;

namespace alarm_clock.Abstractions;

public interface IAlarmService
{
    void Add(string title, DateTime alarmTime, bool isRecurring, DayOfWeek[] recurringDays);
    List<Alarm> GetAll();
    Alarm? GetById(Guid id);
    bool Toggle(Guid id);
    bool Delete(Guid id);
    List<Alarm> GetDueAlarms();
    void StartMonitoring();
    void StopMonitoring();
    event Action<Alarm>? AlarmTriggered;
}
