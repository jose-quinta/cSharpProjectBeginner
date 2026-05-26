public class MenuService
{
    public void ShowMenu()
    {
        Console.WriteLine("\n=== CALCULADORA ===");
        Console.WriteLine("1. Sumar");
        Console.WriteLine("2. Restar");
        Console.WriteLine("3. Multiplicar");
        Console.WriteLine("4. Dividir");
        Console.WriteLine("5. Salir");
        Console.Write("Seleccione una opción: ");
    }

    public string GetChoice()
    {
        string opcion = Console.ReadLine() ?? "";
        return opcion;
    }

    public (bool success,double a, double b) GetNumber()
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

    public void ShowError(string msg)
    {
        Console.WriteLine($"Error: {msg}");
    }
}