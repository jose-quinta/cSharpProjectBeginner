Dictionary<string, string> contactos = new();
bool salir = false;

while (!salir)
{
    Console.WriteLine("\n=== CONTACT BOOK MENU ===");
    Console.WriteLine("1. Agregar contacto");
    Console.WriteLine("2. Buscar contacto");
    Console.WriteLine("3. Ver contactos");
    Console.WriteLine("4. Eliminar contacto");
    Console.WriteLine("5. Salir");
    Console.Write("Seleccione una opción: ");

    string opcion = Console.ReadLine() ?? "";
    string nombre = string.Empty;
    string telefono = string.Empty;

    switch (opcion)
    {
        case "1":
            Console.Write("Ingrese el nombre del contacto: ");
            nombre = Console.ReadLine() ?? "";
            Console.Write("Ingrese el número de teléfono: ");
            telefono = Console.ReadLine() ?? "";
            AgregarContacto(nombre, telefono, contactos);
            break;

        case "2":
            Console.Write("Ingrese el nombre del contacto a buscar: ");
            nombre = Console.ReadLine() ?? "";
            BuscarContacto(nombre, contactos);
            break;

        case "3":
            VerContactos(contactos);
            break;

        case "4":
            Console.Write("Ingrese el nombre del contacto a eliminar: ");
            nombre = Console.ReadLine() ?? "";
            EliminarContacto(nombre, contactos);
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

static void AgregarContacto(string nombre, string telefono, Dictionary<string, string> contactos)
{
    if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(telefono))
    {
        Console.WriteLine("Nombre y teléfono no pueden estar vacíos.");
        return;
    }

    if (!contactos.TryAdd(nombre, telefono))
    {
        Console.WriteLine("El contacto ya existe. Use otro nombre.");
        return;
    }

    Console.WriteLine("Contacto agregado exitosamente.");
}

static void BuscarContacto(string nombre, Dictionary<string, string> contactos)
{
    if (string.IsNullOrWhiteSpace(nombre))
    {
        Console.WriteLine("El nombre no puede estar vacío.");
        return;
    }

    if (contactos.TryGetValue(nombre, out string? telefono))
    {
        Console.WriteLine($"Contacto encontrado: {nombre} - {telefono}");
        return;
    }

    Console.WriteLine("Contacto no encontrado.");
}

static void VerContactos(Dictionary<string, string> contactos)
{
    if (contactos.Count == 0)
    {
        Console.WriteLine("No hay contactos en la agenda.");
        return;
    }

    Console.WriteLine("Contactos en la agenda:");
    foreach (var contacto in contactos)
    {
        Console.WriteLine($"{contacto.Key} - {contacto.Value}");
    }
}

static void EliminarContacto(string nombre, Dictionary<string, string> contactos)
{
    if (string.IsNullOrWhiteSpace(nombre))
    {
        Console.WriteLine("El nombre no puede estar vacío.");
        return;
    }

    if (contactos.Remove(nombre))
    {
        Console.WriteLine("Contacto eliminado exitosamente.");
    }

    Console.WriteLine("Contacto no encontrado.");
}
