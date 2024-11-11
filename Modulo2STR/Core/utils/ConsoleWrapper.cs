using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo2STR.Core.utils
{
    using System;

    class ConsoleWrapper
    {
        public static void WriteLine(string message, string color)
        {
            SetConsoleColor(color);
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Write(string message, string color)
        {
            SetConsoleColor(color);

            Console.Write(message);

            Console.ResetColor();
        }

        private static void SetConsoleColor(string color)
        {
            switch (color.ToLower())
            {
                case "amarelo":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "roxo":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "vermelho":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "laranja":
                    Console.ForegroundColor = ConsoleColor.DarkYellow; 
                    break;
                case "verde":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
        }
    }

}
