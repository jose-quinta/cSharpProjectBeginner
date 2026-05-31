namespace currency_converter.Models;

public class ExchangeRate
{
    public CurrencyCode BaseCurrency { get; set; }
    public Dictionary<CurrencyCode, double> Rates { get; set; } = new Dictionary<CurrencyCode, double>();
    public DateTime LastUpdated { get; set; }
}
