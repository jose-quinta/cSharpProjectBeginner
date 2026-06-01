using morse_code_translator.Models;

namespace morse_code_translator.Abstractions;

public interface IMorseService
{
    string TextToMorse(string text);
    string MorseToText(string morse);
    bool IsValidText(string text);
    bool IsValidMorse(string morse);
    Dictionary<char, string> GetMorseChart();
    void AddToHistory(TranslationRecord record);
    List<TranslationRecord> GetHistory();
    void ClearHistory();
}
