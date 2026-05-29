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
        double? result = calc.MemoryRecall();

        Assert.Equal(42.0, result);
        Assert.True(calc.HasMemory);
        Assert.Equal(42.0, calc.Memory);
    }

    [Fact]
    public void MemoryClear_ResetsMemory()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(42.0);
        calc.MemoryClear();

        Assert.Equal(double.NaN, calc.MemoryRecall());
        Assert.False(calc.HasMemory);
        Assert.Equal(0.0, calc.Memory);
    }

    [Fact]
    public void MemoryAdd_AccumulatesValue()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(10.0);
        calc.MemoryAdd(5.0);
        double? result = calc.MemoryRecall();

        Assert.Equal(15.0, result);
    }

    [Fact]
    public void MemorySubtract_DecrementsValue()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(10.0);
        calc.MemorySubtract(3.0);
        double? result = calc.MemoryRecall();

        Assert.Equal(7.0, result);
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
    public void LoadMemoryFromFile_MissingFile_DoesNothing()
    {
        Calculator calc = new Calculator();
        calc.LoadMemoryFromFile("nonexistent.dat");
        double? result = calc.MemoryRecall();

        Assert.Equal(double.NaN, result);
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

    [Fact]
    public void Power_ReturnsCorrectResult()
    {
        Calculator calc = new Calculator();
        double result = calc.Power(2.0, 3.0);

        Assert.Equal(8.0, result);
    }

    [Fact]
    public void Power_ExponentZero_ReturnsOne()
    {
        Calculator calc = new Calculator();
        double result = calc.Power(5.0, 0.0);

        Assert.Equal(1.0, result);
    }

    [Fact]
    public void Power_NegativeExponent_ReturnsFraction()
    {
        Calculator calc = new Calculator();
        double result = calc.Power(2.0, -1.0);

        Assert.Equal(0.5, result);
    }
    [Fact]
    public void Mod_ReturnsRemainder()
    {
        Calculator calc = new Calculator();
        double result = calc.Mod(10.0, 3.0);

        Assert.Equal(1.0, result);
    }

    [Fact]
    public void Mod_Divisible_ReturnsZero()
    {
        Calculator calc = new Calculator();
        double result = calc.Mod(9.0, 3.0);

        Assert.Equal(0.0, result);
    }

    [Fact]
    public void Mod_ByZero_ReturnsNaN()
    {
        Calculator calc = new Calculator();
        double result = calc.Mod(5.0, 0.0);

        Assert.True(double.IsNaN(result));
    }

    [Fact]
    public void Sin_Zero_ReturnsZero()
    {
        Calculator calc = new Calculator();
        double result = calc.Sin(0.0);

        Assert.Equal(0.0, result);
    }

    [Fact]
    public void Sin_PiHalves_ReturnsOne()
    {
        Calculator calc = new Calculator();
        double result = calc.Sin(Math.PI / 2);

        Assert.Equal(1.0, result, 5);
    }

    [Fact]
    public void Cos_Zero_ReturnsOne()
    {
        Calculator calc = new Calculator();
        double result = calc.Cos(0.0);

        Assert.Equal(1.0, result);
    }

    [Fact]
    public void Cos_Pi_ReturnsNegativeOne()
    {
        Calculator calc = new Calculator();
        double result = calc.Cos(Math.PI);

        Assert.Equal(-1.0, result, 5);
    }

    [Fact]
    public void Tan_Zero_ReturnsZero()
    {
        Calculator calc = new Calculator();
        double result = calc.Tan(0.0);

        Assert.Equal(0.0, result);
    }

    [Fact]
    public void Log10_Hundred_ReturnsTwo()
    {
        Calculator calc = new Calculator();
        double result = calc.Log10(100.0);

        Assert.Equal(2.0, result);
    }

    [Fact]
    public void Log10_Zero_ReturnsNaN()
    {
        Calculator calc = new Calculator();
        double result = calc.Log10(0.0);

        Assert.True(double.IsNaN(result));
    }

    [Fact]
    public void Log10_Negative_ReturnsNaN()
    {
        Calculator calc = new Calculator();
        double result = calc.Log10(-5.0);

        Assert.True(double.IsNaN(result));
    }

    [Fact]
    public void Abs_Negative_ReturnsPositive()
    {
        Calculator calc = new Calculator();
        double result = calc.Abs(-10.0);

        Assert.Equal(10.0, result);
    }

    [Fact]
    public void Abs_Zero_ReturnsZero()
    {
        Calculator calc = new Calculator();
        double result = calc.Abs(0.0);

        Assert.Equal(0.0, result);
    }

    [Fact]
    public void Abs_Positive_ReturnsSame()
    {
        Calculator calc = new Calculator();
        double result = calc.Abs(5.0);

        Assert.Equal(5.0, result);
    }

    [Fact]
    public void Factorial_Zero_ReturnsOne()
    {
        Calculator calc = new Calculator();
        double result = calc.Factorial(0);

        Assert.Equal(1.0, result);
    }

    [Fact]
    public void Factorial_Five_Returns120()
    {
        Calculator calc = new Calculator();
        double result = calc.Factorial(5);

        Assert.Equal(120.0, result);
    }

    [Fact]
    public void Factorial_Negative_ReturnsNaN()
    {
        Calculator calc = new Calculator();
        double result = calc.Factorial(-3);

        Assert.True(double.IsNaN(result));
    }

    [Fact]
    public void SaveAndLoadMemoryToFile_PersistsValue()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(99.9);

        string tempFile = Path.GetTempFileName();
        try
        {
            calc.SaveMemoryToFile(tempFile);

            Calculator loaded = new Calculator();
            loaded.LoadMemoryFromFile(tempFile);

            double? result = loaded.MemoryRecall();
            Assert.Equal(99.9, result);
            Assert.True(loaded.HasMemory);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public void HasMemory_AfterStore_ReturnsTrue()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(10.0);

        Assert.True(calc.HasMemory);
    }

    [Fact]
    public void Memory_ReturnsStoredValue()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(15.0);

        Assert.Equal(15.0, calc.Memory);
    }

    [Fact]
    public void MemoryStore_Zero_ShowsZero()
    {
        Calculator calc = new Calculator();
        calc.MemoryStore(0.0);

        Assert.Equal(0.0, calc.Memory);
    }

    [Fact]
    public void MemoryAdd_WithoutPriorStore_SetsMemory()
    {
        Calculator calc = new Calculator();
        calc.MemoryAdd(5.0);

        Assert.Equal(5.0, calc.Memory);
    }

    [Fact]
    public void MemorySubtract_WithoutPriorStore_SetsMemory()
    {
        Calculator calc = new Calculator();
        calc.MemorySubtract(3.0);

        Assert.Equal(-3.0, calc.Memory);
    }

    [Fact]
    public void LoadMemoryFromFile_InvalidContent_Ignored()
    {
        string tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".dat");
        try
        {
            File.WriteAllText(tempFile, "invalid content");

            Calculator calc = new Calculator();
            calc.LoadMemoryFromFile(tempFile);
            double? result = calc.MemoryRecall();

            Assert.Equal(double.NaN, result);
        }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }
    }

    [Fact]
    public void Subtract_NegativeResult_ReturnsNegative()
    {
        Calculator calc = new Calculator();
        double result = calc.Subtract(5.0, 10.0);

        Assert.Equal(-5.0, result);
    }

    [Fact]
    public void Multiply_ByZero_ReturnsZero()
    {
        Calculator calc = new Calculator();
        double result = calc.Multiply(7.0, 0.0);

        Assert.Equal(0.0, result);
    }

    [Fact]
    public void Sum_WithNaN_ReturnsNaN()
    {
        Calculator calc = new Calculator();
        double result = calc.Sum(double.NaN, 5.0);

        Assert.True(double.IsNaN(result));
    }

    [Fact]
    public void Divide_NegativeDividend_ReturnsNegative()
    {
        Calculator calc = new Calculator();
        double result = calc.Divide(-10.0, 2.0);

        Assert.Equal(-5.0, result);
    }

    [Fact]
    public void Divide_ZeroDividend_ReturnsZero()
    {
        Calculator calc = new Calculator();
        double result = calc.Divide(0.0, 5.0);

        Assert.Equal(0.0, result);
    }

    [Fact]
    public void Sqrt_LargeNumber_ReturnsCorrectResult()
    {
        Calculator calc = new Calculator();
        double result = calc.Sqrt(1e10);

        Assert.Equal(1e5, result);
    }

    [Fact]
    public void Sqrt_FractionalNumber_ReturnsCorrectResult()
    {
        Calculator calc = new Calculator();
        double result = calc.Sqrt(0.25);

        Assert.Equal(0.5, result);
    }
}
