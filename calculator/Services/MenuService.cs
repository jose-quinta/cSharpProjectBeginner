namespace calculator.Services;

public class MenuService
{
    public void ShowMenu()
    {
        Console.WriteLine("\n=== CALCULADORA ===");
        Console.WriteLine("1.  Sumar");
        Console.WriteLine("2.  Restar");
        Console.WriteLine("3.  Multiplicar");
        Console.WriteLine("4.  Dividir");
        Console.WriteLine("5.  √ (Raíz cuadrada)");
        Console.WriteLine("6.  MC (Limpiar memoria)");
        Console.WriteLine("7.  MR (Recuperar memoria)");
        Console.WriteLine("8.  MS (Guardar en memoria)");
        Console.WriteLine("9.  M+ (Sumar a memoria)");
        Console.WriteLine("10. M- (Restar de memoria)");
        Console.WriteLine("11. Salir");
        Console.Write("Seleccione una opción: ");
    }

    public string GetChoice()
    {
        string opcion = Console.ReadLine() ?? "";
        return opcion;
    }

    public (bool success, double a) GetSingleNumber()
    {
        Console.Write("Ingrese el número: ");
        if (!double.TryParse(Console.ReadLine(), out double number))
        {
            Console.WriteLine("Número no válido.");
            return (false, 0);
        }

        return (true, number);
    }

    public (bool success, double a, double b) GetNumber()
    {
        Console.Write("Ingrese el primer número: ");
        if (!double.TryParse(Console.ReadLine(), out double num1))
        {
            Console.WriteLine("Número no válido.");
            return (false, 0, 0);
        }

        Console.Write("Ingrese el segundo número: ");
        if (!double.TryParse(Console.ReadLine(), out double num2))
        {
            Console.WriteLine("Número no válido.");
            return (false, 0, 0);
        }

        return (true, num1, num2);
    }

    public void ShowResult(double a, string op, double b, double r)
    {
        if (double.IsNaN(r))
            ShowError("No se puede dividir por cero.");
        else
            Console.WriteLine($"Resultado: {a} {op} {b} = {r}");
    }

    public void ShowResult(double a, string op, double r)
    {
        if (double.IsNaN(r))
            ShowError("No se puede calcular la raíz de un número negativo.");
        else
            Console.WriteLine($"{op}{a} = {r}");
    }

    public void ShowMemoryStatus(string msg) => Console.WriteLine(msg);

    public void ShowError(string msg) => Console.WriteLine($"Error: {msg}");
}
