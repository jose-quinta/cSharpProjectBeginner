using to_do_list_manager.Models;

namespace to_do_list_manager.Abstractions;

public interface ITodoService
{
    void Add(string title);
    List<TodoItem> GetAll();
    TodoItem? GetById(Guid id);
    bool Update(Guid id, string title);
    bool Toggle(Guid id);
    bool Delete(Guid id);
    int ClearCompleted();
    void Save();
    void Load();
}
