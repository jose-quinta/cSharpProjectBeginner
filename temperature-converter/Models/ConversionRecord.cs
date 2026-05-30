namespace temperature_converter.Models;

public class ConversionRecord
{
    public double InputValue { get; set; }
    public TemperatureUnit InputUnit { get; set; }
    public double OutputValue { get; set; }
    public TemperatureUnit OutputUnit { get; set; }
    public string Formula { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}