namespace calculator.Tests;

using Xunit;
using calculator.Services;
using System.IO;

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

    [Fact]
    public void ClearHistory_RemovesAllEntries()
    {
        Calculator calc = new Calculator();
        calc.History.Add("5 + 3 = 8");
        calc.History.Add("10 - 4 = 6");

        calc.ClearHistory();

        Assert.Empty(calc.History);
    }

    [Fact]
    public void History_StoresFormattedStrings()
    {
        Calculator calc = new Calculator();
        calc.History.Add("5 + 3 = 8");

        Assert.Single(calc.History);
        Assert.Equal("5 + 3 = 8", calc.History[0]);
    }

    [Fact]
    public void SaveAndLoadMemoryToFile_PersistsValue()
    {
        string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".dat");
        try
        {
            Calculator calc = new Calculator();
            calc.MemoryStore(42.5);
            calc.SaveMemoryToFile(tempFile);

            Calculator calc2 = new Calculator();
            calc2.LoadMemoryFromFile(tempFile);
            string result = calc2.MemoryRecall();

            Assert.Equal("Valor en memoria: 42.5", result);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public void LoadMemoryFromFile_MissingFile_DoesNothing()
    {
        Calculator calc = new Calculator();
        calc.LoadMemoryFromFile("nonexistent.dat");
        string result = calc.MemoryRecall();

        Assert.Equal("No hay valor en memoria.", result);
    }

    [Fact]
    public void IsConfirmKey_ReturnsTrueForS()
    {
        Assert.True(MenuService.IsConfirmKey('s'));
    }

    [Fact]
    public void IsConfirmKey_ReturnsTrueForUpperCaseS()
    {
        Assert.True(MenuService.IsConfirmKey('S'));
    }

    [Fact]
    public void IsConfirmKey_ReturnsFalseForN()
    {
        Assert.False(MenuService.IsConfirmKey('n'));
    }

    [Fact]
    public void IsConfirmKey_ReturnsFalseForOther()
    {
        Assert.False(MenuService.IsConfirmKey('x'));
    }

    [Fact]
    public void ParseExpressionString_ValidInput_ReturnsCorrectValues()
    {
        var (success, a, op, b) = MenuService.ParseExpressionString("5 + 3");

        Assert.True(success);
        Assert.Equal(5, a);
        Assert.Equal("+", op);
        Assert.Equal(3, b);
    }

    [Fact]
    public void ParseExpressionString_ExtraSpaces_WorksCorrectly()
    {
        var (success, a, op, b) = MenuService.ParseExpressionString("10   -   4");

        Assert.True(success);
        Assert.Equal(10, a);
        Assert.Equal("-", op);
        Assert.Equal(4, b);
    }

    [Fact]
    public void ParseExpressionString_InvalidFormat_ReturnsFailure()
    {
        var (success, a, op, b) = MenuService.ParseExpressionString("abc");

        Assert.False(success);
        Assert.Equal(0, a);
        Assert.Equal("", op);
        Assert.Equal(0, b);
    }

    [Fact]
    public void ParseExpressionString_EmptyString_ReturnsFailure()
    {
        var (success, _, _, _) = MenuService.ParseExpressionString("");

        Assert.False(success);
    }

    [Fact]
    public void ParseExpressionString_InvalidNumber_ReturnsFailure()
    {
        var (success, _, _, _) = MenuService.ParseExpressionString("abc + 3");

        Assert.False(success);
    }

    [Fact]
    public void ParseExpressionString_UnknownOperator_StillParses()
    {
        var (success, a, op, b) = MenuService.ParseExpressionString("5 ^ 3");

        Assert.True(success);
        Assert.Equal("^", op);
    }
}
