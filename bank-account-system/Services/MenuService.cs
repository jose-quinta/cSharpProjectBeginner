using bank_account_system.Models;

namespace bank_account_system.Services;

public class MenuService
{
    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== BANK ACCOUNT SYSTEM ===");
        Console.ResetColor();
    }

    public void ShowMenu()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("1.  Crear cuenta");
        Console.WriteLine("2.  Depositar");
        Console.WriteLine("3.  Retirar");
        Console.WriteLine("4.  Ver cuenta");
        Console.WriteLine("5.  Historial de movimientos");
        Console.WriteLine("6.  Listar cuentas");
        Console.WriteLine("7.  Aplicar inter\u00e9s (Savings)");
        Console.WriteLine("8.  Cerrar cuenta");
        Console.WriteLine("9.  Salir");
        Console.ResetColor();
        Console.Write("Seleccione una opci\u00f3n (n\u00famero o letra): ");
    }

    public string GetChoice()
    {
        var key = Console.ReadKey(true);
        return key.Key.ToString() switch
        {
            "D1" or "NumPad1" => "1",
            "D2" or "NumPad2" => "2",
            "D3" or "NumPad3" => "3",
            "D4" or "NumPad4" => "4",
            "D5" or "NumPad5" => "5",
            "D6" or "NumPad6" => "6",
            "D7" or "NumPad7" => "7",
            "D8" or "NumPad8" => "8",
            "D9" or "NumPad9" => "9",
            "Escape" => "9",
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "c" => "1",
                "d" => "2",
                "r" => "3",
                "v" => "4",
                "h" => "5",
                "l" => "6",
                "i" => "7",
                "e" => "8",
                "s" or "x" => "9",
                _ => ""
            }
        };
    }

    public (string holder, AccountType type) GetAccountInfo()
    {
        Console.Write("Nombre del titular: ");
        string holder = (Console.ReadLine() ?? "").Trim();

        Console.WriteLine("Tipo de cuenta:");
        Console.WriteLine("  1. Savings (Ahorro)");
        Console.WriteLine("  2. Checking (Corriente)");
        Console.Write("Seleccione (1-2): ");

        AccountType type = AccountType.Savings;
        string input = (Console.ReadLine() ?? "").Trim();
        if (input == "2") type = AccountType.Checking;

        return (holder, type);
    }

    public string GetAccountNumber(string prompt)
    {
        Console.Write(prompt);
        return (Console.ReadLine() ?? "").Trim();
    }

    public decimal GetAmount(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = (Console.ReadLine() ?? "").Trim();
            if (decimal.TryParse(input, out decimal amount) && amount > 0)
                return amount;
            ShowMessage("Monto inv\u00e1lido. Ingrese un n\u00famero positivo.", ConsoleColor.Red);
        }
    }

    public void ShowAccount(Account account)
    {
        string typeLabel = account.Type == AccountType.Savings ? "Savings (Ahorro)" : "Checking (Corriente)";
        string status = account.IsActive ? "Activa" : "Cerrada";

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"N\u00famero:     {account.Number}");
        Console.WriteLine($"Titular:     {account.HolderName}");
        Console.WriteLine($"Tipo:        {typeLabel}");
        Console.WriteLine($"Saldo:       {account.Balance,14:C2}");
        Console.WriteLine($"Estado:      {status}");
        Console.WriteLine($"Creada:      {account.CreatedAt:dd/MM/yyyy HH:mm}");
        Console.WriteLine(new string('-', 50));
        Console.ResetColor();
    }

    public void ShowAccountList(List<Account> accounts)
    {
        Console.WriteLine();

        if (accounts.Count == 0)
        {
            ShowMessage("No hay cuentas registradas.", ConsoleColor.Yellow);
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Cuentas ({accounts.Count}):");
        Console.ResetColor();

        Console.WriteLine(new string('-', 85));
        Console.WriteLine($"{"N\u00famero",-14} {"Titular",-20} {"Tipo",-10} {"Saldo",12} {"Estado",-8}");
        Console.WriteLine(new string('-', 85));

        foreach (Account a in accounts)
        {
            string type = a.Type == AccountType.Savings ? "Savings" : "Checking";
            string status = a.IsActive ? "Activa" : "Cerrada";
            Console.WriteLine($"{a.Number,-14} {a.HolderName,-20} {type,-10} {a.Balance,12:C2} {status,-8}");
        }
    }

    public void ShowTransaction(Transaction t)
    {
        string typeLabel = t.Type switch
        {
            TransactionType.Deposit => "Dep\u00f3sito",
            TransactionType.Withdrawal => "Retiro",
            TransactionType.Interest => "Inter\u00e9s",
            TransactionType.OverdraftFee => "Comisi\u00f3n",
            _ => t.Type.ToString()
        };

        ConsoleColor color = t.Type switch
        {
            TransactionType.Deposit or TransactionType.Interest => ConsoleColor.Green,
            TransactionType.Withdrawal => ConsoleColor.Red,
            TransactionType.OverdraftFee => ConsoleColor.Magenta,
            _ => ConsoleColor.Gray
        };

        Console.ForegroundColor = color;
        Console.WriteLine($"{t.Date:dd/MM/yy HH:mm} | {typeLabel,-10} | {t.Amount,10:C2} | {t.PreviousBalance,10:C2} -> {t.NewBalance,10:C2} | {t.Description}");
        Console.ResetColor();
    }

    public void ShowTransactionList(List<Transaction> transactions)
    {
        Console.WriteLine();

        if (transactions.Count == 0)
        {
            ShowMessage("No hay movimientos registrados.", ConsoleColor.Yellow);
            return;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Movimientos ({transactions.Count}):");
        Console.ResetColor();

        Console.WriteLine(new string('-', 100));
        Console.WriteLine($"{"Fecha",-14} {"Tipo",-12} {"Monto",10} {"Saldo Anterior",14} {"Saldo Actual",12} {"Descripci\u00f3n"}");
        Console.WriteLine(new string('-', 100));

        foreach (Transaction t in transactions)
            ShowTransaction(t);
    }

    public void ShowMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public bool ConfirmAction(string prompt)
    {
        Console.Write(prompt);
        var key = Console.ReadKey(true);
        Console.WriteLine(key.KeyChar);
        return key.KeyChar is 's' or 'S';
    }

    public void Pause()
    {
        Console.Write("\nPresione cualquier tecla para continuar...");
        Console.ReadKey(true);
    }
}
