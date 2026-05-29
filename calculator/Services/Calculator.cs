using System.Globalization;
using calculator.Abstractions;

namespace calculator.Services;

public class Calculator : ICalculator
{
    private double _memory;
    private bool _hasMemory;
    public bool HasMemory => _hasMemory;
    public double Memory => _memory;
    public List<string> History { get; } = new List<string>();

    public void ClearHistory() => History.Clear();

    public void MemoryStore(double value)
    {
        _memory = value;
        _hasMemory = true;
    }

    public void MemoryClear()
    {
        _memory = 0;
        _hasMemory = false;
    }

    public double? MemoryRecall() => (!_hasMemory) ? double.NaN : _memory;

    public void MemoryAdd(double value)
    {
        _memory += value;
        _hasMemory = true;
    }

    public void MemorySubtract(double value)
    {
        _memory -= value;
        _hasMemory = true;
    }

    public double Sum(double a, double b) => a + b;
    public double Subtract(double a, double b) => a - b;
    public double Multiply(double a, double b) => a * b;
    public double Divide(double a, double b) => b != 0 ? a / b : double.NaN;
    public double Sqrt(double a) => a >= 0 ? Math.Sqrt(a) : double.NaN;
    public double Power(double _base, double _exponent) => Math.Pow(_base, _exponent);
    public double Mod(double a, double b) => b != 0 ? a % b : double.NaN;
    public double Sin(double angle) => Math.Sin(angle);
    public double Cos(double angle) => Math.Cos(angle);
    public double Tan(double angle) => Math.Tan(angle);
    public double Log10(double a) => a > 0 ? Math.Log10(a) : double.NaN;
    public double Abs(double a) => Math.Abs(a);
    public double Factorial(int n)
    {
        if (n < 0) return double.NaN;
        if (n == 0) return 1;
        double result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }
    public void SaveMemoryToFile(string path)
    {
        File.WriteAllText(path, _memory.ToString("G", CultureInfo.InvariantCulture));
    }

    public void LoadMemoryFromFile(string path)
    {
        if (File.Exists(path))
        {
            string content = File.ReadAllText(path).Trim();
            if (double.TryParse(content, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                _memory = value;
                _hasMemory = true;
            }
        }
    }
}
