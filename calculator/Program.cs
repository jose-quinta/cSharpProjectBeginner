using calculator.Models;
using calculator.Services;

MenuService menu = new MenuService();
Calculator calculator = new Calculator();

Dictionary<string, Func<double, double, double>> symbolOperations
    = new Dictionary<string, Func<double, double, double>>()
{
    { "+", calculator.Sum },
    { "-", calculator.Subtract },
    { "*", calculator.Multiply },
    { "/", calculator.Divide },
    { "^", calculator.Power },
    { "%", calculator.Mod },
};

Dictionary<Operation, (string symbol, Func<double, double, double> func)> operations
    = new Dictionary<Operation, (string symbol, Func<double, double, double> func)>()
{
    { Operation.Add, ( "+",  symbolOperations["+"] ) },
    { Operation.Subtract, ( "-", symbolOperations["-"] )},
    { Operation.Multiply, ( "*", symbolOperations["*"] )},
    { Operation.Divide, ( "/", symbolOperations["/"] ) },
    { Operation.Power, ( "^", symbolOperations["^"] ) },
    { Operation.Mod, ("%", symbolOperations["%"]) },
};

Dictionary<Trigonometric, (string symbol, Func<double, double> func)> trigOperations
    = new Dictionary<Trigonometric, (string symbol, Func<double, double> func)>()
{
    { Trigonometric.Sin, ("sin", calculator.Sin) },
    { Trigonometric.Cos, ("cos", calculator.Cos) },
    { Trigonometric.Tan, ("tan", calculator.Tan) },
    { Trigonometric.Log10, ("log10", calculator.Log10) },
    { Trigonometric.Abs, ("abs", calculator.Abs) },
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
            if (!double.IsNaN(r))
                (success, a, b) = menu.GetNumbers(r);
            else
                (success, a, b) = menu.GetNumbers(null);
            if (!success) continue;
            r = operation.func(a, b);
            menu.ShowResult(a, operation.symbol, b, r);
            if (!double.IsNaN(r))
                calculator.History.Add($"{a} {operation.symbol} {b} = {r}");
        }
        else if (Enum.TryParse<Trigonometric>(opcion, out var tOpcion)
            && trigOperations.TryGetValue(tOpcion, out var trigOperation))
        {
            (success, a) = menu.GetSingleNumber();
            if (!success) continue;
            r = trigOperation.func(a);
            menu.ShowResult(a, trigOperation.symbol, r);
            if (!double.IsNaN(r))
                calculator.History.Add($"{trigOperation.symbol}({a}) = {r}");
        }
        else
        {
            switch (opcion)
            {
                case "7":
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
                case "8":
                    calculator.MemoryClear();
                    calculator.SaveMemoryToFile(memoryFile);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "9":
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "10":
                    if (double.IsNaN(r)) continue;
                    calculator.MemoryStore(r);
                    calculator.SaveMemoryToFile(memoryFile);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "11":
                    if (double.IsNaN(r)) continue;
                    calculator.MemoryAdd(r);
                    calculator.SaveMemoryToFile(memoryFile);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "12":
                    if (double.IsNaN(r)) continue;
                    calculator.MemorySubtract(r);
                    calculator.SaveMemoryToFile(memoryFile);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "13":
                    menu.ShowHistory(calculator.History);
                    Console.Write("\u00BFLimpiar historial? [S/N]: ");
                    var clearKey = Console.ReadKey(intercept: true);
                    Console.WriteLine(clearKey.KeyChar);
                    if (MenuService.IsConfirmKey(clearKey.KeyChar))
                        calculator.ClearHistory();
                    break;
                case "14":
                    string op;
                    (success, a, op, b) = menu.ParseExpression();
                    if (!success) continue;

                    if (op == "x") op = "*";

                    if (!symbolOperations.TryGetValue(op, out var exprFunc))
                    {
                        Console.WriteLine("Operador no v\u00E1lido. Use: +, -, *, /, ^, %");
                        continue;
                    }
                    r = exprFunc(a, b);
                    menu.ShowResult(a, op, b, r);
                    if (!double.IsNaN(r))
                        calculator.History.Add($"{a} {op} {b} = {r}");
                    break;
                case "20":
                    Console.Write("Ingrese n\u00FAmero entero no negativo: ");
                    if (!int.TryParse(Console.ReadLine(), out int n) || n < 0)
                    {
                        Console.WriteLine("Debe ingresar un entero >= 0.");
                        continue;
                    }
                    r = calculator.Factorial(n);
                    menu.ShowResult(n, "!", r);
                    if (!double.IsNaN(r))
                        calculator.History.Add($"{n}! = {r}");
                    break;
                case "21":
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
