using System.Text.Json;
using to_do_list_manager.Abstractions;
using to_do_list_manager.Models;

namespace to_do_list_manager.Services;

public class TodoService : ITodoService
{
    private readonly List<TodoItem> _items = new();
    private static readonly string DataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
    private static readonly string FilePath = Path.Combine(DataDir, "todos.json");

    public TodoService()
    {
        Load();
    }

    public void Add(string title)
    {
        _items.Add(new TodoItem { Title = title });
        Save();
    }

    public List<TodoItem> GetAll()
    {
        return _items.OrderBy(i => i.IsCompleted).ThenByDescending(i => i.CreatedAt).ToList();
    }

    public TodoItem? GetById(Guid id)
    {
        return _items.FirstOrDefault(i => i.Id == id);
    }

    public bool Update(Guid id, string title)
    {
        TodoItem? item = GetById(id);
        if (item == null) return false;
        item.Title = title;
        Save();
        return true;
    }

    public bool Toggle(Guid id)
    {
        TodoItem? item = GetById(id);
        if (item == null) return false;
        item.IsCompleted = !item.IsCompleted;
        item.CompletedAt = item.IsCompleted ? DateTime.Now : null;
        Save();
        return true;
    }

    public bool Delete(Guid id)
    {
        TodoItem? item = GetById(id);
        if (item == null) return false;
        _items.Remove(item);
        Save();
        return true;
    }

    public int ClearCompleted()
    {
        int count = _items.RemoveAll(i => i.IsCompleted);
        if (count > 0) Save();
        return count;
    }

    public void Save()
    {
        if (!Directory.Exists(DataDir))
            Directory.CreateDirectory(DataDir);

        string json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public void Load()
    {
        if (!File.Exists(FilePath)) return;

        string json = File.ReadAllText(FilePath);
        var items = JsonSerializer.Deserialize<List<TodoItem>>(json);
        if (items != null)
        {
            _items.Clear();
            _items.AddRange(items);
        }
    }
}
