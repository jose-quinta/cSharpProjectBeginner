bool salir = false;

while (!salir)
{
    Console.WriteLine("\n=== CALCULADORA ===");
    Console.WriteLine("1. Sumar");
    Console.WriteLine("2. Restar");
    Console.WriteLine("3. Multiplicar");
    Console.WriteLine("4. Dividir");
    Console.WriteLine("5. Salir");
    Console.Write("Seleccione una opción: ");

    string opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            Calcular("+");
            break;
        case "2":
            Calcular("-");
            break;
        case "3":
            Calcular("*");
            break;
        case "4":
            Calcular("/");
            break;
        case "5":
            salir = true;
            Console.WriteLine("¡Hasta luego!");
            break;
        default:
            Console.WriteLine("Opción no válida. Intente de nuevo.");
            break;
    }
}

static void Calcular(string operador)
{
    Console.Write("Ingrese el primer número: ");
    if (!double.TryParse(Console.ReadLine(), out double num1))
    {
        Console.WriteLine("Número no válido.");
        return;
    }

    Console.Write("Ingrese el segundo número: ");
    if (!double.TryParse(Console.ReadLine(), out double num2))
    {
        Console.WriteLine("Número no válido.");
        return;
    }

    double resultado = operador switch
    {
        "+" => num1 + num2,
        "-" => num1 - num2,
        "*" => num1 * num2,
        "/" when num2 == 0 => double.NaN,
        "/" => num1 / num2,
        _ => double.NaN
    };

    if (double.IsNaN(resultado))
        Console.WriteLine("Error: No se puede dividir por cero.");
    else
        Console.WriteLine($"Resultado: {num1} {operador} {num2} = {resultado}");
}
