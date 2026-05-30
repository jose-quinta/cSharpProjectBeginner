using System.Security.Cryptography;
using System.Text;
using password_generator.Abstractions;
using password_generator.Models;

namespace password_generator.Services;

public class PasswordService : IPasswordService
{
    private const string UpperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowerChars = "abcdefghijklmnopqrstuvwxyz";
    private const string DigitChars = "0123456789";
    private const string SymbolChars = "!@#$%^&*()-_=+[]{}|;:,.<>?/";
    private const string SimilarChars = "1lI0O";

    public PasswordEntry Generate(int length, bool upper, bool lower, bool digits, bool symbols, bool excludeSimilar)
    {
        if (length < 4) length = 4;
        if (length > 128) length = 128;

        if (!upper && !lower && !digits && !symbols)
        {
            lower = true;
            digits = true;
        }

        string available = BuildCharset(upper, lower, digits, symbols, excludeSimilar);
        var required = new List<char>();

        if (upper) required.Add(GetRandomChar(FilterChars(UpperChars, excludeSimilar)));
        if (lower) required.Add(GetRandomChar(FilterChars(LowerChars, excludeSimilar)));
        if (digits) required.Add(GetRandomChar(FilterChars(DigitChars, excludeSimilar)));
        if (symbols) required.Add(GetRandomChar(FilterChars(SymbolChars, excludeSimilar)));

        int remaining = length - required.Count;
        var password = new StringBuilder();
        password.Append(string.Join("", required));

        for (int i = 0; i < remaining; i++)
            password.Append(available[GetRandomIndex(available.Length)]);

        string shuffled = Shuffle(password.ToString());
        int charsetSize = GetCharsetSize(upper, lower, digits, symbols);
        double entropy = CalculateEntropy(length, charsetSize);
        StrengthLevel strength = EvaluateStrength(length, upper, lower, digits, symbols);

        return new PasswordEntry
        {
            Password = shuffled,
            Length = length,
            HasUpper = upper,
            HasLower = lower,
            HasDigit = digits,
            HasSymbol = symbols,
            Strength = GetStrengthLabel(strength),
            Entropy = Math.Round(entropy, 1),
            Timestamp = DateTime.Now
        };
    }

    public List<PasswordEntry> GenerateMultiple(int count, int length, bool upper, bool lower, bool digits, bool symbols, bool excludeSimilar)
    {
        var results = new List<PasswordEntry>();
        for (int i = 0; i < count; i++)
            results.Add(Generate(length, upper, lower, digits, symbols, excludeSimilar));
        return results;
    }

    public string GetStrengthLabel(StrengthLevel level)
    {
        return level switch
        {
            StrengthLevel.Weak => "D\u00e9bil",
            StrengthLevel.Medium => "Media",
            StrengthLevel.Strong => "Fuerte",
            StrengthLevel.VeryStrong => "Muy fuerte",
            _ => "Desconocida"
        };
    }

    public StrengthLevel EvaluateStrength(int length, bool upper, bool lower, bool digits, bool symbols)
    {
        int types = (upper ? 1 : 0) + (lower ? 1 : 0) + (digits ? 1 : 0) + (symbols ? 1 : 0);

        if (length >= 16 && types >= 3 && symbols) return StrengthLevel.VeryStrong;
        if (length >= 12 && types >= 2) return StrengthLevel.Strong;
        if (length >= 8 && types >= 1) return StrengthLevel.Medium;
        return StrengthLevel.Weak;
    }

    public double CalculateEntropy(int length, int charsetSize)
    {
        if (charsetSize <= 0 || length <= 0) return 0;
        return length * Math.Log2(charsetSize);
    }

    public int GetCharsetSize(bool upper, bool lower, bool digits, bool symbols)
    {
        int size = 0;
        if (upper) size += FilterChars(UpperChars, false).Length;
        if (lower) size += FilterChars(LowerChars, false).Length;
        if (digits) size += FilterChars(DigitChars, false).Length;
        if (symbols) size += FilterChars(SymbolChars, false).Length;
        return size;
    }

    private string BuildCharset(bool upper, bool lower, bool digits, bool symbols, bool excludeSimilar)
    {
        var sb = new StringBuilder();
        if (upper) sb.Append(FilterChars(UpperChars, excludeSimilar));
        if (lower) sb.Append(FilterChars(LowerChars, excludeSimilar));
        if (digits) sb.Append(FilterChars(DigitChars, excludeSimilar));
        if (symbols) sb.Append(FilterChars(SymbolChars, excludeSimilar));
        return sb.ToString();
    }

    private string FilterChars(string chars, bool excludeSimilar)
    {
        if (!excludeSimilar) return chars;
        return new string(chars.Where(c => !SimilarChars.Contains(c)).ToArray());
    }

    private static char GetRandomChar(string chars)
    {
        return chars[GetRandomIndex(chars.Length)];
    }

    private static int GetRandomIndex(int max)
    {
        return RandomNumberGenerator.GetInt32(max);
    }

    private static string Shuffle(string input)
    {
        char[] array = input.ToCharArray();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = RandomNumberGenerator.GetInt32(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
        return new string(array);
    }
}
