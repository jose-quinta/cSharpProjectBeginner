using text_based_rpg.Abstractions;
using text_based_rpg.Models;
using text_based_rpg.Services;

IGameService game = new GameService();
SaveService save = new SaveService();
MenuService menu = new MenuService();

bool salir = false;
while (!salir)
{
    Console.Clear();
    menu.ShowBanner();
    menu.ShowMainMenu();
    string opcion = menu.GetChoice();
    Console.WriteLine();

    if (string.IsNullOrEmpty(opcion))
    {
        menu.ShowError("Opci\u00f3n inv\u00e1lida.");
        menu.Pause();
        continue;
    }

    if (opcion == "3")
    {
        if (menu.ConfirmAction("\u00bfSalir"))
            salir = true;
        continue;
    }

    Player? player = null;

    try
    {
        if (opcion == "1")
        {
            string name = menu.GetName();
            CharacterClass classType = menu.GetClassChoice();
            player = game.CreatePlayer(name, classType);
            Console.WriteLine();
            menu.ShowMessage($"\u00a1Bienvenido, {name} el {game.ClassName(classType)}!", ConsoleColor.Green);
            menu.Pause();
        }
        else if (opcion == "2")
        {
            player = save.Load();
            if (player == null)
            {
                menu.ShowError("No hay partida guardada.");
                menu.Pause();
                continue;
            }
            menu.ShowMessage($"Partida cargada. Piso {player.CurrentFloor}, Nivel {player.Level}.", ConsoleColor.Green);
            menu.Pause();
        }

        if (player == null)
            continue;

        // ── GAME LOOP ──
        bool gameOver = false;
        while (player.CurrentFloor <= 10 && !gameOver && player.IsAlive)
        {
            Console.Clear();
            menu.ShowFloorEntry(player.CurrentFloor);

            if (player.CurrentFloor == 1)
                menu.ShowMessage("La torre se alza frente a ti. Debes llegar al piso 10 y derrotar al drag\u00f3n...", ConsoleColor.DarkGray);

            // Shop (50% chance per floor, but always on floor 1)
            if (player.CurrentFloor == 1 || new Random().Next(2) == 0)
            {
                List<Item> shopItems = game.GetShopItems(player.CurrentFloor);
                bool shopping = true;
                while (shopping)
                {
                    Console.Clear();
                    menu.ShowFloorEntry(player.CurrentFloor);
                    menu.ShowShop(shopItems, player);
                    string shopChoice = menu.GetShopChoice(shopItems.Count);

                    if (shopChoice == "0")
                    {
                        shopping = false;
                    }
                    else if (int.TryParse(shopChoice, out int idx) && idx >= 1 && idx <= shopItems.Count)
                    {
                        Item selected = shopItems[idx - 1];
                        if (game.BuyItem(player, selected))
                        {
                            menu.ShowMessage($"\u00a1{selected.Name} comprado!", ConsoleColor.Green);
                        }
                        else
                        {
                            menu.ShowError("No tienes suficiente oro.");
                        }
                        menu.Pause();
                    }
                }
            }

            if (!player.IsAlive || gameOver)
                break;

            // ── COMBAT ──
            Enemy enemy = game.GenerateEnemy(player.CurrentFloor);

            // Boss on floor 10
            if (player.CurrentFloor == 10)
            {
                enemy = new Enemy
                {
                    Name = "Drag\u00f3n",
                    MaxHP = 120,
                    HP = 120,
                    Attack = 22,
                    Defense = 12,
                    Speed = 8,
                    XP = 200,
                    Gold = 100,
                    MinFloor = 10,
                    MaxFloor = 10
                };
            }

            bool combatOver = false;
            bool playerDefending = false;
            bool fled = false;

            while (!combatOver && player.IsAlive)
            {
                Console.Clear();
                menu.ShowFloorEntry(player.CurrentFloor);
                menu.ShowCombatHUD(player, enemy);
                menu.ShowCombatMenu();

                string action = menu.GetCombatChoice();
                Console.WriteLine();

                if (string.IsNullOrEmpty(action))
                {
                    menu.ShowError("Acci\u00f3n inv\u00e1lida.");
                    menu.Pause();
                    continue;
                }

                switch (action)
                {
                    case "1": // Attack
                    {
                        bool enemyDefeated = game.PlayerAttack(player, enemy, out CombatLog log);
                        menu.ShowCombatLog(log);
                        playerDefending = false;

                        if (enemyDefeated)
                        {
                            combatOver = true;
                            menu.Pause();
                            break;
                        }
                        break;
                    }
                    case "2": // Defend
                    {
                        playerDefending = true;
                        menu.ShowMessage("Te preparas para defender...", ConsoleColor.Blue);
                        break;
                    }
                    case "3": // Potion
                    {
                        if (game.UsePotion(player))
                        {
                            menu.ShowMessage($"\u00a1Usaste una poci\u00f3n! HP: {player.HP}/{player.MaxHP}", ConsoleColor.Green);
                        }
                        else
                        {
                            menu.ShowError("No tienes pociones.");
                            continue;
                        }
                        playerDefending = false;
                        break;
                    }
                    case "4": // Flee
                    {
                        if (game.TryFlee(player, enemy))
                        {
                            menu.ShowMessage("\u00a1Lograste huir!", ConsoleColor.Yellow);
                            fled = true;
                            combatOver = true;
                        }
                        else
                        {
                            menu.ShowMessage("\u00a1No pudiste huir!", ConsoleColor.Red);
                        }
                        playerDefending = false;
                        break;
                    }
                }

                if (combatOver || fled)
                    continue;

                // Enemy turn
                Thread.Sleep(400);
                game.EnemyAttack(player, enemy, playerDefending, out CombatLog enemyLog);
                menu.ShowCombatLog(enemyLog);
                playerDefending = false;

                if (!player.IsAlive)
                {
                    combatOver = true;
                }

                menu.Pause();
            }

            // ── POST-COMBAT ──
            if (!player.IsAlive)
            {
                Console.Clear();
                menu.ShowDefeat();
                save.Delete();
                menu.Pause();
                gameOver = true;
                break;
            }

            if (fled)
            {
                menu.Pause();
                continue;
            }

            // Victory
            Item? drop = game.GetRandomDrop(player.CurrentFloor);
            if (drop != null)
            {
                player.Inventory.Add(drop);
                if (drop.Type == "Weapon" || drop.Type == "Armor")
                {
                    bool equip = menu.ConfirmAction($"\u00bfEquipar {drop.Name}?");
                    if (equip)
                        game.EquipItem(player, drop);
                }
            }

            game.ApplyRewards(player, enemy);

            Console.Clear();
            menu.ShowFloorEntry(player.CurrentFloor);

            // Check level up
            if (player.XP < player.XPToNext && player.Level > 1)
            {
                menu.ShowVictory(player, enemy, drop);
            }
            else
            {
                menu.ShowVictory(player, enemy, drop);
            }

            if (player.CurrentFloor == 10 && enemy.Name == "Drag\u00f3n" && !player.IsAlive)
            {
                // This shouldn't happen since we check above, but just in case
            }

            if (player.CurrentFloor == 10 && enemy.Name == "Drag\u00f3n" && player.IsAlive)
            {
                Console.Clear();
                menu.ShowBossVictory();
                menu.ShowPlayerStats(player);
                menu.ShowMessage("\u00a1Gracias por jugar!", ConsoleColor.Cyan);
                save.Delete();
                menu.Pause();
                gameOver = true;
                break;
            }

            // Next floor
            bool saveQuit = menu.ConfirmAction("\u00bfGuardar y salir?");
            if (saveQuit)
            {
                save.Save(player);
                menu.ShowMessage("Partida guardada.", ConsoleColor.Green);
                menu.Pause();
                gameOver = true;
                break;
            }

            game.NextFloor(player);
            if (player.CurrentFloor <= 10)
                menu.ShowFloorTransition(player.CurrentFloor);
        }

        if (player.CurrentFloor > 10 && player.IsAlive)
        {
            Console.Clear();
            menu.ShowBossVictory();
            menu.ShowPlayerStats(player);
            save.Delete();
            menu.Pause();
        }
    }
    catch (Exception ex)
    {
        menu.ShowError($"Error inesperado: {ex.Message}");
        menu.Pause();
    }
}
