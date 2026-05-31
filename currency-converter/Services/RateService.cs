using currency_converter.Abstractions;
using currency_converter.Models;

namespace currency_converter.Services;

public class RateService : IRateService
{
    private static readonly Random Rng = new Random();

    private static readonly Dictionary<CurrencyCode, double> BaseRates = new Dictionary<CurrencyCode, double>()
    {
        [CurrencyCode.USD] = 1.0,
        [CurrencyCode.EUR] = 0.92,
        [CurrencyCode.GBP] = 0.79,
        [CurrencyCode.JPY] = 149.50,
        [CurrencyCode.MXN] = 17.20,
        [CurrencyCode.BRL] = 4.95,
        [CurrencyCode.ARS] = 870.00,
        [CurrencyCode.CAD] = 1.36,
        [CurrencyCode.AUD] = 1.53,
        [CurrencyCode.CHF] = 0.88,
        [CurrencyCode.CNY] = 7.24,
        [CurrencyCode.INR] = 83.10,
    };

    private Dictionary<CurrencyCode, double> _currentRates = new Dictionary<CurrencyCode, double>();
    private DateTime _lastUpdated;

    public RateService()
    {
        RefreshRates();
    }

    public ExchangeRate? CurrentRates { get; private set; }

    public void RefreshRates()
    {
        _currentRates = new Dictionary<CurrencyCode, double>();

        foreach (var kvp in BaseRates)
        {
            double variation = 1.0 + (Rng.NextDouble() * 0.04 - 0.02);
            _currentRates[kvp.Key] = Math.Round(kvp.Value * variation, 6);
        }

        _currentRates[CurrencyCode.USD] = 1.0;
        _lastUpdated = DateTime.Now;

        CurrentRates = new ExchangeRate
        {
            BaseCurrency = CurrencyCode.USD,
            Rates = new Dictionary<CurrencyCode, double>(_currentRates),
            LastUpdated = _lastUpdated
        };
    }

    public ExchangeRate GetRates(CurrencyCode baseCurrency)
    {
        if (_currentRates.Count == 0)
            RefreshRates();

        if (baseCurrency == CurrencyCode.USD)
        {
            return new ExchangeRate
            {
                BaseCurrency = CurrencyCode.USD,
                Rates = new Dictionary<CurrencyCode, double>(_currentRates),
                LastUpdated = _lastUpdated
            };
        }

        double baseToUsd = _currentRates[baseCurrency];
        var converted = new Dictionary<CurrencyCode, double>();

        foreach (var kvp in _currentRates)
        {
            converted[kvp.Key] = Math.Round(kvp.Value / baseToUsd, 6);
        }

        return new ExchangeRate
        {
            BaseCurrency = baseCurrency,
            Rates = converted,
            LastUpdated = _lastUpdated
        };
    }

    public decimal Convert(decimal amount, CurrencyCode from, CurrencyCode to)
    {
        if (_currentRates.Count == 0)
            RefreshRates();

        if (from == to)
            return amount;

        double rateFrom = _currentRates[from];
        double rateTo = _currentRates[to];
        double rate = rateTo / rateFrom;

        decimal result = amount * (decimal)rate;
        return Math.Round(result, 2);
    }
}
