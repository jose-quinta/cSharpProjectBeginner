namespace text_based_rpg.Models;

public class Player
{
    public string Name { get; set; } = string.Empty;
    public CharacterClass Class { get; set; }
    public int Level { get; set; } = 1;
    public int XP { get; set; }
    public int XPToNext { get; set; } = 50;
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int MP { get; set; }
    public int MaxMP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Gold { get; set; }
    public List<Item> Inventory { get; set; } = new();
    public Item? Weapon { get; set; }
    public Item? Armor { get; set; }
    public int CurrentFloor { get; set; } = 1;

    public int TotalAttack => Attack + (Weapon?.Bonus ?? 0);
    public int TotalDefense => Defense + (Armor?.Bonus ?? 0);

    public bool IsAlive => HP > 0;

    public void TakeDamage(int damage)
    {
        HP = Math.Max(0, HP - damage);
    }

    public void Heal(int amount)
    {
        HP = Math.Min(MaxHP, HP + amount);
    }

    public void RestoreMana(int amount)
    {
        MP = Math.Min(MaxMP, MP + amount);
    }
}
