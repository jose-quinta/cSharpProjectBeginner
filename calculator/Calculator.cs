public class Calculator
{
    // Use properties if you want to use stateful.
    // public double a { get; set; }
    // public double b { get; set; }

    public double Sum(double a, double b)
        => a + b;
    public double Subtract(double a, double b)
        => a - b;
    public double Multiply(double a, double b)
        => a * b;
    public double Divide(double a, double b)
        => b != 0 ? a / b : double.NaN;
}