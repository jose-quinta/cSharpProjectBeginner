using word_counter.Models;
using word_counter.Services;

static TextAnalysisResult? AnalyzeText(WordCounterService counter, FileService fileService, MenuService menu, string source, string text)
{
    if (string.IsNullOrWhiteSpace(text))
    {
        menu.ShowError("No se ingres\u00f3 texto.");
        return null;
    }

    bool ignoreStop = menu.PromptIgnoreStopWords();
    TextAnalysisResult result = counter.Analyze(text, source, ignoreStop);
    menu.ShowResult(result);

    fileService.SaveToHistory(result);

    if (menu.PromptExport())
    {
        fileService.ExportResult(result);
        menu.ShowExportSuccess($"analysis_{source}_{result.AnalyzedAt:yyyyMMdd_HHmmss}.txt");
    }

    return result;
}

static TextAnalysisResult? GetTextForCompare(WordCounterService counter, FileService fileService, MenuService menu, string label)
{
    string choice = menu.PromptCompareSource(label);
    if (choice == "a")
    {
        string? path = menu.PromptFilePath();
        if (path == null) return null;
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
        string? content = fileService.ReadTextFile(fullPath);
        if (content == null)
        {
            menu.ShowError("No se pudo leer el archivo.");
            return null;
        }
        return counter.Analyze(content, path, false);
    }
    else
    {
        string text = menu.PromptText();
        if (string.IsNullOrWhiteSpace(text))
        {
            menu.ShowError("No se ingres\u00f3 texto.");
            return null;
        }
        return counter.Analyze(text, "Texto manual", false);
    }
}

try
{
    var counter = new WordCounterService();
    var fileService = new FileService();
    var menu = new MenuService(fileService);

    menu.ShowTitle();

    bool running = true;
    while (running)
    {
        string option = menu.ShowMainMenu();
        Console.WriteLine();

        switch (option)
        {
            case "1":
            {
                string text = menu.PromptText();
                AnalyzeText(counter, fileService, menu, "Texto manual", text);
                break;
            }

            case "2":
            {
                string? fileName = menu.PromptFilePath();
                if (fileName == null) break;

                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                string? content = fileService.ReadTextFile(fullPath);
                if (content == null)
                {
                    menu.ShowError("No se pudo leer el archivo.");
                    break;
                }

                AnalyzeText(counter, fileService, menu, fileName, content);
                break;
            }

            case "3":
            {
                List<TextAnalysisResult> history = fileService.LoadHistory();
                menu.ShowHistory(history);
                int? detailIdx = menu.PromptHistoryDetail();
                if (detailIdx.HasValue && detailIdx.Value >= 1 && detailIdx.Value <= history.Count)
                {
                    menu.ShowResult(history[detailIdx.Value - 1]);
                }
                break;
            }

            case "4":
            {
                TextAnalysisResult? t1 = GetTextForCompare(counter, fileService, menu, "TEXTO 1");
                if (t1 == null) break;
                TextAnalysisResult? t2 = GetTextForCompare(counter, fileService, menu, "TEXTO 2");
                if (t2 == null) break;
                menu.ShowComparison(t1, t2);
                break;
            }

            case "5":
                running = false;
                continue;
        }

        if (running && !menu.PromptContinue())
            running = false;
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\n Error inesperado: {ex.Message}");
    Console.ResetColor();
}
