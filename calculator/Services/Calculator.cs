using calculator.Abstractions;

namespace calculator.Services;

public class Calculator : ICalculator
{
    private double _memory;
    private bool _hasMemory;
    public bool HasMemory => _hasMemory;
    public double Memory => _memory;
    public List<double> History { get; } = new List<double>();

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

    public string MemoryRecall()
    {
        if (!_hasMemory)
            return "No hay valor en memoria.";
        else
            return $"Valor en memoria: {_memory}";
    }

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
}
