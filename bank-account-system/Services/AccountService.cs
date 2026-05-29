using System.Text.Json;
using bank_account_system.Abstractions;
using bank_account_system.Models;

namespace bank_account_system.Services;

public class AccountService : IAccountService
{
    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "accounts.json");

    public Account Create(string holder, AccountType type)
    {
        Account account = new Account
        {
            Number = $"ACC-{Guid.NewGuid().ToString("N")[..8].ToUpper()}",
            HolderName = holder.Trim(),
            Type = type,
            Balance = 0,
            CreatedAt = DateTime.Now,
            IsActive = true
        };

        List<Account> accounts = Load();
        accounts.Add(account);
        Save(accounts);
        return account;
    }

    public Account? FindByNumber(string number)
    {
        List<Account> accounts = Load();
        return accounts.FirstOrDefault(a =>
            a.Number.Equals(number.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public List<Account> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAll();

        string q = query.Trim();
        List<Account> accounts = Load();
        return accounts.Where(a =>
            a.Number.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            a.HolderName.Contains(q, StringComparison.OrdinalIgnoreCase)
        ).OrderBy(a => a.HolderName).ToList();
    }

    public List<Account> GetAll()
    {
        List<Account> accounts = Load();
        return accounts.OrderBy(a => a.HolderName).ToList();
    }

    public bool Close(string number)
    {
        List<Account> accounts = Load();
        Account? account = accounts.FirstOrDefault(a =>
            a.Number.Equals(number.Trim(), StringComparison.OrdinalIgnoreCase));

        if (account == null || !account.IsActive)
            return false;

        account.IsActive = false;
        Save(accounts);
        return true;
    }

    public List<Account> Load()
    {
        if (!File.Exists(FilePath))
            return new List<Account>();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Account>>(json) ?? new List<Account>();
    }

    public void Save(List<Account> accounts)
    {
        string json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
