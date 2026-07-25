using System;
using Sokoban.Core;

namespace Sokoban.UI
{
    public class ConsoleMenu
    {
        // --- SINGLE-AGENT MAPS ---
        private static readonly string[] singleEasy = {
            "######",
            "#    #",
            "# X. #",
            "# o  #",
            "######"
        };
        private static readonly string[] singleMedium = {
            "  #####",
            "###   #",
            "#.oX  #",
            "### X.#",
            "#.##X #",
            "# # . ##",
            "#X xXX.#",
            "#   .  #",
            "########",
        };
        private static readonly string[] singleHard = {
            "    #####",
            "    #   #",
            "    #X  #",
            "  ###  X##",
            "  #  X X #",
            "### # ## #   ######",
            "#   # ## #####  ..#",
            "# X  X          ..#",
            "##### ### #o##  ..#",
            "    #     #########",
            "    #######",
        };
        private static readonly string[] singleImpossible = {
            "  ####",
            "###  ####",
            "#     X #",
            "# #  #X #",
            "# . .#o #",
            "#########"
        };

        // --- MULTI-AGENT MAPS ---
        private static readonly string[] multiEasy = {
            "       ###",
            "########o#",
            "#>X      #",
            "######## #",
            "       #.#",
            "       ###",
        };
        private static readonly string[] multiMedium = {
            " ###",
            " # #",
            "## #####",
            "#ovX  .#",
            "########"
        };
        private static readonly string[] multiHard = {
            "  #######",
            "  #     #",
            "### v X ###",
            "#   #     #",
            "# o   ^ . #",
            "###########"
        };
        private static readonly string[] multiImpossible = {
            "##########",
            "#o  X   .#",
            "# v    ^ #",
            "#   X    #",
            "# .    < #",
            "##########"
        };

        public static void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Sokoban Terminal!");
                Console.WriteLine("Select game mode (or press 'T' for Tutorial):");
                Console.WriteLine("1 - Single-Agent (Classic Sokoban)");
                Console.WriteLine("2 - Multi-Agent (Multiple agents moving at the same time)");
                Console.WriteLine("Q - Quit");

                char modeChoice = Console.ReadKey(true).KeyChar;

                if (modeChoice == 'q' || modeChoice == 'Q')
                {
                    return;
                }
                else if (modeChoice == 't' || modeChoice == 'T')
                {
                    ShowTutorial();
                    continue;
                }
                else if (modeChoice == '1' || modeChoice == '2')
                {
                    RunDifficultyMenu(modeChoice);
                }
            }
        }

        private static void RunDifficultyMenu(char modeChoice)
        {
            string modeName = modeChoice == '1' ? "SINGLE-AGENT MODE" : "MULTI-AGENT MODE";
            
            Console.Clear();
            Console.WriteLine($"--- {modeName} ---");
            Console.WriteLine("Select difficulty:");
            Console.WriteLine("1 - Easy");
            Console.WriteLine("2 - Medium");
            Console.WriteLine("3 - Hard");
            Console.WriteLine("4 - Impossible");
            Console.WriteLine("B - Back to Main Menu");

            char diffChoice;
            while (true)
            {
                diffChoice = Console.ReadKey(true).KeyChar;
                if (diffChoice == 'b' || diffChoice == 'B') return;
                if (diffChoice >= '1' && diffChoice <= '4') break;
            }

            string[] selectedMap = singleEasy; // Default fallback
            string diffName = "";

            if (modeChoice == '1') // Single Agent
            {
                switch (diffChoice)
                {
                    case '1': selectedMap = singleEasy; diffName = "Single-Agent: Easy"; break;
                    case '2': selectedMap = singleMedium; diffName = "Single-Agent: Medium"; break;
                    case '3': selectedMap = singleHard; diffName = "Single-Agent: Hard"; break;
                    case '4': selectedMap = singleImpossible; diffName = "Single-Agent: Impossible"; break;
                }
            }
            else // Multi Agent
            {
                switch (diffChoice)
                {
                    case '1': selectedMap = multiEasy; diffName = "Multi-Agent: Easy"; break;
                    case '2': selectedMap = multiMedium; diffName = "Multi-Agent: Medium"; break;
                    case '3': selectedMap = multiHard; diffName = "Multi-Agent: Hard"; break;
                    case '4': selectedMap = multiImpossible; diffName = "Multi-Agent: Impossible"; break;
                }
            }

            GameEngine engine = new GameEngine(selectedMap, diffName);
            engine.GameLoop();
            
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }

        private static void ShowTutorial()
        {
            Console.Clear();
            Console.WriteLine("=== SOKOBAN TUTORIAL ===");
            Console.WriteLine("Goal:");
            Console.WriteLine("Push all crates (X) onto the target spots (.).");
            Console.WriteLine();
            Console.WriteLine("Controls:");
            Console.WriteLine("W - Move Up");
            Console.WriteLine("S - Move Down");
            Console.WriteLine("A - Move Left");
            Console.WriteLine("D - Move Right");
            Console.WriteLine("R - Restart the current level");
            Console.WriteLine("Q - Quit the game (or return to menu)");
            Console.WriteLine();
            Console.WriteLine("Symbols:");
            Console.WriteLine(" o  - You (the player)");
            Console.WriteLine(" #  - Wall");
            Console.WriteLine(" X  - Crate");
            Console.WriteLine(" .  - Target");
            Console.WriteLine(" x  - Crate already on a target");
            Console.WriteLine(" ^v>< - Other agents (if any)");
            Console.WriteLine();
            Console.WriteLine("What is an Agent (^v><)?");
            Console.WriteLine(" - They are automated entities that move independently on every turn.");
            Console.WriteLine(" - They follow a fixed patrol route (e.g., up and down a hallway).");
            Console.WriteLine(" - They bounce back when hitting a wall, crate, or another agent.");
            Console.WriteLine(" - They DO NOT respond to your movement commands.");
            Console.WriteLine();
            Console.WriteLine("Rules:");
            Console.WriteLine(" - You cannot push other agents.");
            Console.WriteLine(" - AGENTS CANNOT COLLIDE! If you or any other agents bump");
            Console.WriteLine("   into each other, it's GAME OVER.");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey(true);
        }
    }
}
