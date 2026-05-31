namespace currency_converter.Models;

public class ConversionRecord
{
    public decimal Amount { get; set; }
    public CurrencyCode From { get; set; }
    public CurrencyCode To { get; set; }
    public decimal Result { get; set; }
    public double Rate { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}
