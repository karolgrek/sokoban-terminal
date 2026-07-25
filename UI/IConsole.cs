using System;

namespace Sokoban.UI
{
    public interface IConsole
    {
        void Clear();
        void WriteLine(string value = "");
        void Write(string value);
        ConsoleKeyInfo ReadKey(bool intercept);
    }
}
