namespace calculator.Tests;

using Xunit;
using calculator.Services;

public class CalculatorTests
{
    [Fact]
    public void Sum_ReturnsCorrectResult()
    {
        Calculator calc = new Calculator();
        double result = calc.Sum(5.0, 3.0);

        Assert.Equal(8.0, result);
    }

    [Fact]
    public void Subtract_ReturnsCorrectResult()
    {
        Calculator calc = new Calculator();
        double result = calc.Subtract(10.0, 4.0);

        Assert.Equal(6.0, result);
    }

    [Fact]
    public void Multiply_ReturnsCorrectResult()
    {
        Calculator calc = new Calculator();
        double result = calc.Multiply(3.0, 4.0);

        Assert.Equal(12.0, result);
    }

    [Fact]
    public void Divide_ReturnsCorrectResult()
    {
        Calculator calc = new Calculator();
        double result = calc.Divide(10.0, 2.0);

        Assert.Equal(5.0, result);
    }

    [Fact]
    public void Divide_ByZero_ReturnsNaN()
    {
        Calculator calc = new Calculator();
        double result = calc.Divide(10.0, 0.0);

        Assert.True(double.IsNaN(result));
    }

    [Fact]
    public void MemoryStore_ThenMemoryRecall_ReturnsStoredValue()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(42.0);
        string result = calc.MemoryRecall();

        Assert.Equal("Valor en memoria: 42", result);
    }

    [Fact]
    public void MemoryClear_ResetsMemory()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(42.0);
        calc.MemoryClear();
        string result = calc.MemoryRecall();

        Assert.Equal("No hay valor en memoria.", result);
    }

    [Fact]
    public void MemoryAdd_AccumulatesValue()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(10.0);
        calc.MemoryAdd(5.0);
        string result = calc.MemoryRecall();

        Assert.Equal("Valor en memoria: 15", result);
    }

    [Fact]
    public void MemorySubtract_DecrementsValue()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(10.0);
        calc.MemorySubtract(3.0);
        string result = calc.MemoryRecall();

        Assert.Equal("Valor en memoria: 7", result);
    }

    [Fact]
    public void Sqrt_ReturnsCorrectResult()
    {
        Calculator calc = new Calculator();
        double result = calc.Sqrt(25.0);

        Assert.Equal(5.0, result);
    }

    [Fact]
    public void Sqrt_NegativeNumber_ReturnsNaN()
    {
        Calculator calc = new Calculator();
        double result = calc.Sqrt(-9.0);

        Assert.True(double.IsNaN(result));
    }

    [Fact]
    public void Sqrt_Zero_ReturnsZero()
    {
        Calculator calc = new Calculator();
        double result = calc.Sqrt(0.0);

        Assert.Equal(0.0, result);
    }
}
