namespace text_based_rpg.Models;

public class CombatLog
{
    public int Turn { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Damage { get; set; }
    public bool IsPlayerTurn { get; set; }
}
