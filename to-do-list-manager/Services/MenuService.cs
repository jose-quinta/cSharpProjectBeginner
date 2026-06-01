using to_do_list_manager.Models;

namespace to_do_list_manager.Services;

public class MenuService
{
    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║         TO-DO LIST MANAGER           ║");
        Console.WriteLine(@"╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" MENÚ PRINCIPAL");
        Console.ResetColor();
        Console.WriteLine(" ────────────────────────────");
        Console.WriteLine(" [1] Ver tareas");
        Console.WriteLine(" [2] Agregar tarea");
        Console.WriteLine(" [3] Editar tarea");
        Console.WriteLine(" [4] Marcar/Desmarcar completada");
        Console.WriteLine(" [5] Eliminar tarea");
        Console.WriteLine(" [6] Limpiar completadas");
        Console.WriteLine(" [7] Salir");
        Console.WriteLine();
        Console.Write(" Seleccione una opción: ");
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
            "D7" or "NumPad7" => "7",
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "v" => "1",
                "a" => "2",
                "e" => "3",
                "m" => "4",
                "d" => "5",
                "l" => "6",
                "s" => "7",
                _ => ""
            }
        };
    }

    public void ShowTodoList(List<TodoItem> items)
    {
        if (items.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" (No hay tareas)");
            Console.ResetColor();
            Console.WriteLine();
            return;
        }

        int pending = items.Count(i => !i.IsCompleted);
        int completed = items.Count(i => i.IsCompleted);

        Console.WriteLine($" Total: {items.Count}  |  Pendientes: {pending}  |  Completadas: {completed}");
        Console.WriteLine();

        for (int i = 0; i < items.Count; i++)
        {
            var t = items[i];
            string status = t.IsCompleted ? "✓" : " ";
            Console.ForegroundColor = t.IsCompleted ? ConsoleColor.Green : ConsoleColor.Yellow;
            Console.WriteLine($" [{status}] {t.Title}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"      └ ID: {t.Id}");
            if (t.IsCompleted && t.CompletedAt.HasValue)
                Console.WriteLine($"      └ Completada: {t.CompletedAt:dd/MM/yy HH:mm}");
            else
                Console.WriteLine($"      └ Creada: {t.CreatedAt:dd/MM/yy HH:mm}");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    public string GetTitle()
    {
        Console.Write(" Título: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public Guid GetId(string prompt)
    {
        while (true)
        {
            Console.Write($" {prompt}: ");
            string? input = Console.ReadLine()?.Trim();
            if (Guid.TryParse(input, out Guid id))
                return id;
            ShowError("ID inválido. Ingrese un GUID válido.");
        }
    }

    public void ShowSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" {message}");
        Console.ResetColor();
    }

    public void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" Error: {message}");
        Console.ResetColor();
    }

    public void Pause()
    {
        Console.Write("\n Presione cualquier tecla para continuar...");
        Console.ReadKey(true);
        Console.WriteLine();
    }
}
