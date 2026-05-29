using bank_account_system.Models;

namespace bank_account_system.Abstractions;

public interface IAccountService
{
    Account Create(string holder, AccountType type);
    Account? FindByNumber(string number);
    List<Account> Search(string query);
    List<Account> GetAll();
    bool Close(string number);
    List<Account> Load();
    void Save(List<Account> accounts);
}
