using word_counter.Models;

namespace word_counter.Services;

public class MenuService
{
    private readonly FileService _fileService;

    public MenuService(FileService fileService)
    {
        _fileService = fileService;
    }

    public void ShowTitle()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║        WORD COUNTER v2.0             ║");
        Console.WriteLine(@"║   Análisis completo de texto         ║");
        Console.WriteLine(@"╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public string ShowMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" MENÚ PRINCIPAL");
        Console.ResetColor();
        Console.WriteLine(" ────────────────────────────");
        Console.WriteLine(" [1] Analizar texto manual");
        Console.WriteLine(" [2] Analizar archivo .txt");
        Console.WriteLine(" [3] Ver historial");
        Console.WriteLine(" [4] Modo comparar");
        Console.WriteLine(" [5] Salir");
        Console.WriteLine();
        Console.Write(" Seleccione una opción: ");
        return Console.ReadLine()?.Trim() ?? "";
    }

    public string PromptText()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n Ingrese el texto a analizar (doble Enter para finalizar):");
        Console.ResetColor();

        var lines = new List<string>();
        string? line;
        int emptyCount = 0;

        while ((line = Console.ReadLine()) != null)
        {
            if (string.IsNullOrEmpty(line))
            {
                emptyCount++;
                if (emptyCount >= 2) break;
                lines.Add(line);
            }
            else
            {
                emptyCount = 0;
                lines.Add(line);
            }
        }

        return string.Join(Environment.NewLine, lines);
    }

    public string? PromptFilePath()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n Archivos .txt disponibles:");
        Console.ResetColor();

        List<string> files = _fileService.GetTextFiles(Directory.GetCurrentDirectory());
        if (files.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" (No se encontraron archivos .txt en el directorio actual)");
            Console.ResetColor();
            return null;
        }

        for (int i = 0; i < files.Count; i++)
            Console.WriteLine($" [{i + 1}] {files[i]}");

        Console.WriteLine();
        Console.Write(" Seleccione un archivo (número) o escriba la ruta completa: ");
        string input = Console.ReadLine()?.Trim() ?? "";

        if (int.TryParse(input, out int idx) && idx >= 1 && idx <= files.Count)
            return files[idx - 1];

        return input;
    }

    public bool PromptIgnoreStopWords()
    {
        Console.Write("\n ¿Ignorar stop words (palabras vacías)? (s/n): ");
        return Console.ReadLine()?.Trim().ToLower() == "s";
    }

    public void ShowResult(TextAnalysisResult result)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         RESULTADO DEL ANÁLISIS       ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"  Fuente:              {result.SourceName}");
        Console.WriteLine($"  Analizado:           {result.AnalyzedAt:dd/MM/yyyy HH:mm:ss}");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"  Idioma detectado:    {result.Language}");
        Console.ResetColor();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  ─── ESTADÍSTICAS ───");
        Console.ResetColor();
        Console.WriteLine($"  Palabras:            {result.WordCount:N0}");
        Console.WriteLine($"  Caracteres:          {result.CharacterCount:N0}");
        Console.WriteLine($"  Caracteres (s/sp):   {result.CharacterCountNoSpaces:N0}");
        Console.WriteLine($"  Líneas:              {result.LineCount:N0}");
        Console.WriteLine($"  Oraciones:           {result.SentenceCount:N0}");
        Console.WriteLine($"  Párrafos:            {result.ParagraphCount:N0}");
        Console.WriteLine();
        Console.WriteLine($"  Palabra más larga:   \"{result.LongestWord}\" ({result.LongestWord.Length} letras)");
        Console.WriteLine($"  Palabra más corta:   \"{result.ShortestWord}\" ({result.ShortestWord.Length} letras)");
        Console.WriteLine($"  Promedio:            {result.AverageWordLength} letras/palabra");
        Console.WriteLine($"  Tiempo lectura:      {result.ReadingTimeMinutes} min");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  ─── TOP 10 PALABRAS ───");
        Console.ResetColor();
        for (int i = 0; i < result.TopWords.Count; i++)
        {
            var w = result.TopWords[i];
            double pct = result.WordCount > 0 ? (double)w.Count / result.WordCount * 100 : 0;
            Console.WriteLine($"  {i + 1,2}. \"{w.Word}\" -> {w.Count} veces ({pct:F1}%)");
        }
        Console.WriteLine();
    }

    public bool PromptExport()
    {
        Console.Write(" ¿Exportar resultado a archivo? (s/n): ");
        return Console.ReadLine()?.Trim().ToLower() == "s";
    }

    public void ShowExportSuccess(string fileName)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" Resultado exportado: {fileName}");
        Console.ResetColor();
    }

    public void ShowComparison(TextAnalysisResult a, TextAnalysisResult b)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         MODO COMPARAR               ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        string header1 = $"TEXTO 1: {a.SourceName}";
        string header2 = $"TEXTO 2: {b.SourceName}";
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  {header1,-38} | {header2}");
        Console.ResetColor();

        PrintCompareRow("Palabras", a.WordCount, b.WordCount);
        PrintCompareRow("Caracteres", a.CharacterCount, b.CharacterCount);
        PrintCompareRow("Caract. (s/sp)", a.CharacterCountNoSpaces, b.CharacterCountNoSpaces);
        PrintCompareRow("Líneas", a.LineCount, b.LineCount);
        PrintCompareRow("Oraciones", a.SentenceCount, b.SentenceCount);
        PrintCompareRow("Párrafos", a.ParagraphCount, b.ParagraphCount);
        PrintCompareRow("Promedio", a.AverageWordLength, b.AverageWordLength);
        PrintCompareRow("Lectura (min)", a.ReadingTimeMinutes, b.ReadingTimeMinutes);

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"  Idioma 1: {a.Language}");
        Console.WriteLine($"  Idioma 2: {b.Language}");
        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  ─── PALABRAS ÚNICAS COMPARTIDAS ───");
        Console.ResetColor();
        var words1 = new HashSet<string>(a.TopWords.Select(w => w.Word), StringComparer.OrdinalIgnoreCase);
        var words2 = new HashSet<string>(b.TopWords.Select(w => w.Word), StringComparer.OrdinalIgnoreCase);
        var shared = words1.Intersect(words2, StringComparer.OrdinalIgnoreCase).ToList();

        if (shared.Count > 0)
        {
            Console.WriteLine($"  Palabras en común en top: {string.Join(", ", shared.Take(8))}");
        }
        else
        {
            Console.WriteLine("  (sin palabras compartidas en el top 10)");
        }
        Console.WriteLine();
    }

    private void PrintCompareRow(string label, object val1, object val2)
    {
        Console.WriteLine($"  {label,-20} {val1,12:N} | {val2,12:N}");
    }

    public void ShowHistory(List<TextAnalysisResult> history)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         HISTORIAL DE ANÁLISIS       ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        if (history.Count == 0)
        {
            Console.WriteLine("  (No hay análisis previos)");
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < history.Count; i++)
        {
            var h = history[i];
            Console.WriteLine($"  [{i + 1}] {h.SourceName,-30} {h.AnalyzedAt:dd/MM/yy HH:mm}  {h.WordCount,6} palabras  {h.Language}");
        }
        Console.WriteLine();
    }

    public int? PromptHistoryDetail()
    {
        Console.Write(" Ver detalle (número) o Enter para continuar: ");
        string input = Console.ReadLine()?.Trim() ?? "";
        if (int.TryParse(input, out int idx) && idx >= 1)
            return idx;
        return null;
    }

    public string PromptCompareSource(string label)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n {label}");
        Console.ResetColor();
        Console.WriteLine(" [t] Texto manual");
        Console.WriteLine(" [a] Archivo .txt");
        Console.Write(" Seleccione: ");
        return Console.ReadLine()?.Trim().ToLower() ?? "";
    }

    public bool PromptContinue()
    {
        Console.Write("\n Presione Enter para continuar, o 's' para salir: ");
        return Console.ReadLine()?.Trim().ToLower() != "s";
    }

    public void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" Error: {message}");
        Console.ResetColor();
    }
}
