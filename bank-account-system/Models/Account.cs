namespace bank_account_system.Models;

public class Account
{
    public string Number { get; set; } = string.Empty;
    public string HolderName { get; set; } = string.Empty;
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;
}
