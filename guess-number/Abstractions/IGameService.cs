namespace guess_number.Abstractions;

public enum GuessResult
{
    Lower,
    Higher,
    Correct,
    GameOver
}

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
