using guess_number.Abstractions;

namespace guess_number.Services;

public class GameService : IGameService
{
    private static readonly Random _random = new();

    private int _secretNumber;
    private int _attempts;
    private int _rangeMin;
    private int _rangeMax;
    private int _maxAttempts;
    private bool _isGameOver;
    private readonly List<int> _guesses = new();

    public int SecretNumber => _secretNumber;
    public int Attempts => _attempts;
    public int RangeMin => _rangeMin;
    public int RangeMax => _rangeMax;
    public int MaxAttempts => _maxAttempts;
    public bool IsGameOver => _isGameOver;
    public IReadOnlyList<int> Guesses => _guesses.AsReadOnly();

    public void StartNewGame(int rangeMin, int rangeMax, int maxAttempts)
    {
        _secretNumber = _random.Next(rangeMin, rangeMax + 1);
        _attempts = 0;
        _rangeMin = rangeMin;
        _rangeMax = rangeMax;
        _maxAttempts = maxAttempts;
        _isGameOver = false;
        _guesses.Clear();
    }

    public GuessResult Guess(int number)
    {
        if (_isGameOver)
            return GuessResult.GameOver;

        _attempts++;
        _guesses.Add(number);

        if (number == _secretNumber)
        {
            _isGameOver = true;
            return GuessResult.Correct;
        }

        if (_attempts >= _maxAttempts)
        {
            _isGameOver = true;
            return GuessResult.GameOver;
        }

        return number < _secretNumber ? GuessResult.Lower : GuessResult.Higher;
    }

    public string GetTemperatureHint(int guess)
    {
        int diff = Math.Abs(guess - _secretNumber);
        int range = _rangeMax - _rangeMin;
        double ratio = range > 0 ? (double)diff / range : 1;

        if (ratio > 0.50) return "Congelado";
        if (ratio > 0.30) return "Frio";
        if (ratio > 0.15) return "Tibio";
        if (ratio > 0.05) return "Caliente";
        return "Ardiendo";
    }

    public ConsoleColor GetHintColor(int guess)
    {
        int diff = Math.Abs(guess - _secretNumber);
        int range = _rangeMax - _rangeMin;
        double ratio = range > 0 ? (double)diff / range : 1;

        if (ratio > 0.50) return ConsoleColor.DarkBlue;
        if (ratio > 0.30) return ConsoleColor.Cyan;
        if (ratio > 0.15) return ConsoleColor.Yellow;
        if (ratio > 0.05) return ConsoleColor.Magenta;
        return ConsoleColor.Red;
    }
}
