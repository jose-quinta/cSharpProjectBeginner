using text_based_rpg.Abstractions;
using text_based_rpg.Models;

namespace text_based_rpg.Services;

public class GameService : IGameService
{
    private static readonly Random Rng = new();

    private static readonly List<Item> AllItems = new()
    {
        new Item { Name = "Daga de Hierro",       Type = "Weapon", Bonus = 3,  Value = 30, Description = "ATK +3" },
        new Item { Name = "Espada Larga",         Type = "Weapon", Bonus = 6,  Value = 60, Description = "ATK +6" },
        new Item { Name = "Hacha de Guerra",      Type = "Weapon", Bonus = 10, Value = 120, Description = "ATK +10" },
        new Item { Name = "Bastón de Madera",     Type = "Weapon", Bonus = 3,  Value = 30, Description = "ATK +3" },
        new Item { Name = "Bastón Arcano",        Type = "Weapon", Bonus = 6,  Value = 70, Description = "ATK +6" },
        new Item { Name = "Arco Corto",           Type = "Weapon", Bonus = 4,  Value = 40, Description = "ATK +4" },
        new Item { Name = "Arco Largo",           Type = "Weapon", Bonus = 8,  Value = 80, Description = "ATK +8" },
        new Item { Name = "Armadura de Cuero",    Type = "Armor",  Bonus = 3,  Value = 30, Description = "DEF +3" },
        new Item { Name = "Cota de Malla",        Type = "Armor",  Bonus = 6,  Value = 60, Description = "DEF +6" },
        new Item { Name = "Armadura de Placas",   Type = "Armor",  Bonus = 10, Value = 120, Description = "DEF +10" },
        new Item { Name = "Túnica de Magia",      Type = "Armor",  Bonus = 4,  Value = 50, Description = "DEF +4" },
        new Item { Name = "Poción de Vida",       Type = "Potion", Bonus = 0,  Value = 15, Description = "Restaura 40 HP", IsConsumable = true, HealAmount = 40 },
        new Item { Name = "Poción de Vida+",      Type = "Potion", Bonus = 0,  Value = 30, Description = "Restaura 80 HP", IsConsumable = true, HealAmount = 80 },
        new Item { Name = "Poción de Maná",       Type = "Potion", Bonus = 0,  Value = 20, Description = "Restaura 30 MP", IsConsumable = true, HealAmount = 30 },
    };

    private static readonly List<Enemy> EnemyTemplates = new()
    {
        new() { Name = "Slime",     HP = 20, Attack = 5,  Defense = 2, Speed = 3,  XP = 15, Gold = 5,  MinFloor = 1, MaxFloor = 3 },
        new() { Name = "Goblin",    HP = 28, Attack = 7,  Defense = 3, Speed = 5,  XP = 22, Gold = 8,  MinFloor = 1, MaxFloor = 3 },
        new() { Name = "Esqueleto", HP = 35, Attack = 9,  Defense = 5, Speed = 4,  XP = 30, Gold = 10, MinFloor = 4, MaxFloor = 6 },
        new() { Name = "Lobo",      HP = 30, Attack = 11, Defense = 3, Speed = 8,  XP = 28, Gold = 7,  MinFloor = 4, MaxFloor = 6 },
        new() { Name = "Orco",      HP = 55, Attack = 13, Defense = 8, Speed = 4,  XP = 45, Gold = 15, MinFloor = 7, MaxFloor = 9 },
        new() { Name = "Mago Oscuro", HP = 40, Attack = 16, Defense = 4, Speed = 7, XP = 50, Gold = 18, MinFloor = 7, MaxFloor = 9 },
    };

    public Player CreatePlayer(string name, CharacterClass classType)
    {
        var player = new Player
        {
            Name = name,
            Class = classType,
            Level = 1,
            XP = 0,
            XPToNext = 50,
            Gold = 10,
            CurrentFloor = 1
        };

        switch (classType)
        {
            case CharacterClass.Warrior:
                player.MaxHP = 120; player.HP = 120;
                player.MaxMP = 20;  player.MP = 20;
                player.Attack = 12; player.Defense = 10; player.Speed = 5;
                break;
            case CharacterClass.Mage:
                player.MaxHP = 70;  player.HP = 70;
                player.MaxMP = 80;  player.MP = 80;
                player.Attack = 6;  player.Defense = 5;  player.Speed = 7;
                break;
            case CharacterClass.Archer:
                player.MaxHP = 90;  player.HP = 90;
                player.MaxMP = 30;  player.MP = 30;
                player.Attack = 10; player.Defense = 6;  player.Speed = 10;
                break;
        }

        player.Inventory.Add(new Item
        {
            Name = "Poción de Vida", Type = "Potion",
            Value = 15, Description = "Restaura 40 HP",
            IsConsumable = true, HealAmount = 40
        });

        return player;
    }

    public Enemy GenerateEnemy(int floor)
    {
        var candidates = EnemyTemplates
            .Where(e => e.MinFloor <= floor && e.MaxFloor >= floor)
            .ToList();

        Enemy template = candidates[Rng.Next(candidates.Count)];

        double scale = 1.0 + (floor - 1) * 0.15;

        return new Enemy
        {
            Name = template.Name,
            MaxHP = (int)(template.HP * scale),
            HP = (int)(template.HP * scale),
            Attack = (int)(template.Attack * scale),
            Defense = (int)(template.Defense * scale),
            Speed = (int)(template.Speed * scale),
            XP = (int)(template.XP * scale),
            Gold = (int)(template.Gold * scale),
            MinFloor = template.MinFloor,
            MaxFloor = template.MaxFloor
        };
    }

    public int CalculateDamage(int attackerAtk, int defenderDef)
    {
        int raw = Math.Max(1, attackerAtk - defenderDef);
        int variance = Rng.Next(-2, 3);
        return Math.Max(1, raw + variance);
    }

    public bool PlayerAttack(Player player, Enemy enemy, out CombatLog log)
    {
        int damage = CalculateDamage(player.TotalAttack, enemy.Defense);
        enemy.HP = Math.Max(0, enemy.HP - damage);

        log = new CombatLog
        {
            IsPlayerTurn = true,
            Damage = damage,
            Message = $"{player.Name} ataca a {enemy.Name} y causa {damage} de daño!"
        };

        return enemy.HP <= 0;
    }

    public void EnemyAttack(Player player, Enemy enemy, bool playerDefending, out CombatLog log)
    {
        int damage = CalculateDamage(enemy.Attack, player.TotalDefense);

        if (playerDefending)
            damage = Math.Max(1, damage / 2);

        player.TakeDamage(damage);

        string defMsg = playerDefending ? " (defendiendo, daño reducido)" : "";
        log = new CombatLog
        {
            IsPlayerTurn = false,
            Damage = damage,
            Message = $"{enemy.Name} ataca a {player.Name} y causa {damage} de daño{defMsg}!"
        };
    }

    public void ApplyRewards(Player player, Enemy enemy)
    {
        player.XP += enemy.XP;
        player.Gold += enemy.Gold;

        while (player.XP >= player.XPToNext)
            LevelUp(player);
    }

    public void LevelUp(Player player)
    {
        player.XP -= player.XPToNext;
        player.Level++;
        player.XPToNext = player.Level * 50;

        double scale = 1.15;

        switch (player.Class)
        {
            case CharacterClass.Warrior:
                player.MaxHP = (int)(player.MaxHP * scale);
                player.MaxMP = (int)(player.MaxMP * scale);
                player.Attack = (int)(player.Attack * scale);
                player.Defense = (int)(player.Defense * scale);
                break;
            case CharacterClass.Mage:
                player.MaxHP = (int)(player.MaxHP * scale);
                player.MaxMP = (int)(player.MaxMP * scale);
                player.Attack = (int)(player.Attack * scale);
                player.Defense = (int)(player.Defense * scale);
                break;
            case CharacterClass.Archer:
                player.MaxHP = (int)(player.MaxHP * scale);
                player.MaxMP = (int)(player.MaxMP * scale);
                player.Attack = (int)(player.Attack * scale);
                player.Defense = (int)(player.Defense * scale);
                break;
        }

        player.HP = player.MaxHP;
        player.MP = player.MaxMP;
    }

    public bool UsePotion(Player player)
    {
        Item? potion = player.Inventory.FirstOrDefault(i => i.IsConsumable);
        if (potion == null)
            return false;

        if (potion.Name.Contains("Maná"))
            player.RestoreMana(potion.HealAmount);
        else
            player.Heal(potion.HealAmount);

        player.Inventory.Remove(potion);
        return true;
    }

    public bool TryFlee(Player player, Enemy enemy)
    {
        int chance = 40 + (player.Speed - enemy.Speed) * 2;
        chance = Math.Clamp(chance, 15, 80);
        return Rng.Next(100) < chance;
    }

    public Item? GetRandomDrop(int floor)
    {
        if (Rng.Next(100) >= 40)
            return null;

        var candidates = AllItems
            .Where(i => i.Type != "Potion")
            .ToList();

        int maxValue = floor switch
        {
            <= 3 => 40,
            <= 6 => 80,
            _ => 120
        };

        candidates = candidates.Where(i => i.Value <= maxValue).ToList();
        return candidates.Count > 0 ? candidates[Rng.Next(candidates.Count)] : null;
    }

    public List<Item> GetShopItems(int floor)
    {
        int maxValue = floor switch
        {
            <= 3 => 40,
            <= 6 => 70,
            _ => 120
        };

        var items = AllItems
            .Where(i => i.Value <= maxValue)
            .ToList();

        int count = Math.Min(4, items.Count);
        var shop = new List<Item>();

        // Always offer potions
        var potions = items.Where(i => i.IsConsumable).ToList();
        shop.Add(potions[Rng.Next(potions.Count)]);

        // Offer random equipment
        var equipment = items.Where(i => !i.IsConsumable).ToList();
        var shuffled = equipment.OrderBy(_ => Rng.Next()).Take(count - 1);
        shop.AddRange(shuffled);

        return shop;
    }

    public bool BuyItem(Player player, Item item)
    {
        if (player.Gold < item.Value)
            return false;

        player.Gold -= item.Value;
        player.Inventory.Add(new Item
        {
            Name = item.Name,
            Type = item.Type,
            Bonus = item.Bonus,
            Value = item.Value,
            Description = item.Description,
            IsConsumable = item.IsConsumable,
            HealAmount = item.HealAmount
        });
        return true;
    }

    public void EquipItem(Player player, Item item)
    {
        if (item.Type == "Weapon")
        {
            if (player.Weapon != null)
                player.Inventory.Add(player.Weapon);
            player.Weapon = item;
            player.Inventory.Remove(item);
        }
        else if (item.Type == "Armor")
        {
            if (player.Armor != null)
                player.Inventory.Add(player.Armor);
            player.Armor = item;
            player.Inventory.Remove(item);
        }
    }

    public void NextFloor(Player player)
    {
        player.CurrentFloor++;
    }

    public string ClassName(CharacterClass c)
    {
        return c switch
        {
            CharacterClass.Warrior => "Guerrero",
            CharacterClass.Mage => "Mago",
            CharacterClass.Archer => "Arquero",
            _ => "Desconocido"
        };
    }
}
