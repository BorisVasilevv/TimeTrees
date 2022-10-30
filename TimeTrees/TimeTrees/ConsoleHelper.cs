using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTrees
{
    public class ConsoleHelper
    {
        public static void CleanConsole()
        {
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth), Console.BackgroundColor = ConsoleColor.Black);
                Console.SetCursorPosition(0, i);
            }
            Console.SetCursorPosition(0, 0);
        }
    }
}
