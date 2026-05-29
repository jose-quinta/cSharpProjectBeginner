using guess_number.Models;

namespace guess_number.Abstractions;

public interface IGameService
{
    int SecretNumber { get; }
    int Attempts { get; }
    int RangeMin { get; }
    int RangeMax { get; }
    int MaxAttempts { get; }
    bool IsGameOver { get; }
    IReadOnlyList<int> Guesses { get; }

    void StartNewGame(int rangeMin, int rangeMax, int maxAttempts);
    GuessResult Guess(int number);
    string GetTemperatureHint(int guess);
    ConsoleColor GetHintColor(int guess);
}
