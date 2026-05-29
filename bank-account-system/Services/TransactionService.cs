using System.Text.Json;
using bank_account_system.Abstractions;
using bank_account_system.Models;

namespace bank_account_system.Services;

public class TransactionService : ITransactionService
{
    private const decimal OverdraftLimit = 500m;
    private const decimal OverdraftFee = 35m;
    private const decimal AnnualInterestRate = 0.03m;

    private static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "transactions.json");

    private readonly IAccountService _accountService;

    public TransactionService(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public Transaction Deposit(string number, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("El monto debe ser positivo.");

        List<Account> accounts = _accountService.Load();
        Account? account = accounts.FirstOrDefault(a =>
            a.Number.Equals(number.Trim(), StringComparison.OrdinalIgnoreCase));

        if (account == null)
            throw new InvalidOperationException("Cuenta no encontrada.");
        if (!account.IsActive)
            throw new InvalidOperationException("La cuenta est\u00e1 cerrada.");

        decimal previous = account.Balance;
        account.Balance += amount;

        Transaction txn = new Transaction
        {
            AccountNumber = account.Number,
            Type = TransactionType.Deposit,
            Amount = amount,
            PreviousBalance = previous,
            NewBalance = account.Balance,
            Description = $"Dep\u00f3sito de {amount:C2}",
            Date = DateTime.Now
        };

        _accountService.Save(accounts);
        Save(AddToHistory(txn));
        return txn;
    }

    public Transaction Withdraw(string number, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("El monto debe ser positivo.");

        List<Account> accounts = _accountService.Load();
        Account? account = accounts.FirstOrDefault(a =>
            a.Number.Equals(number.Trim(), StringComparison.OrdinalIgnoreCase));

        if (account == null)
            throw new InvalidOperationException("Cuenta no encontrada.");
        if (!account.IsActive)
            throw new InvalidOperationException("La cuenta est\u00e1 cerrada.");

        decimal previous = account.Balance;
        decimal maxWithdrawal = account.Type == AccountType.Checking
            ? account.Balance + OverdraftLimit
            : account.Balance;

        if (amount > maxWithdrawal)
            throw new InvalidOperationException(
                account.Type == AccountType.Checking
                    ? $"Fondos insuficientes. Puede retirar hasta {maxWithdrawal:C2} (saldo + {OverdraftLimit:C2} de sobregiro)."
                    : $"Fondos insuficientes. Saldo disponible: {account.Balance:C2}.");

        account.Balance -= amount;
        List<Transaction> txns = new List<Transaction>();

        Transaction txn = new Transaction
        {
            AccountNumber = account.Number,
            Type = TransactionType.Withdrawal,
            Amount = amount,
            PreviousBalance = previous,
            NewBalance = account.Balance,
            Description = $"Retiro de {amount:C2}",
            Date = DateTime.Now
        };
        txns.Add(txn);

        if (account.Balance < 0)
        {
            account.Balance -= OverdraftFee;
            Transaction feeTxn = new Transaction
            {
                AccountNumber = account.Number,
                Type = TransactionType.OverdraftFee,
                Amount = OverdraftFee,
                PreviousBalance = txn.NewBalance,
                NewBalance = account.Balance,
                Description = $"Comisi\u00f3n por sobregiro de {OverdraftFee:C2}",
                Date = DateTime.Now
            };
            txns.Add(feeTxn);
        }

        _accountService.Save(accounts);

        List<Transaction> history = Load();
        history.AddRange(txns);
        Save(history);

        return txn;
    }

    public Transaction? ApplyInterest(string number)
    {
        List<Account> accounts = _accountService.Load();
        Account? account = accounts.FirstOrDefault(a =>
            a.Number.Equals(number.Trim(), StringComparison.OrdinalIgnoreCase));

        if (account == null)
            throw new InvalidOperationException("Cuenta no encontrada.");
        if (!account.IsActive)
            throw new InvalidOperationException("La cuenta est\u00e1 cerrada.");
        if (account.Type != AccountType.Savings)
            throw new InvalidOperationException("Solo cuentas de ahorro (Savings) generan inter\u00e9s.");
        if (account.Balance <= 0)
            return null;

        decimal interest = account.Balance * AnnualInterestRate / 12;
        interest = Math.Round(interest, 2);

        if (interest <= 0)
            return null;

        decimal previous = account.Balance;
        account.Balance += interest;

        Transaction txn = new Transaction
        {
            AccountNumber = account.Number,
            Type = TransactionType.Interest,
            Amount = interest,
            PreviousBalance = previous,
            NewBalance = account.Balance,
            Description = $"Inter\u00e9s mensual de {interest:C2} ({(AnnualInterestRate * 100):F1}% anual)",
            Date = DateTime.Now
        };

        _accountService.Save(accounts);
        Save(AddToHistory(txn));
        return txn;
    }

    public List<Transaction> GetHistory(string number)
    {
        List<Transaction> all = Load();
        return all.Where(t =>
            t.AccountNumber.Equals(number.Trim(), StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(t => t.Date)
            .ToList();
    }

    public List<Transaction> GetAll()
    {
        List<Transaction> transactions = Load();
        return transactions.OrderByDescending(t => t.Date).ToList();
    }

    public List<Transaction> Load()
    {
        if (!File.Exists(FilePath))
            return new List<Transaction>();

        string json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
    }

    public void Save(List<Transaction> transactions)
    {
        string json = JsonSerializer.Serialize(transactions, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    private List<Transaction> AddToHistory(Transaction txn)
    {
        List<Transaction> history = Load();
        history.Add(txn);
        return history;
    }
}
