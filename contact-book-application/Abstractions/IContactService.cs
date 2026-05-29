using contact_book_application.Models;

namespace contact_book_application.Abstractions;

public interface IContactService
{
    void Add(Contact contact);
    Contact? FindByName(string name);
    List<Contact> Search(string query);
    List<Contact> GetAll();
    bool Update(string name, Contact updated);
    bool Delete(string name);
    List<Contact> Load();
    void Save(List<Contact> contacts);
}
