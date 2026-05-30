using text_based_rpg.Models;

namespace text_based_rpg.Services;

public class MenuService
{
    public void ShowBanner()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"╔══════════════════════════════════════╗");
        Console.WriteLine(@"║       TEXT-BASED RPG DUNGEON         ║");
        Console.WriteLine(@"║   ¡Sube la torre y vence al dragón!  ║");
        Console.WriteLine(@"╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" MENÚ PRINCIPAL");
        Console.ResetColor();
        Console.WriteLine(" ────────────────────────────");
        Console.WriteLine(" [1] Nueva partida");
        Console.WriteLine(" [2] Continuar");
        Console.WriteLine(" [3] Salir");
        Console.WriteLine();
        Console.Write(" Seleccione una opción: ");
    }

    public string GetChoice()
    {
        var key = Console.ReadKey(true);
        return key.Key.ToString() switch
        {
            "D1" or "NumPad1" => "1",
            "D2" or "NumPad2" => "2",
            "D3" or "NumPad3" => "3",
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "n" => "1",
                "c" => "2",
                "s" => "3",
                _ => ""
            }
        };
    }

    public string GetCombatChoice()
    {
        var key = Console.ReadKey(true);
        return key.Key.ToString() switch
        {
            "D1" or "NumPad1" => "1",
            "D2" or "NumPad2" => "2",
            "D3" or "NumPad3" => "3",
            "D4" or "NumPad4" => "4",
            _ => key.KeyChar.ToString().ToLower() switch
            {
                "a" => "1",
                "d" => "2",
                "p" => "3",
                "h" => "4",
                _ => ""
            }
        };
    }

    public string GetName()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(" Ingrese el nombre de su personaje:");
        Console.ResetColor();
        Console.Write(" Nombre: ");
        return Console.ReadLine()?.Trim() ?? "Héroe";
    }

    public CharacterClass GetClassChoice()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" Selecciona tu clase:");
        Console.ResetColor();
        Console.WriteLine(" ────────────────────────────");
        Console.WriteLine(" [1] Guerrero  (HP:120, ATK:12, DEF:10)");
        Console.WriteLine(" [2] Mago      (HP:70,  MP:80, ATK:6)");
        Console.WriteLine(" [3] Arquero   (HP:90,  ATK:10, SPD:10)");
        Console.WriteLine();

        while (true)
        {
            Console.Write(" Clase (1-3): ");
            string? input = Console.ReadLine()?.Trim();
            if (input == "1") return CharacterClass.Warrior;
            if (input == "2") return CharacterClass.Mage;
            if (input == "3") return CharacterClass.Archer;
            ShowError("Opción inválida.");
        }
    }

    public void ShowFloorEntry(int floor)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"\n═══════════════════════════════════════");
        Console.WriteLine($"       PISO {floor} - {FloorName(floor)}");
        Console.WriteLine($"═══════════════════════════════════════\n");
        Console.ResetColor();
    }

    public void ShowCombatHUD(Player player, Enemy enemy)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(" ⚔  ¡COMBATE!");
        Console.ResetColor();
        Console.WriteLine(" ────────────────────────────");
        Console.Write($" {player.Name}");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  [{ClassName(player.Class)}]  Nv.{player.Level}");
        Console.ResetColor();
        DrawBar("HP", player.HP, player.MaxHP, ConsoleColor.Green, ConsoleColor.Red);
        DrawBar("MP", player.MP, player.MaxMP, ConsoleColor.Blue, ConsoleColor.DarkBlue);
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" {enemy.Name}");
        Console.ResetColor();
        Console.WriteLine($"  (Piso {player.CurrentFloor})");
        DrawBar("HP", enemy.HP, enemy.MaxHP, ConsoleColor.Red, ConsoleColor.DarkRed);
        Console.WriteLine();
        Console.WriteLine(" ────────────────────────────");
    }

    public void ShowCombatMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" [a/1] Atacar    [d/2] Defender    [p/3] Poción    [h/4] Huir");
        Console.ResetColor();
        Console.Write(" Acción: ");
    }

    public void ShowCombatLog(CombatLog log)
    {
        if (log.Damage > 0)
        {
            Console.ForegroundColor = log.IsPlayerTurn ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($" {log.Message}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($" {log.Message}");
            Console.ResetColor();
        }
    }

    public void ShowPlayerStats(Player player)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         ESTADÍSTICAS                 ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"  Nombre:     {player.Name}");
        Console.WriteLine($"  Clase:      {ClassName(player.Class)}");
        Console.WriteLine($"  Nivel:      {player.Level}");
        Console.WriteLine($"  XP:         {player.XP}/{player.XPToNext}");
        Console.WriteLine($"  HP:         {player.HP}/{player.MaxHP}");
        Console.WriteLine($"  MP:         {player.MP}/{player.MaxMP}");
        Console.WriteLine($"  ATK:        {player.TotalAttack}   (base: {player.Attack})");
        Console.WriteLine($"  DEF:        {player.TotalDefense}   (base: {player.Defense})");
        Console.WriteLine($"  SPD:        {player.Speed}");
        Console.WriteLine($"  Oro:        {player.Gold}");
        Console.WriteLine($"  Piso:       {player.CurrentFloor}");
        Console.WriteLine();
    }

    public void ShowInventory(Player player)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         INVENTARIO                   ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        Console.Write("  Arma:   ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(player.Weapon?.Name ?? "(ninguna)");
        Console.ResetColor();

        Console.Write("  Armadura: ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(player.Armor?.Name ?? "(ninguna)");
        Console.ResetColor();

        Console.WriteLine($"  Oro:        {player.Gold}");
        Console.WriteLine();

        if (player.Inventory.Count == 0)
        {
            Console.WriteLine("  (No tienes objetos)");
        }
        else
        {
            Console.WriteLine("  Objetos:");
            for (int i = 0; i < player.Inventory.Count; i++)
            {
                var item = player.Inventory[i];
                string equipped = "";
                if (player.Weapon == item || player.Armor == item)
                    equipped = " [E]";
                Console.WriteLine($"  [{i + 1}] {item.Name,-18} {item.Description,-18}{equipped}");
            }
        }
        Console.WriteLine();
    }

    public void ShowVictory(Player player, Enemy enemy, Item? drop)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n ¡{enemy.Name} derrotado!");
        Console.ResetColor();
        Console.WriteLine($"  +{enemy.XP} XP");
        Console.WriteLine($"  +{enemy.Gold} oro");

        if (drop != null)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"  ¡{drop.Name} obtenido! ({drop.Description})");
            Console.ResetColor();
        }

        if (player.XP == 0 && player.Level > 1)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n ¡SUBISTE AL NIVEL {player.Level}!");
            Console.WriteLine($"  HP/MP restaurados completamente.");
            Console.ResetColor();
        }
        Console.WriteLine();
    }

    public void ShowLevelUp(Player player)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\n ╔══════════════════════════════════╗");
        Console.WriteLine($" ║    ¡SUBISTE AL NIVEL {player.Level}!   ║");
        Console.WriteLine($" ╚══════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine($"  HP: {player.MaxHP}   MP: {player.MaxMP}");
        Console.WriteLine($"  ATK: {player.Attack}   DEF: {player.Defense}");
        Console.WriteLine($"  HP/MP restaurados.");
        Console.WriteLine();
    }

    public void ShowDefeat()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n ╔══════════════════════════════════╗");
        Console.WriteLine($" ║         ¡HAS MUERTO!             ║");
        Console.WriteLine($" ║     Game Over...                 ║");
        Console.WriteLine($" ╚══════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowBossVictory()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n ╔══════════════════════════════════════════╗");
        Console.WriteLine($" ║        ¡VICTORIA!                        ║");
        Console.WriteLine($" ║   Has derrotado al Dragón y liberado     ║");
        Console.WriteLine($" ║         la torre del mal.                ║");
        Console.WriteLine($" ║        ¡FELICIDADES, CAMPEÓN!            ║");
        Console.WriteLine($" ╚══════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    public void ShowShop(List<Item> items, Player player)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║         TIENDA                       ║");
        Console.WriteLine("╚══════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine($"  Oro: {player.Gold}");
        Console.WriteLine();

        Console.WriteLine("  0. Salir de la tienda");
        for (int i = 0; i < items.Count; i++)
        {
            string canBuy = player.Gold >= items[i].Value ? "" : " (no puedes pagarlo)";
            Console.WriteLine($"  {i + 1}. {items[i].Name,-18} {items[i].Description,-18} {items[i].Value,3} oro{canBuy}");
        }
        Console.WriteLine();
        Console.Write(" Seleccione: ");
    }

    public void ShowFloorTransition(int floor)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine($"\n Subiendo al piso {floor}...");
        Console.ResetColor();
        Thread.Sleep(500);
    }

    public string GetShopChoice(int max)
    {
        string? input = Console.ReadLine()?.Trim();
        return input ?? "0";
    }

    public bool ConfirmAction(string prompt)
    {
        Console.Write($" {prompt} (s/n): ");
        return Console.ReadLine()?.Trim().ToLower() == "s";
    }

    public void Pause()
    {
        Console.Write("\n Presione cualquier tecla para continuar...");
        Console.ReadKey(true);
        Console.WriteLine();
    }

    public void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" Error: {message}");
        Console.ResetColor();
    }

    public void ShowMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($" {message}");
        Console.ResetColor();
    }

    private static void DrawBar(string label, int current, int max, ConsoleColor fillColor, ConsoleColor emptyColor)
    {
        int barWidth = 20;
        int filled = max > 0 ? (int)((double)current / max * barWidth) : 0;
        filled = Math.Clamp(filled, 0, barWidth);
        int empty = barWidth - filled;

        Console.Write($"  {label}: {current,4}/{max,-4} ");
        Console.Write("[");
        Console.ForegroundColor = fillColor;
        Console.Write(new string('█', filled));
        Console.ForegroundColor = emptyColor;
        Console.Write(new string('░', empty));
        Console.ResetColor();
        Console.WriteLine("]");
    }

    private static string ClassName(CharacterClass c)
    {
        return c switch
        {
            CharacterClass.Warrior => "Guerrero",
            CharacterClass.Mage => "Mago",
            CharacterClass.Archer => "Arquero",
            _ => "?"
        };
    }

    private static string FloorName(int floor)
    {
        return floor switch
        {
            1 => "Entrada de la Torre",
            2 => "Salón de las Sombras",
            3 => "Cripta Olvidada",
            4 => "Bosque Petrificado",
            5 => "Caverna Resonante",
            6 => "Biblioteca Maldita",
            7 => "Muralla de Hueso",
            8 => "Cámara de Ecos",
            9 => "Antesala del Dragón",
            10 => "¡La Guarida del Dragón!",
            _ => "Desconocido"
        };
    }
}
