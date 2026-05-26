MenuService menu = new MenuService();
Calculator calculator = new Calculator();
bool salir = false;

while (!salir)
{
    menu.ShowMenu();

    string opcion = menu.GetChoice();

    double a = 0, b = 0, r = 0;
    bool success = false;

    switch (opcion)
    {
        case "1":
            (success, a, b) = menu.GetNumber();
            if (!success) break;
            r = calculator.Sum(a, b);
            menu.ShowResult(a, "+", b, r);
            break;
        case "2":
            (success, a, b) = menu.GetNumber();
            if (!success) break;
            r = calculator.Subtract(a, b);
            menu.ShowResult(a, "-", b, r);
            break;
        case "3":
            (success, a, b) = menu.GetNumber();
            if (!success) break;
            r = calculator.Multiply(a, b);
            menu.ShowResult(a, "*", b, r);
            break;
        case "4":
            (success, a, b) = menu.GetNumber();
            if (!success) break;
            r = calculator.Divide(a, b);
            menu.ShowResult(a, "/", b, r);
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