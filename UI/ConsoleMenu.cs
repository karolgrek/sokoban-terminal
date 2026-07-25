using System;
using Sokoban.Core;

namespace Sokoban.UI
{
    public class ConsoleMenu
    {
        private static readonly string[] easy = {
            "###",
            "#.#",
            "#X#",
            "#o#",
            "###",
        };
        private static readonly string[] medium = {
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
        private static readonly string[] hard = {
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
        private static readonly string[] collaborative = {
            "       ###",
            "########o#",
            "#>X      #",
            "######## #",
            "       #.#",
            "       ###",
        };
        private static readonly string[] gap = {
            " ###",
            " # #",
            "## #####",
            "#ovX  .#",
            "########"
        };

        public static void Run()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Sokoban!");
            Console.WriteLine("Select difficulty (or press 'T' for Tutorial):");
            Console.WriteLine("1 - Easy");
            Console.WriteLine("2 - Medium");
            Console.WriteLine("3 - Hard");
            Console.WriteLine("4 - Gap");
            Console.WriteLine("5 - Collaborative");

            char choice;
            while (true)
            {
                choice = Console.ReadKey(true).KeyChar;
                if (choice == 't' || choice == 'T')
                {
                    ShowTutorial();
                    Console.Clear();
                    Console.WriteLine("Welcome to Sokoban!");
                    Console.WriteLine("Select difficulty:");
                    Console.WriteLine("1 - Easy");
                    Console.WriteLine("2 - Medium");
                    Console.WriteLine("3 - Hard");
                    Console.WriteLine("4 - Gap");
                    Console.WriteLine("5 - Collaborative");
                    continue;
                }
                if (choice >= '1' && choice <= '5') break;
            }

            string[] selectedMap = easy;
            switch (choice)
            {
                case '1': selectedMap = easy; break;
                case '2': selectedMap = medium; break;
                case '3': selectedMap = hard; break;
                case '4': selectedMap = gap; break;
                case '5': selectedMap = collaborative; break;
            }

            GameEngine engine = new GameEngine(selectedMap);
            engine.GameLoop();
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
            Console.WriteLine("Q - Quit the game");
            Console.WriteLine();
            Console.WriteLine("Symbols:");
            Console.WriteLine(" o  - You (the player)");
            Console.WriteLine(" #  - Wall");
            Console.WriteLine(" X  - Crate");
            Console.WriteLine(" .  - Target");
            Console.WriteLine(" x  - Crate already on a target");
            Console.WriteLine(" ^v>< - Other agents (if any)");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey(true);
        }
    }
}
