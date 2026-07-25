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

        public static void Run(IConsole console)
        {
            while (true)
            {
                console.Clear();
                console.WriteLine("Welcome to Sokoban Terminal!");
                console.WriteLine("Select game mode (or press 'T' for Tutorial):");
                console.WriteLine("1 - Single-Agent (Classic Sokoban)");
                console.WriteLine("2 - Multi-Agent (Multiple agents moving at the same time)");
                console.WriteLine("Q - Quit");

                char modeChoice = console.ReadKey(true).KeyChar;

                if (modeChoice == 'q' || modeChoice == 'Q')
                {
                    return;
                }
                else if (modeChoice == 't' || modeChoice == 'T')
                {
                    ShowTutorial(console);
                    continue;
                }
                else if (modeChoice == '1' || modeChoice == '2')
                {
                    RunDifficultyMenu(console, modeChoice);
                }
            }
        }

        private static void RunDifficultyMenu(IConsole console, char modeChoice)
        {
            string modeName = modeChoice == '1' ? "SINGLE-AGENT MODE" : "MULTI-AGENT MODE";
            
            console.Clear();
            console.WriteLine($"--- {modeName} ---");
            console.WriteLine("Select difficulty:");
            console.WriteLine("1 - Easy");
            console.WriteLine("2 - Medium");
            console.WriteLine("3 - Hard");
            console.WriteLine("4 - Impossible");
            console.WriteLine("B - Back to Main Menu");

            char diffChoice;
            while (true)
            {
                diffChoice = console.ReadKey(true).KeyChar;
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

            GameEngine engine = new GameEngine(console, selectedMap, diffName);
            engine.GameLoop();
            
            console.WriteLine("Press any key to return to the main menu...");
            console.ReadKey(true);
        }

        private static void ShowTutorial(IConsole console)
        {
            console.Clear();
            console.WriteLine("=== SOKOBAN TUTORIAL ===");
            console.WriteLine("Goal:");
            console.WriteLine("Push all crates (X) onto the target spots (.).");
            console.WriteLine();
            console.WriteLine("Controls:");
            console.WriteLine("W - Move Up");
            console.WriteLine("S - Move Down");
            console.WriteLine("A - Move Left");
            console.WriteLine("D - Move Right");
            console.WriteLine("R - Restart the current level");
            console.WriteLine("Q - Quit the game (or return to menu)");
            console.WriteLine();
            console.WriteLine("Symbols:");
            console.WriteLine(" o  - You (the player)");
            console.WriteLine(" #  - Wall");
            console.WriteLine(" X  - Crate");
            console.WriteLine(" .  - Target");
            console.WriteLine(" x  - Crate already on a target");
            console.WriteLine(" ^v>< - Other agents (if any)");
            console.WriteLine();
            console.WriteLine("What is an Agent (^v><)?");
            console.WriteLine(" - They are automated entities that move independently on every turn.");
            console.WriteLine(" - They follow a fixed patrol route (e.g., up and down a hallway).");
            console.WriteLine(" - They bounce back when hitting a wall, crate, or another agent.");
            console.WriteLine(" - They DO NOT respond to your movement commands.");
            console.WriteLine();
            console.WriteLine("Rules:");
            console.WriteLine(" - You cannot push other agents.");
            console.WriteLine(" - AGENTS CANNOT COLLIDE! If you or any other agents bump");
            console.WriteLine("   into each other, it's GAME OVER.");
            console.WriteLine();
            console.WriteLine("Press any key to return to the menu...");
            console.ReadKey(true);
        }
    }
}
