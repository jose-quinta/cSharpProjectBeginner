namespace alarm_clock.Models;

public class Alarm
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public DateTime AlarmTime { get; set; }
    public bool IsRecurring { get; set; }
    public DayOfWeek[] RecurringDays { get; set; } = Array.Empty<DayOfWeek>();
    public bool IsEnabled { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool HasFired { get; set; }
}
