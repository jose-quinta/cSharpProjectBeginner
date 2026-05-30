using System.Text.Json;
using password_generator.Models;

namespace password_generator.Services;

public class HistoryService
{
    private const int MaxRecords = 20;
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "password_history.json");

    public void AddRecord(PasswordEntry entry)
    {
        List<PasswordEntry> records = Load();
        records.Insert(0, entry);
        if (records.Count > MaxRecords)
            records = records.Take(MaxRecords).ToList();
        Save(records);
    }

    public void AddRange(List<PasswordEntry> entries)
    {
        List<PasswordEntry> records = Load();
        records.InsertRange(0, entries);
        if (records.Count > MaxRecords)
            records = records.Take(MaxRecords).ToList();
        Save(records);
    }

    public List<PasswordEntry> Load()
    {
        if (!File.Exists(FilePath))
            return new List<PasswordEntry>();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<PasswordEntry>>(json) ?? new List<PasswordEntry>();
    }

    public void Save(List<PasswordEntry> records)
    {
        string json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public void Clear()
    {
        if (File.Exists(FilePath))
            File.Delete(FilePath);
    }
}
