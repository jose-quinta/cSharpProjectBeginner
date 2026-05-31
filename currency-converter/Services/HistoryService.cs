using System.Text.Json;
using currency_converter.Models;

namespace currency_converter.Services;

public class HistoryService
{
    private const int MaxRecords = 20;
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "currency_history.json");

    public void AddRecord(ConversionRecord record)
    {
        List<ConversionRecord> records = Load();
        records.Insert(0, record);
        if (records.Count > MaxRecords)
            records = records.Take(MaxRecords).ToList();
        Save(records);
    }

    public List<ConversionRecord> Load()
    {
        if (!File.Exists(FilePath))
            return new List<ConversionRecord>();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<ConversionRecord>>(json) ?? new List<ConversionRecord>();
    }

    public void Save(List<ConversionRecord> records)
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
