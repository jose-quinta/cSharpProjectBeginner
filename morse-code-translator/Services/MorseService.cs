using System.Text;
using System.Text.RegularExpressions;
using morse_code_translator.Abstractions;
using morse_code_translator.Models;

namespace morse_code_translator.Services;

public class MorseService : IMorseService
{
    private static readonly Dictionary<char, string> TextToMorseMap = new()
    {
        {'A', ".-"}, {'B', "-..."}, {'C', "-.-."}, {'D', "-.."},
        {'E', "."}, {'F', "..-."}, {'G', "--."}, {'H', "...."},
        {'I', ".."}, {'J', ".---"}, {'K', "-.-"}, {'L', ".-.."},
        {'M', "--"}, {'N', "-."}, {'O', "---"}, {'P', ".--."},
        {'Q', "--.-"}, {'R', ".-."}, {'S', "..."}, {'T', "-"},
        {'U', "..-"}, {'V', "...-"}, {'W', ".--"}, {'X', "-..-"},
        {'Y', "-.--"}, {'Z', "--.."},
        {'0', "-----"}, {'1', ".----"}, {'2', "..---"}, {'3', "...--"},
        {'4', "....-"}, {'5', "....."}, {'6', "-...."}, {'7', "--..."},
        {'8', "---.."}, {'9', "----."},
        {'.', ".-.-.-"}, {',', "--..--"}, {'?', "..--.."}, {'!', "-.-.--"},
        {':', "---..."}, {';', "-.-.-."}, {'-', "-....-"}, {'/', "-..-."},
        {'(', "-.--."}, {')', "-.--.-"}, {'\"', ".-..-."}, {'\'', ".----."},
        {'@', ".--.-."}, {'=', "-...-"}, {'+', ".-.-."}, {' ', "/"}
    };

    private static readonly Dictionary<string, char> MorseToTextMap = TextToMorseMap
        .ToDictionary(kv => kv.Value, kv => kv.Key);

    private static readonly HashSet<char> ValidChars = new(TextToMorseMap.Keys);

    private readonly List<TranslationRecord> _history = new();

    public string TextToMorse(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        text = text.ToUpperInvariant();
        var result = new StringBuilder();

        foreach (char c in text)
        {
            if (TextToMorseMap.TryGetValue(c, out string? morse))
            {
                if (result.Length > 0 && c != ' ')
                    result.Append(' ');
                result.Append(morse);
            }
            else
            {
                result.Append('?');
            }
        }

        return result.ToString().Trim();
    }

    public string MorseToText(string morse)
    {
        if (string.IsNullOrWhiteSpace(morse))
            return string.Empty;

        var result = new StringBuilder();

        string[] words = morse.Split(new[] { " / " }, StringSplitOptions.None);

        for (int w = 0; w < words.Length; w++)
        {
            if (w > 0)
                result.Append(' ');

            string[] letters = words[w].Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int l = 0; l < letters.Length; l++)
            {
                if (MorseToTextMap.TryGetValue(letters[l], out char ch))
                    result.Append(ch);
                else
                    result.Append('?');
            }
        }

        return result.ToString();
    }

    public bool IsValidText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        return text.ToUpperInvariant().All(c => ValidChars.Contains(c));
    }

    public bool IsValidMorse(string morse)
    {
        if (string.IsNullOrWhiteSpace(morse))
            return false;

        return Regex.IsMatch(morse, @"^[.\- /]+$");
    }

    public Dictionary<char, string> GetMorseChart()
    {
        return new Dictionary<char, string>(TextToMorseMap);
    }

    public void AddToHistory(TranslationRecord record)
    {
        _history.Add(record);
    }

    public List<TranslationRecord> GetHistory()
    {
        return new List<TranslationRecord>(_history);
    }

    public void ClearHistory()
    {
        _history.Clear();
    }
}
