using System.Text.Json;
using palindrome_checker.Abstractions;
using palindrome_checker.Models;
using palindrome_checker.Services;

IPalindromeService service = new PalindromeService();
HistoryService history = new HistoryService();
MenuService menu = new MenuService();

bool salir = false;
while (!salir)
{
    Console.Clear();
    menu.ShowBanner();
    menu.ShowMenu();
    string opcion = menu.GetChoice();
    Console.WriteLine();

    if (string.IsNullOrEmpty(opcion))
    {
        menu.ShowError("Opci\u00f3n inv\u00e1lida.");
        menu.Pause();
        continue;
    }

    if (opcion == "8")
    {
        if (menu.ConfirmExit())
            salir = true;
        continue;
    }

    try
    {
        switch (opcion)
        {
            case "1":
            {
                string text = menu.GetText("Ingrese la palabra o frase a verificar:");
                if (string.IsNullOrEmpty(text))
                {
                    menu.ShowError("No ingres\u00f3 ning\u00fan texto.");
                    break;
                }
                string category = text.Contains(' ') ? "Frase" : "Palabra";
                AnalysisResult result = service.Check(text, category);
                menu.ShowResult(result);
                history.AddRecord(result);
                break;
            }
            case "2":
            {
                long number = menu.GetNumber();
                AnalysisResult result = service.Check(number.ToString(), "N\u00famero");
                menu.ShowResult(result);
                history.AddRecord(result);
                break;
            }
            case "3":
            {
                string text = menu.GetText("Ingrese el texto para buscar pal\u00edndromos:");
                if (string.IsNullOrEmpty(text))
                {
                    menu.ShowError("No ingres\u00f3 ning\u00fan texto.");
                    break;
                }
                AnalysisResult result = service.CheckAll(text);
                menu.ShowPalindromesFound(result);
                break;
            }
            case "4":
            {
                string text = menu.GetText("Ingrese el texto a revertir:");
                if (string.IsNullOrEmpty(text))
                {
                    menu.ShowError("No ingres\u00f3 ning\u00fan texto.");
                    break;
                }
                string reversed = service.GetReversed(text);
                menu.ShowReversed(text, reversed);
                break;
            }
            case "5":
            {
                List<AnalysisResult> records = history.Load();
                menu.ShowHistory(records);
                if (records.Count > 0)
                {
                    Console.Write(" Ver detalle (n\u00famero) o Enter para continuar: ");
                    string? input = Console.ReadLine()?.Trim();
                    if (int.TryParse(input, out int idx) && idx >= 1 && idx <= records.Count)
                    {
                        menu.ShowDetail(records[idx - 1]);
                    }
                }
                break;
            }
            case "6":
            {
                List<AnalysisResult> records = history.Load();
                PalindromeStats stats = service.GetStats(records);
                menu.ShowStats(stats);
                break;
            }
            case "7":
            {
                List<AnalysisResult> records = history.Load();
                if (records.Count == 0)
                {
                    menu.ShowError("No hay historial para exportar.");
                    break;
                }
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = $"palindrome_export_{timestamp}.txt";
                string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
                var lines = new List<string>
                {
                    "========================================",
                    "  PALINDROME CHECKER - HISTORIAL",
                    "========================================",
                    $"  Exportado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}",
                    $"  Total registros: {records.Count}",
                    "========================================",
                    ""
                };
                for (int i = 0; i < records.Count; i++)
                {
                    var r = records[i];
                    lines.Add($"  [{i + 1}] {r.Timestamp:dd/MM HH:mm} | {(r.IsPalindrome ? "SI" : "NO")} | {r.Category} | \"{r.InputText}\"");
                }
                lines.Add("");
                lines.Add("========================================");
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName), string.Join(Environment.NewLine, lines));
                menu.ShowExportSuccess(fileName);
                break;
            }
        }

        if (opcion != "8")
            menu.Pause();
    }
    catch (Exception ex)
    {
        menu.ShowError($"Error inesperado: {ex.Message}");
        menu.Pause();
    }
}
