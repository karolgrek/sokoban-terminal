using System;

namespace Sokoban.UI
{
    public class SystemConsole : IConsole
    {
        public void Clear() => Console.Clear();
        public void WriteLine(string value = "") => Console.WriteLine(value);
        public void Write(string value) => Console.Write(value);
        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
    }
}
