using System.Text.Json;
using text_based_rpg.Models;

namespace text_based_rpg.Services;

public class SaveService
{
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "save.json");

    public void Save(Player player)
    {
        string json = JsonSerializer.Serialize(player, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public Player? Load()
    {
        if (!File.Exists(FilePath))
            return null;

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<Player>(json);
    }

    public bool SaveExists()
    {
        return File.Exists(FilePath);
    }

    public void Delete()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}
