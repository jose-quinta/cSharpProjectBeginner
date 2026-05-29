using System.Text.Json;
using System.Text.RegularExpressions;
using contact_book_application.Abstractions;
using contact_book_application.Models;

namespace contact_book_application.Services;

public class ContactService : IContactService
{
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "contacts.json");

    private static readonly Regex PhoneRegex = new(@"^[\d\s\-\(\)\+]{7,20}$");
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public string ValidateContact(Contact contact)
    {
        if (string.IsNullOrWhiteSpace(contact.Name))
            return "El nombre no puede estar vac\u00edo.";

        if (string.IsNullOrWhiteSpace(contact.Phone))
            return "El tel\u00e9fono no puede estar vac\u00edo.";

        if (!PhoneRegex.IsMatch(contact.Phone.Trim()))
            return "Tel\u00e9fono inv\u00e1lido. Use solo d\u00edgitos, guiones, par\u00e9ntesis o espacios (7-20 caracteres).";

        if (!string.IsNullOrWhiteSpace(contact.Email) && !EmailRegex.IsMatch(contact.Email.Trim()))
            return "Email inv\u00e1lido. Debe contener @ y un dominio v\u00e1lido.";

        return string.Empty;
    }

    public List<Contact> Load()
    {
        if (!File.Exists(FilePath))
            return new List<Contact>();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
    }

    public void Save(List<Contact> contacts)
    {
        string json = JsonSerializer.Serialize(contacts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public void Add(Contact contact)
    {
        List<Contact> contacts = Load();
        contacts.Add(contact);
        contacts = contacts.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase).ToList();
        Save(contacts);
    }

    public Contact? FindByName(string name)
    {
        List<Contact> contacts = Load();
        return contacts.FirstOrDefault(c =>
            c.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public List<Contact> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Load();

        List<Contact> contacts = Load();
        string q = query.Trim();
        return contacts.Where(c =>
            c.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            c.Phone.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            (c.Email?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)
        ).OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase).ToList();
    }

    public List<Contact> GetAll()
    {
        List<Contact> contacts = Load();
        return contacts.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase).ToList();
    }

    public bool Update(string name, Contact updated)
    {
        List<Contact> contacts = Load();
        Contact? existing = contacts.FirstOrDefault(c =>
            c.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        if (existing == null)
            return false;

        existing.Name = updated.Name;
        existing.Phone = updated.Phone;
        existing.Email = updated.Email;

        contacts = contacts.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase).ToList();
        Save(contacts);
        return true;
    }

    public bool Delete(string name)
    {
        List<Contact> contacts = Load();
        int removed = contacts.RemoveAll(c =>
            c.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));

        if (removed > 0)
        {
            Save(contacts);
            return true;
        }

        return false;
    }
}
