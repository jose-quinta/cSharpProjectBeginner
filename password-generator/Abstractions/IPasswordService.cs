using password_generator.Models;

namespace password_generator.Abstractions;

public interface IPasswordService
{
    PasswordEntry Generate(int length, bool upper, bool lower, bool digits, bool symbols, bool excludeSimilar);
    List<PasswordEntry> GenerateMultiple(int count, int length, bool upper, bool lower, bool digits, bool symbols, bool excludeSimilar);
    string GetStrengthLabel(StrengthLevel level);
    StrengthLevel EvaluateStrength(int length, bool upper, bool lower, bool digits, bool symbols);
    double CalculateEntropy(int length, int charsetSize);
    int GetCharsetSize(bool upper, bool lower, bool digits, bool symbols);
}
