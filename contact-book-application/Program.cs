using contact_book_application.Abstractions;
using contact_book_application.Models;
using contact_book_application.Services;

IContactService contactService = new ContactService();
MenuService menu = new MenuService();
List<Contact> contacts = contactService.Load();
bool salir = false;

while (!salir)
{
    Console.Clear();
    menu.ShowBanner();
    menu.ShowMenu();

    string opcion = menu.GetChoice();
    Console.WriteLine();

    switch (opcion)
    {
        case "1":
            Agregar();
            break;
        case "2":
            Buscar();
            break;
        case "3":
            VerTodos();
            break;
        case "4":
            Editar();
            break;
        case "5":
            Eliminar();
            break;
        case "6":
            salir = true;
            menu.ShowMessage("\u00a1Hasta luego!", ConsoleColor.Cyan);
            break;
        default:
            menu.ShowMessage("Opci\u00f3n no v\u00e1lida.", ConsoleColor.Red);
            menu.Pause();
            break;
    }
}

void Agregar()
{
    Contact c = menu.GetNewContact();
    string validation = ((ContactService)contactService).ValidateContact(c);

    if (!string.IsNullOrEmpty(validation))
    {
        menu.ShowMessage(validation, ConsoleColor.Red);
        menu.Pause();
        return;
    }

    if (contactService.FindByName(c.Name) != null)
    {
        menu.ShowMessage("Ya existe un contacto con ese nombre.", ConsoleColor.Red);
        menu.Pause();
        return;
    }

    contactService.Add(c);
    menu.ShowMessage("Contacto agregado exitosamente.", ConsoleColor.Green);
    menu.Pause();
}

void Buscar()
{
    string query = menu.GetSearchQuery();

    if (string.IsNullOrWhiteSpace(query))
    {
        menu.ShowMessage("Debe ingresar un t\u00e9rmino de b\u00fasqueda.", ConsoleColor.Red);
        menu.Pause();
        return;
    }

    List<Contact> resultados = contactService.Search(query);

    if (resultados.Count == 0)
    {
        menu.ShowMessage("No se encontraron contactos.", ConsoleColor.Yellow);
        menu.Pause();
        return;
    }

    if (resultados.Count == 1)
    {
        menu.ShowContactDetail(resultados[0]);
    }
    else
    {
        menu.ShowContactList(resultados);
    }

    menu.Pause();
}

void VerTodos()
{
    List<Contact> all = contactService.GetAll();

    if (all.Count == 0)
    {
        menu.ShowMessage("No hay contactos en la agenda.", ConsoleColor.Yellow);
    }
    else
    {
        menu.ShowContactList(all);
    }

    menu.Pause();
}

void Editar()
{
    string name = menu.GetContactName("Ingrese el nombre del contacto a editar: ");

    if (string.IsNullOrWhiteSpace(name))
    {
        menu.ShowMessage("El nombre no puede estar vac\u00edo.", ConsoleColor.Red);
        menu.Pause();
        return;
    }

    Contact? existing = contactService.FindByName(name);

    if (existing == null)
    {
        menu.ShowMessage("Contacto no encontrado.", ConsoleColor.Red);
        menu.Pause();
        return;
    }

    menu.ShowContactDetail(existing);
    Console.WriteLine();
    Contact updated = menu.GetUpdatedContact(existing);
    string validation = ((ContactService)contactService).ValidateContact(updated);

    if (!string.IsNullOrEmpty(validation))
    {
        menu.ShowMessage(validation, ConsoleColor.Red);
        menu.Pause();
        return;
    }

    if (!name.Equals(updated.Name, StringComparison.OrdinalIgnoreCase)
        && contactService.FindByName(updated.Name) != null)
    {
        menu.ShowMessage("Ya existe otro contacto con ese nombre.", ConsoleColor.Red);
        menu.Pause();
        return;
    }

    contactService.Update(name, updated);
    menu.ShowMessage("Contacto actualizado exitosamente.", ConsoleColor.Green);
    menu.Pause();
}

void Eliminar()
{
    string name = menu.GetContactName("Ingrese el nombre del contacto a eliminar: ");

    if (string.IsNullOrWhiteSpace(name))
    {
        menu.ShowMessage("El nombre no puede estar vac\u00edo.", ConsoleColor.Red);
        menu.Pause();
        return;
    }

    Contact? existing = contactService.FindByName(name);

    if (existing == null)
    {
        menu.ShowMessage("Contacto no encontrado.", ConsoleColor.Red);
        menu.Pause();
        return;
    }

    menu.ShowContactDetail(existing);

    if (!menu.ConfirmAction("\u00bfEliminar este contacto? [S/N]: "))
    {
        menu.ShowMessage("Eliminaci\u00f3n cancelada.", ConsoleColor.Yellow);
        menu.Pause();
        return;
    }

    contactService.Delete(name);
    menu.ShowMessage("Contacto eliminado exitosamente.", ConsoleColor.Green);
    menu.Pause();
}
