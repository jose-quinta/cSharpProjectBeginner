namespace text_based_rpg.Models;

public class Enemy
{
    public string Name { get; set; } = string.Empty;
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int XP { get; set; }
    public int Gold { get; set; }
    public int MinFloor { get; set; }
    public int MaxFloor { get; set; }
}
