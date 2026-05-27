using calculator.Models;
using calculator.Services;

MenuService menu = new MenuService();
Calculator calculator = new Calculator();

Dictionary<Operation, (string symbol, Func<double, double, double> func)> operations
    = new Dictionary<Operation, (string symbol, Func<double, double, double> func)>()
{
    { Operation.Add, ( "+",  calculator.Sum ) },
    { Operation.Subtract, ( "-", calculator.Subtract )},
    { Operation.Multiply, ( "*", calculator.Multiply )},
    { Operation.Divide, ( "/", calculator.Divide ) }
};

string memoryFile = Path.Combine(Directory.GetCurrentDirectory(), "memory.dat");
calculator.LoadMemoryFromFile(memoryFile);

bool salir = false;
double a = 0, b = 0, r = double.NaN;
bool success = false;

try
{
    while (!salir)
    {
        Console.Clear();

        if (!double.IsNaN(r))
            menu.ShowResultBanner(r);

        menu.ShowMenu();

        string opcion = menu.GetChoice();
        Console.Write("\n");

        if (Enum.TryParse<Operation>(opcion, out var pOpcion)
            && operations.TryGetValue(pOpcion, out var operation))
        {
            (success, a, b) = menu.GetNumbers(r);
            if (!success) continue;
            r = operation.func(a, b);
            menu.ShowResult(a, operation.symbol, b, r);
            if (!double.IsNaN(r))
                calculator.History.Add($"{a} {operation.symbol} {b} = {r}");
        }
        else
        {
            switch (opcion)
            {
                case "5":
                    if (!double.IsNaN(r) && menu.ConfirmUseLastResult(r))
                        a = r;
                    else
                    {
                        (success, a) = menu.GetSingleNumber();
                        if (!success) continue;
                    }
                    r = calculator.Sqrt(a);
                    menu.ShowResult(a, "\u221A", r);
                    if (!double.IsNaN(r))
                        calculator.History.Add($"\u221A{a} = {r}");
                    break;
                case "6":
                    calculator.MemoryClear();
                    calculator.SaveMemoryToFile(memoryFile);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "7":
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "8":
                    if (double.IsNaN(r)) continue;
                    calculator.MemoryStore(r);
                    calculator.SaveMemoryToFile(memoryFile);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "9":
                    if (double.IsNaN(r)) continue;
                    calculator.MemoryAdd(r);
                    calculator.SaveMemoryToFile(memoryFile);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "10":
                    if (double.IsNaN(r)) continue;
                    calculator.MemorySubtract(r);
                    calculator.SaveMemoryToFile(memoryFile);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "11":
                    menu.ShowHistory(calculator.History);
                    break;
                case "12":
                    string op;
                    (success, a, op, b) = menu.ParseExpression();
                    if (!success) continue;
                    string? symbol = op switch
                    {
                        "+" => "+",
                        "-" => "-",
                        "*" or "x" => "*",
                        "/" => "/",
                        _ => null
                    };
                    if (symbol == null)
                    {
                        Console.WriteLine("Operador no v\u00E1lido. Use: +, -, *, /");
                        continue;
                    }
                    r = symbol switch
                    {
                        "+" => calculator.Sum(a, b),
                        "-" => calculator.Subtract(a, b),
                        "*" => calculator.Multiply(a, b),
                        "/" => calculator.Divide(a, b),
                        _ => double.NaN
                    };
                    menu.ShowResult(a, symbol, b, r);
                    if (!double.IsNaN(r))
                        calculator.History.Add($"{a} {symbol} {b} = {r}");
                    break;
                case "13":
                    Console.Write("\u00BFEst\u00E1 seguro que desea salir? [S/N]: ");
                    var confirm = Console.ReadKey(intercept: true);
                    Console.WriteLine(confirm.KeyChar);
                    if (MenuService.IsConfirmKey(confirm.KeyChar))
                    {
                        salir = true;
                        Console.WriteLine("\u00A1Hasta luego!");
                    }
                    break;
                default:
                    Console.WriteLine("Opci\u00F3n no v\u00E1lida. Intente de nuevo.");
                    break;
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error inesperado: {ex.Message}");
}
