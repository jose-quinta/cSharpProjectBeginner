using temperature_converter.Abstractions;
using temperature_converter.Models;

namespace temperature_converter.Services;

public class TemperatureService : ITemperatureService
{
    private static double CelsiusToFahrenheit(double value) => (value * 9.0 / 5.0) + 32.0;
    private static double FahrenheitToCelsius(double value) => (value - 32.0) * 5.0 / 9.0;
    private static double CelsiusToKelvin(double value) => value + 273.15;
    private static double KelvinToCelsius(double value) => value - 273.15;
    private static double FahrenheitToKelvin(double value) => (value - 32.0) * 5.0 / 9.0 + 273.15;
    private static double KelvinToFahrenheit(double value) => (value - 273.15) * 9.0 / 5.0 + 32.0;

    public double Convert(double value, TemperatureUnit from, TemperatureUnit to)
    {
        if (!IsValidTemperature(value, from))
            throw new ArgumentException($"Valor inv\u00e1lido: {value}°{from} est\u00e1 por debajo del cero absoluto.");

        if (from == to) return value;

        return (from, to) switch
        {
            (TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit) => CelsiusToFahrenheit(value),
            (TemperatureUnit.Celsius, TemperatureUnit.Kelvin) => CelsiusToKelvin(value),
            (TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius) => FahrenheitToCelsius(value),
            (TemperatureUnit.Fahrenheit, TemperatureUnit.Kelvin) => FahrenheitToKelvin(value),
            (TemperatureUnit.Kelvin, TemperatureUnit.Celsius) => KelvinToCelsius(value),
            (TemperatureUnit.Kelvin, TemperatureUnit.Fahrenheit) => KelvinToFahrenheit(value),
            _ => throw new InvalidOperationException($"Conversi\u00f3n de {from} a {to} no soportada.")
        };
    }

    public Dictionary<TemperatureUnit, double> ConvertAll(double value, TemperatureUnit from)
    {
        if (!IsValidTemperature(value, from))
            throw new ArgumentException($"Valor inv\u00e1lido: {value}°{from} est\u00e1 por debajo del cero absoluto.");

        Dictionary<TemperatureUnit, double> result = new Dictionary<TemperatureUnit, double>();
        foreach (TemperatureUnit unit in Enum.GetValues<TemperatureUnit>())
        {
            result[unit] = Math.Round(Convert(value, from, unit), 2);
        }
        return result;
    }

    public string GetFormula(TemperatureUnit from, TemperatureUnit to)
    {
        return (from, to) switch
        {
            (TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit) => "°F = (°C × 9/5) + 32",
            (TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius) => "°C = (°F - 32) × 5/9",
            (TemperatureUnit.Celsius, TemperatureUnit.Kelvin) => "K = °C + 273.15",
            (TemperatureUnit.Kelvin, TemperatureUnit.Celsius) => "°C = K - 273.15",
            (TemperatureUnit.Fahrenheit, TemperatureUnit.Kelvin) => "K = (°F - 32) × 5/9 + 273.15",
            (TemperatureUnit.Kelvin, TemperatureUnit.Fahrenheit) => "°F = (K - 273.15) × 9/5 + 32",
            (TemperatureUnit.Celsius, TemperatureUnit.Celsius) => "°C = °C (misma unidad)",
            (TemperatureUnit.Fahrenheit, TemperatureUnit.Fahrenheit) => "°F = °F (misma unidad)",
            (TemperatureUnit.Kelvin, TemperatureUnit.Kelvin) => "K = K (misma unidad)",
            _ => throw new InvalidOperationException($"F\u00f3rmula de {from} a {to} no disponible.")
        };
    }

    public bool IsValidTemperature(double value, TemperatureUnit unit)
    {
        return unit switch
        {
            TemperatureUnit.Celsius => value >= -273.15,
            TemperatureUnit.Fahrenheit => value >= -459.67,
            TemperatureUnit.Kelvin => value >= 0,
            _ => true
        };
    }
}
