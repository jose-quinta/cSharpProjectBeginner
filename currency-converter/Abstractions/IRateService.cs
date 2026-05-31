using currency_converter.Models;

namespace currency_converter.Abstractions;

public interface IRateService
{
    ExchangeRate GetRates(CurrencyCode baseCurrency);
    decimal Convert(decimal amount, CurrencyCode from, CurrencyCode to);
    void RefreshRates();
    ExchangeRate? CurrentRates { get; }
}
