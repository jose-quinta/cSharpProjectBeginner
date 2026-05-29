namespace calculator.Abstractions;

public interface ICalculator
{
    double Sum(double a, double b);
    double Subtract(double a, double b);
    double Multiply(double a, double b);
    double Divide(double a, double b);
    double Sqrt(double a);
    double Power(double _base, double _exponent);
    double Mod(double a, double b);
    double Sin(double angle);
    double Cos(double angle);
    double Tan(double angle);
    double Log10(double a);
    double Abs(double a);
    double Factorial(int n);
    List<string> History { get; }
    void ClearHistory();
    bool HasMemory { get; }
    double Memory { get; }
    void MemoryStore(double value);
    void MemoryClear();
    double? MemoryRecall();
    void MemoryAdd(double value);
    void MemorySubtract(double value);
}
