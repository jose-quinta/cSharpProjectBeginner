using to_do_list_manager.Services;

namespace to_do_list_manager;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var todoService = new TodoService();
        var menu = new MenuService();

        while (true)
        {
            Console.Clear();
            menu.ShowBanner();
            menu.ShowMainMenu();

            string choice = menu.GetChoice();
            Console.WriteLine(choice);

            switch (choice)
            {
                case "1":
                    ShowTodos(todoService, menu);
                    break;
                case "2":
                    AddTodo(todoService, menu);
                    break;
                case "3":
                    UpdateTodo(todoService, menu);
                    break;
                case "4":
                    ToggleTodo(todoService, menu);
                    break;
                case "5":
                    DeleteTodo(todoService, menu);
                    break;
                case "6":
                    ClearCompleted(todoService, menu);
                    break;
                case "7":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n ¡Hasta luego!");
                    Console.ResetColor();
                    return;
                default:
                    menu.ShowError("Opción no válida.");
                    menu.Pause();
                    continue;
            }
        }
    }

    private static void ShowTodos(TodoService todoService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║            LISTA DE TAREAS           ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var items = todoService.GetAll();
        menu.ShowTodoList(items);
        menu.Pause();
    }

    private static void AddTodo(TodoService todoService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║          AGREGAR TAREA               ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        string title = menu.GetTitle();
        if (string.IsNullOrEmpty(title))
        {
            menu.ShowError("El título no puede estar vacío.");
        }
        else
        {
            todoService.Add(title);
            menu.ShowSuccess("Tarea agregada correctamente.");
        }

        menu.Pause();
    }

    private static void UpdateTodo(TodoService todoService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║           EDITAR TAREA               ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var items = todoService.GetAll();
        menu.ShowTodoList(items);

        Guid id = menu.GetId("ID de la tarea a editar");
        var item = todoService.GetById(id);
        if (item == null)
        {
            menu.ShowError("Tarea no encontrada.");
            menu.Pause();
            return;
        }

        Console.WriteLine($" Editando: {item.Title}");
        string title = menu.GetTitle();
        if (string.IsNullOrEmpty(title))
        {
            menu.ShowError("El título no puede estar vacío.");
        }
        else if (todoService.Update(id, title))
        {
            menu.ShowSuccess("Tarea actualizada correctamente.");
        }

        menu.Pause();
    }

    private static void ToggleTodo(TodoService todoService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║    MARCAR / DESMARCAR COMPLETADA     ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var items = todoService.GetAll();
        menu.ShowTodoList(items);

        Guid id = menu.GetId("ID de la tarea");
        if (todoService.Toggle(id))
        {
            var toggled = todoService.GetById(id);
            string state = toggled?.IsCompleted == true ? "completada" : "pendiente";
            menu.ShowSuccess($"Tarea marcada como {state}.");
        }
        else
        {
            menu.ShowError("Tarea no encontrada.");
        }

        menu.Pause();
    }

    private static void DeleteTodo(TodoService todoService, MenuService menu)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║          ELIMINAR TAREA              ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var items = todoService.GetAll();
        menu.ShowTodoList(items);

        Guid id = menu.GetId("ID de la tarea a eliminar");
        if (todoService.Delete(id))
            menu.ShowSuccess("Tarea eliminada correctamente.");
        else
            menu.ShowError("Tarea no encontrada.");

        menu.Pause();
    }

    private static void ClearCompleted(TodoService todoService, MenuService menu)
    {
        int count = todoService.ClearCompleted();
        if (count > 0)
            menu.ShowSuccess($"Se eliminaron {count} tarea(s) completada(s).");
        else
            menu.ShowSuccess("No hay tareas completadas para limpiar.");

        menu.Pause();
    }
}
