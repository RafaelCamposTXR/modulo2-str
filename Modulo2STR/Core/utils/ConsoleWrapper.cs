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

        private static void SetConsoleColor(string cor)
        {
            switch (cor.ToLower())
            {
                case "preto":
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case "azul escuro":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "verde escuro":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case "ciano escuro":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case "vermelho escuro":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case "magenta escuro":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "amarelo escuro":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "cinza":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case "cinza escuro":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case "azul":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "verde":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "ciano":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "vermelho":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "amarelo":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case "branco":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
        }
    }

}
