using text_based_rpg.Models;

namespace text_based_rpg.Abstractions;

public interface IGameService
{
    Player CreatePlayer(string name, CharacterClass classType);
    Enemy GenerateEnemy(int floor);
    int CalculateDamage(int attackerAtk, int defenderDef);
    void EnemyAttack(Player player, Enemy enemy, bool playerDefending, out CombatLog log);
    bool PlayerAttack(Player player, Enemy enemy, out CombatLog log);
    void ApplyRewards(Player player, Enemy enemy);
    void LevelUp(Player player);
    bool UsePotion(Player player);
    bool TryFlee(Player player, Enemy enemy);
    Item? GetRandomDrop(int floor);
    List<Item> GetShopItems(int floor);
    bool BuyItem(Player player, Item item);
    void EquipItem(Player player, Item item);
    void NextFloor(Player player);
    string ClassName(CharacterClass c);
}
