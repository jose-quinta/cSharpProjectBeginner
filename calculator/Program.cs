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

bool salir = false;
double a = 0, b = 0, r = 0;
bool success = false;

try
{
    while (!salir)
    {
        menu.ShowMenu();

        string opcion = menu.GetChoice();


        if (Enum.TryParse<Operation>(opcion, out var pOpcion)
            && operations.TryGetValue(pOpcion, out var operation))
        {
            (success, a, b) = menu.GetNumber();
            if (!success) break;
            r = operation.func(a, b);
            menu.ShowResult(a, operation.symbol, b, r);
        }
        else
        {
            switch (opcion)
            {
                case "5":
                    (success, a) = menu.GetSingleNumber();
                    if (!success) break;
                    r = calculator.Sqrt(a);
                    menu.ShowResult(a, "√", r);
                    break;
                case "6":
                    calculator.MemoryClear();
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "7":
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "8":
                    if (double.IsNaN(r))
                        break;
                    calculator.MemoryStore(r);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "9":
                    if (double.IsNaN(r))
                        break;
                    calculator.MemoryAdd(r);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "10":
                    if (double.IsNaN(r))
                        break;
                    calculator.MemorySubtract(r);
                    menu.ShowMemoryStatus(calculator.MemoryRecall());
                    break;
                case "11":
                    salir = true;
                    Console.WriteLine("¡Hasta luego!");
                    break;
                default:
                    Console.WriteLine("Opción no válida. Intente de nuevo.");
                    break;
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error inesperado: {ex.Message}");
}
