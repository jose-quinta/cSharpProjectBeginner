using bank_account_system.Abstractions;
using bank_account_system.Models;
using bank_account_system.Services;

IAccountService accountService = new AccountService();
ITransactionService transactionService = new TransactionService(accountService);
MenuService menu = new MenuService();
bool salir = false;

while (!salir)
{
    Console.Clear();
    menu.ShowBanner();
    menu.ShowMenu();

    string opcion = menu.GetChoice();
    Console.WriteLine();

    try
    {
        switch (opcion)
        {
            case "1": CrearCuenta(); break;
            case "2": Depositar(); break;
            case "3": Retirar(); break;
            case "4": VerCuenta(); break;
            case "5": Historial(); break;
            case "6": ListarCuentas(); break;
            case "7": AplicarInteres(); break;
            case "8": CerrarCuenta(); break;
            case "9":
                salir = true;
                menu.ShowMessage("\u00a1Hasta luego!", ConsoleColor.Cyan);
                break;
            default:
                menu.ShowMessage("Opci\u00f3n no v\u00e1lida.", ConsoleColor.Red);
                menu.Pause();
                break;
        }
    }
    catch (Exception ex)
    {
        menu.ShowMessage($"Error: {ex.Message}", ConsoleColor.Red);
        menu.Pause();
    }
}

void CrearCuenta()
{
    var (holder, type) = menu.GetAccountInfo();
    if (string.IsNullOrWhiteSpace(holder))
    {
        menu.ShowMessage("El nombre del titular no puede estar vac\u00edo.", ConsoleColor.Red);
        menu.Pause();
        return;
    }
    Account account = accountService.Create(holder, type);
    menu.ShowMessage($"Cuenta creada exitosamente.", ConsoleColor.Green);
    menu.ShowAccount(account);
    menu.Pause();
}

void Depositar()
{
    string number = menu.GetAccountNumber("N\u00famero de cuenta: ");
    if (string.IsNullOrWhiteSpace(number)) return;

    decimal amount = menu.GetAmount("Monto a depositar: ");
    Transaction txn = transactionService.Deposit(number, amount);
    menu.ShowMessage($"Dep\u00f3sito exitoso. Nuevo saldo: {txn.NewBalance:C2}", ConsoleColor.Green);
    menu.Pause();
}

void Retirar()
{
    string number = menu.GetAccountNumber("N\u00famero de cuenta: ");
    if (string.IsNullOrWhiteSpace(number)) return;

    decimal amount = menu.GetAmount("Monto a retirar: ");
    Transaction txn = transactionService.Withdraw(number, amount);
    menu.ShowMessage($"Retiro exitoso. Nuevo saldo: {txn.NewBalance:C2}", ConsoleColor.Green);
    if (txn.NewBalance < 0)
        menu.ShowMessage("Su cuenta ha entrado en sobregiro. Se aplic\u00f3 una comisi\u00f3n.", ConsoleColor.Magenta);
    menu.Pause();
}

void VerCuenta()
{
    string number = menu.GetAccountNumber("N\u00famero de cuenta: ");
    if (string.IsNullOrWhiteSpace(number)) return;

    Account? account = accountService.FindByNumber(number);
    if (account == null)
    {
        menu.ShowMessage("Cuenta no encontrada.", ConsoleColor.Red);
        menu.Pause();
        return;
    }
    menu.ShowAccount(account);

    List<Transaction> lastTxns = transactionService.GetHistory(number).Take(5).ToList();
    if (lastTxns.Count > 0)
    {
        Console.WriteLine("\n\u00daltimos movimientos:");
        foreach (var t in lastTxns)
            menu.ShowTransaction(t);
    }
    menu.Pause();
}

void Historial()
{
    string number = menu.GetAccountNumber("N\u00famero de cuenta: ");
    if (string.IsNullOrWhiteSpace(number)) return;

    Account? account = accountService.FindByNumber(number);
    if (account == null)
    {
        menu.ShowMessage("Cuenta no encontrada.", ConsoleColor.Red);
        menu.Pause();
        return;
    }

    List<Transaction> txns = transactionService.GetHistory(number);
    menu.ShowTransactionList(txns);
    menu.Pause();
}

void ListarCuentas()
{
    List<Account> accounts = accountService.GetAll();
    menu.ShowAccountList(accounts);
    menu.Pause();
}

void AplicarInteres()
{
    string number = menu.GetAccountNumber("N\u00famero de cuenta: ");
    if (string.IsNullOrWhiteSpace(number)) return;

    Transaction? txn = transactionService.ApplyInterest(number);
    if (txn == null)
    {
        menu.ShowMessage("La cuenta no genera inter\u00e9s en este momento (saldo <= 0 o no es Savings).", ConsoleColor.Yellow);
    }
    else
    {
        menu.ShowMessage($"Inter\u00e9s aplicado: {txn.Amount:C2}. Nuevo saldo: {txn.NewBalance:C2}", ConsoleColor.Green);
    }
    menu.Pause();
}

void CerrarCuenta()
{
    string number = menu.GetAccountNumber("N\u00famero de cuenta: ");
    if (string.IsNullOrWhiteSpace(number)) return;

    Account? account = accountService.FindByNumber(number);
    if (account == null)
    {
        menu.ShowMessage("Cuenta no encontrada.", ConsoleColor.Red);
        menu.Pause();
        return;
    }
    if (!account.IsActive)
    {
        menu.ShowMessage("La cuenta ya est\u00e1 cerrada.", ConsoleColor.Yellow);
        menu.Pause();
        return;
    }

    Console.WriteLine();
    menu.ShowAccount(account);

    if (!menu.ConfirmAction("\u00bfEst\u00e1 seguro de cerrar esta cuenta? [S/N]: "))
    {
        menu.ShowMessage("Operaci\u00f3n cancelada.", ConsoleColor.Yellow);
        menu.Pause();
        return;
    }

    accountService.Close(number);
    menu.ShowMessage("Cuenta cerrada exitosamente.", ConsoleColor.Green);
    menu.Pause();
}
