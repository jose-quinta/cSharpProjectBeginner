using System.Globalization;
using contact_book_application.Models;

namespace contact_book_application.Services;

public class MenuService
{
    private static readonly TextInfo TextInfo = CultureInfo.CurrentCulture.TextInfo;

    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== CONTACT BOOK ===");
        Console.ResetColor();
    }

    public void ShowMenu()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("1.  Agregar contacto");
        Console.WriteLine("2.  Buscar contacto");
        Console.WriteLine("3.  Ver todos los contactos");
        Console.WriteLine("4.  Editar contacto");
        Console.WriteLine("5.  Eliminar contacto");
        Console.WriteLine("6.  Salir");
        Console.ResetColor();
        Console.Write("Seleccione una opci\u00f3n (n\u00famero o letra): ");
    }

    public string GetChoice()
    {
        var key = Console.ReadKey(true);
        return key.Key.ToString() switch
        {
            "D1" or "NumPad1" => "1",
            "D2" or "NumPad2" => "2",
            "D3" or "NumPad3" => "3",
            "D4" or "NumPad4" => "4",
            "D5" or "NumPad5" => "5",
            "D6" or "NumPad6" => "6",
            "Escape" => "6",
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "a" => "1",
                "b" => "2",
                "v" => "3",
                "e" => "4",
                "d" => "5",
                "s" or "x" => "6",
                _ => ""
            }
        };
    }

    public Contact GetNewContact()
    {
        Contact contact = new Contact();

        Console.Write("Nombre: ");
        contact.Name = ToTitleCase(Console.ReadLine() ?? "").Trim();

        Console.Write("Tel\u00e9fono: ");
        contact.Phone = (Console.ReadLine() ?? "").Trim();

        Console.Write("Email (opcional): ");
        string email = (Console.ReadLine() ?? "").Trim();
        contact.Email = string.IsNullOrWhiteSpace(email) ? null : email;

        return contact;
    }

    public Contact GetUpdatedContact(Contact existing)
    {
        Contact contact = new Contact();

        Console.Write($"Nombre [{existing.Name}]: ");
        string name = (Console.ReadLine() ?? "").Trim();
        contact.Name = string.IsNullOrWhiteSpace(name) ? existing.Name : ToTitleCase(name);

        Console.Write($"Tel\u00e9fono [{existing.Phone}]: ");
        string phone = (Console.ReadLine() ?? "").Trim();
        contact.Phone = string.IsNullOrWhiteSpace(phone) ? existing.Phone : phone;

        string currentEmail = existing.Email ?? "";
        Console.Write($"Email [{currentEmail}]: ");
        string email = (Console.ReadLine() ?? "").Trim();
        contact.Email = string.IsNullOrWhiteSpace(email) ? existing.Email : email;

        return contact;
    }

    public string GetSearchQuery()
    {
        Console.Write("Ingrese t\u00e9rmino de b\u00fasqueda (nombre/tel\u00e9fono/email): ");
        return (Console.ReadLine() ?? "").Trim();
    }

    public string GetContactName(string prompt)
    {
        Console.Write(prompt);
        return (Console.ReadLine() ?? "").Trim();
    }

    public void ShowContactList(List<Contact> contacts)
    {
        Console.WriteLine();

        if (contacts.Count == 0)
        {
            ShowMessage("No hay contactos en la agenda.", ConsoleColor.Yellow);
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Contactos ({contacts.Count}):");
        Console.ResetColor();

        Console.WriteLine(new string('-', 60));
        Console.WriteLine($"{"Nombre",-20} {"Tel\u00e9fono",-15} {"Email",-20}");
        Console.WriteLine(new string('-', 60));

        foreach (Contact c in contacts)
        {
            string email = c.Email ?? "-";
            Console.WriteLine($"{c.Name,-20} {c.Phone,-15} {email,-20}");
        }
    }

    public void ShowContactDetail(Contact c)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Nombre:    {c.Name}");
        Console.WriteLine($"Tel\u00e9fono:  {c.Phone}");
        Console.WriteLine($"Email:     {c.Email ?? "(sin email)"}");
        Console.WriteLine($"Creado:    {c.CreatedAt:dd/MM/yyyy HH:mm}");
        Console.ResetColor();
    }

    public void ShowMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public bool ConfirmAction(string prompt)
    {
        Console.Write(prompt);
        var key = Console.ReadKey(true);
        Console.WriteLine(key.KeyChar);
        return key.KeyChar is 's' or 'S';
    }

    public void Pause()
    {
        Console.Write("\nPresione cualquier tecla para continuar...");
        Console.ReadKey(true);
    }

    private static string ToTitleCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;
        return TextInfo.ToTitleCase(input.ToLower());
    }
}
