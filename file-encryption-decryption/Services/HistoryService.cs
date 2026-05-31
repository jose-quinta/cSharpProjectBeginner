using System.Text.Json;
using file_encryption_decryption.Models;

namespace file_encryption_decryption.Services;

public class HistoryService
{
    private const int MaxRecords = 20;
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "encryption_history.json");

    public void AddRecord(FileOperation record)
    {
        List<FileOperation> records = Load();
        records.Insert(0, record);
        if (records.Count > MaxRecords)
            records = records.Take(MaxRecords).ToList();
        Save(records);
    }

    public List<FileOperation> Load()
    {
        if (!File.Exists(FilePath))
            return new List<FileOperation>();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<FileOperation>>(json) ?? new List<FileOperation>();
    }

    public void Save(List<FileOperation> records)
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
