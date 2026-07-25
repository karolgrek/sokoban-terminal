using System;
using System.Diagnostics;
using System.Linq;
using Sokoban.UI;

namespace Sokoban.Tests
{
    public class UISmokeTests
    {
        public static void RunAll()
        {
            Test_Menu_Quit();
            Test_Menu_Navigation_To_SingleEasy();
            Test_Menu_Tutorial();
            Test_Menu_Navigation_To_MultiHard();
            Test_Gameplay_Win_SingleEasy();
            Console.WriteLine("UI smoke tests passed.");
        }

        private static void Test_Menu_Quit()
        {
            var mockConsole = new MockConsole();
            mockConsole.AddKey('q'); // Press Quit at main menu

            ConsoleMenu.Run(mockConsole);

            // If it returns without throwing exception, and output has "Welcome to Sokoban Terminal!", it works
            Debug.Assert(mockConsole.OutputLines.Any(line => line.Contains("Welcome to Sokoban Terminal!")));
        }

        private static void Test_Menu_Navigation_To_SingleEasy()
        {
            var mockConsole = new MockConsole();
            // Sequence: 
            // '1' (Single Agent)
            // '1' (Easy)
            // 'q' (Quit in GameEngine)
            mockConsole.AddKey('1');
            mockConsole.AddKey('1');
            mockConsole.AddKey('q');
            // 'q' (Quit from "Press any key to return to the main menu") - wait, any key works
            mockConsole.AddKey(' ');
            // 'q' (Quit from main menu)
            mockConsole.AddKey('q');

            ConsoleMenu.Run(mockConsole);

            // Assert that the game board header was printed
            // Assert that the game board header was printed
            Debug.Assert(mockConsole.OutputLines.Any(line => line.Contains("=== Single-Agent: Easy ===")));
            
            // Assert that the main menu was printed again after quitting the game
            Debug.Assert(mockConsole.OutputLines.Count(line => line.Contains("Welcome to Sokoban Terminal!")) == 2);
        }

        private static void Test_Menu_Tutorial()
        {
            var mockConsole = new MockConsole();
            mockConsole.AddKey('t'); // Open tutorial
            mockConsole.AddKey(' '); // Any key to return to menu
            mockConsole.AddKey('q'); // Quit

            ConsoleMenu.Run(mockConsole);

            // Assert tutorial was displayed
            Debug.Assert(mockConsole.OutputLines.Any(line => line.Contains("=== SOKOBAN TUTORIAL ===")));
            // Assert main menu was displayed twice (initially, and after tutorial)
            Debug.Assert(mockConsole.OutputLines.Count(line => line.Contains("Welcome to Sokoban Terminal!")) == 2);
        }

        private static void Test_Menu_Navigation_To_MultiHard()
        {
            var mockConsole = new MockConsole();
            mockConsole.AddKey('2'); // Multi Agent
            mockConsole.AddKey('3'); // Hard
            mockConsole.AddKey('q'); // Quit game
            mockConsole.AddKey(' '); // Any key to return
            mockConsole.AddKey('q'); // Quit menu

            ConsoleMenu.Run(mockConsole);

            Debug.Assert(mockConsole.OutputLines.Any(line => line.Contains("=== Multi-Agent: Hard ===")));
        }

        private static void Test_Gameplay_Win_SingleEasy()
        {
            var mockConsole = new MockConsole();
            mockConsole.AddKey('1'); // Single Agent
            mockConsole.AddKey('1'); // Easy

            // SingleEasy map requires: Left(a), Up(w), Right(d) to push crate on target
            mockConsole.AddKey('a');
            mockConsole.AddKey('w');
            mockConsole.AddKey('d');

            mockConsole.AddKey(' '); // Any key to return (game ends on win)
            mockConsole.AddKey('q'); // Quit menu

            ConsoleMenu.Run(mockConsole);

            // Assert that winning message was displayed
            Debug.Assert(mockConsole.OutputLines.Any(line => line.Contains("Solved!")));
        }
    }
}
