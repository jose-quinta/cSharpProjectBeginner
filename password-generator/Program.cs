using password_generator.Abstractions;
using password_generator.Models;
using password_generator.Services;

IPasswordService service = new PasswordService();
HistoryService history = new HistoryService();
MenuService menu = new MenuService();
Random rng = new();

bool salir = false;
while (!salir)
{
    Console.Clear();
    menu.ShowBanner();
    menu.ShowMainMenu();
    string opcion = menu.GetChoice();
    Console.WriteLine();

    if (string.IsNullOrEmpty(opcion))
    {
        menu.ShowError("Opci\u00f3n inv\u00e1lida.");
        menu.Pause();
        continue;
    }

    if (opcion == "7")
    {
        if (menu.ConfirmAction("\u00bfSalir"))
            salir = true;
        continue;
    }

    try
    {
        switch (opcion)
        {
            case "1":
            {
                PasswordEntry entry = service.Generate(
                    menu.ConfigLength,
                    menu.ConfigUpper,
                    menu.ConfigLower,
                    menu.ConfigDigits,
                    menu.ConfigSymbols,
                    menu.ConfigExcludeSimilar
                );
                menu.ShowPassword(entry);
                history.AddRecord(entry);
                break;
            }
            case "2":
            {
                int count = menu.GetMultipleCount();
                List<PasswordEntry> entries = service.GenerateMultiple(
                    count,
                    menu.ConfigLength,
                    menu.ConfigUpper,
                    menu.ConfigLower,
                    menu.ConfigDigits,
                    menu.ConfigSymbols,
                    menu.ConfigExcludeSimilar
                );
                menu.ShowPasswords(entries);
                history.AddRange(entries);
                break;
            }
            case "3":
            {
                bool configMode = true;
                while (configMode)
                {
                    Console.Clear();
                    menu.ShowBanner();
                    menu.ShowConfig();
                    menu.ShowConfigMenu();
                    string cfgChoice = menu.GetConfigChoice();
                    Console.WriteLine();

                    switch (cfgChoice)
                    {
                        case "1":
                            menu.ConfigLength = menu.GetLength();
                            menu.ShowConfigUpdated($"Longitud cambiada a {menu.ConfigLength}.");
                            break;
                        case "2":
                            menu.ConfigUpper = !menu.ConfigUpper;
                            menu.ShowConfigUpdated($"May\u00fasculas: {(menu.ConfigUpper ? "activadas" : "desactivadas")}.");
                            break;
                        case "3":
                            menu.ConfigLower = !menu.ConfigLower;
                            menu.ShowConfigUpdated($"Min\u00fasculas: {(menu.ConfigLower ? "activadas" : "desactivadas")}.");
                            break;
                        case "4":
                            menu.ConfigDigits = !menu.ConfigDigits;
                            menu.ShowConfigUpdated($"D\u00edgitos: {(menu.ConfigDigits ? "activados" : "desactivados")}.");
                            break;
                        case "5":
                            menu.ConfigSymbols = !menu.ConfigSymbols;
                            menu.ShowConfigUpdated($"S\u00edmbolos: {(menu.ConfigSymbols ? "activados" : "desactivados")}.");
                            break;
                        case "6":
                            menu.ConfigExcludeSimilar = !menu.ConfigExcludeSimilar;
                            menu.ShowConfigUpdated($"Excluir similares: {(menu.ConfigExcludeSimilar ? "activado" : "desactivado")}.");
                            break;
                        case "7":
                            configMode = false;
                            continue;
                    }
                    if (configMode)
                        menu.Pause();
                }
                break;
            }
            case "4":
            {
                List<PasswordEntry> records = history.Load();
                menu.ShowHistory(records);
                break;
            }
            case "5":
            {
                List<PasswordEntry> records = history.Load();
                if (records.Count == 0)
                {
                    menu.ShowError("No hay historial para exportar.");
                    break;
                }

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = $"passwords_export_{timestamp}.txt";
                var lines = new List<string>
                {
                    "========================================",
                    "  PASSWORD GENERATOR - EXPORTACION",
                    "========================================",
                    $"  Exportado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
                    $"  Total: {records.Count} contrase\u00f1as",
                    "========================================",
                    ""
                };
                for (int i = 0; i < records.Count; i++)
                {
                    var r = records[i];
                    lines.Add($"  [{i + 1}] {r.Password}  [{r.Strength}]  ({r.Length} chars, {r.Entropy} bits)");
                }
                lines.Add("");
                lines.Add("========================================");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), string.Join(Environment.NewLine, lines));
                menu.ShowExportSuccess(fileName);
                break;
            }
            case "6":
            {
                if (menu.ConfirmAction("\u00bfLimpiar todo el historial"))
                {
                    history.Clear();
                    menu.ShowClearHistory();
                }
                break;
            }
        }

        if (opcion != "7")
            menu.Pause();
    }
    catch (Exception ex)
    {
        menu.ShowError($"Error inesperado: {ex.Message}");
        menu.Pause();
    }
}
