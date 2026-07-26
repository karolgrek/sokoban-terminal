using System;
using System.Collections.Generic;
using Sokoban.UI;

namespace Sokoban.Tests
{
    public class MockConsole : IConsole
    {
        public List<string> OutputLines { get; } = new List<string>();
        public Queue<ConsoleKeyInfo> InputKeys { get; } = new Queue<ConsoleKeyInfo>();
        
        private string currentLine = "";

        public void Clear()
        {
            OutputLines.Add("--- CLEAR ---");
            currentLine = "";
        }

        public void Write(string value)
        {
            currentLine += value;
            if (currentLine.Contains("\n"))
            {
                var parts = currentLine.Split('\n');
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    OutputLines.Add(parts[i]);
                }
                currentLine = parts[parts.Length - 1];
            }
        }

        public void WriteLine(string value = "")
        {
            Write(value + "\n");
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            if (InputKeys.Count > 0)
                return InputKeys.Dequeue();
            throw new Exception("MockConsole ran out of predefined keys to read.");
        }

        public void AddKey(char keyChar)
        {
            InputKeys.Enqueue(new ConsoleKeyInfo(keyChar, (ConsoleKey)char.ToUpper(keyChar), false, false, false));
        }
    }
}
