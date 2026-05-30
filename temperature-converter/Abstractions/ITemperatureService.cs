using temperature_converter.Models;

namespace temperature_converter.Abstractions;

public interface ITemperatureService
{
    double Convert(double value, TemperatureUnit from, TemperatureUnit to);
    Dictionary<TemperatureUnit, double> ConvertAll(double value, TemperatureUnit from);
    string GetFormula(TemperatureUnit from, TemperatureUnit to);
    bool IsValidTemperature(double value, TemperatureUnit unit);
}