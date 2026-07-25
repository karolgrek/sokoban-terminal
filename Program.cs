using Sokoban.UI;

namespace Sokoban
{
    class Program
    {
        static void Main()
        {
            IConsole console = new SystemConsole();
            ConsoleMenu.Run(console);
        }
    }
}
