using System.Diagnostics;
using guess_number.Abstractions;
using guess_number.Models;
using guess_number.Services;

GameService game = new GameService();
MenuService menu = new MenuService();
HighScoreService scoreService = new HighScoreService();
StatsService statsService = new StatsService();

List<HighScoreRecord> scores = scoreService.Load();
GameStats stats = statsService.Load();
bool jugar = true;

while (jugar)
{
    var (min, max, maxAttempts, levelName) = menu.ShowDifficultyMenu();
    game.StartNewGame(min, max, maxAttempts);

    Stopwatch stopwatch = Stopwatch.StartNew();
    bool isNewRecord = false;

    while (!game.IsGameOver)
    {
        int guess = menu.GetGuess(game.Guesses);
        GuessResult result = game.Guess(guess);

        if (result == GuessResult.Correct)
        {
            stopwatch.Stop();
            menu.ShowWin(game.Attempts, game.SecretNumber, stopwatch.Elapsed, game.MaxAttempts);

            if (scoreService.IsHighScore(game.Attempts, scores))
            {
                isNewRecord = true;
                string name = menu.GetPlayerName();
                scores = scoreService.AddHighScore(name, game.Attempts, game.SecretNumber, scores);
                scoreService.Save(scores);
            }

            statsService.RecordGame(game.Attempts, isNewRecord ? scores[0].PlayerName : "", ref stats);
            break;
        }

        if (result == GuessResult.GameOver)
        {
            stopwatch.Stop();
            menu.ShowHint(result, "", ConsoleColor.Red, 0, game.SecretNumber, game.Attempts, game.MaxAttempts);
            statsService.RecordGame(game.Attempts, "", ref stats);
            break;
        }

        string temp = game.GetTemperatureHint(guess);
        ConsoleColor color = game.GetHintColor(guess);
        menu.ShowHint(result, temp, color, guess, game.SecretNumber, game.Attempts, game.MaxAttempts);
    }

    menu.ShowHighScores(scores, isNewRecord);

    if (menu.AskShowStats())
    {
        double avg = stats.TotalGames > 0 ? (double)stats.TotalAttempts / stats.TotalGames : 0;
        menu.ShowStats(stats.TotalGames, stats.TotalAttempts, avg, stats.BestScore, stats.BestPlayer);
    }

    if (menu.AskClearScores())
    {
        scoreService.Clear();
        scores = new List<HighScoreRecord>();
    }

    jugar = menu.AskPlayAgain();
}

statsService.Save(stats);
Console.WriteLine("\u00a1Hasta luego!");
