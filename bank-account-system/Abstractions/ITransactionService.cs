using bank_account_system.Models;

namespace bank_account_system.Abstractions;

public interface ITransactionService
{
    Transaction Deposit(string number, decimal amount);
    Transaction Withdraw(string number, decimal amount);
    Transaction? ApplyInterest(string number);
    List<Transaction> GetHistory(string number);
    List<Transaction> GetAll();
    List<Transaction> Load();
    void Save(List<Transaction> transactions);
}
