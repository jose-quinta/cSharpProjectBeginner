namespace text_based_rpg.Models;

public class Item
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Bonus { get; set; }
    public int Value { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsConsumable { get; set; }
    public int HealAmount { get; set; }
}
