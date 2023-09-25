using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.Models
{
    internal static class ConsoleExtendet
    {
        internal static string GetPasswordFromConsole()
        {
            Console.Write("Введите пароль: ");
            Console.CursorVisible = false;
            StringBuilder pass = new StringBuilder();
            bool reading = true;
            while (reading)
            {
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey(true);
                char passChar = consoleKeyInfo.KeyChar;

                if (passChar == '\r' || consoleKeyInfo.Key == ConsoleKey.Enter)
                {
                    reading = false;
                }
                else if (consoleKeyInfo.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass.Remove(pass.Length - 1, 1);
                }
                else
                {
                    pass.Append(passChar);
                }
            }
            Console.WriteLine();
            Console.CursorVisible = true;
            return pass.ToString();
        }

        internal static char GetSelected(string text_select, Dictionary<char, string> list)
        {
            Console.WriteLine($"{text_select}:");
            foreach(var pair in list)
            {
                Console.WriteLine($"{pair.Key}. {pair.Value}");
            }
            bool isSelect = false;
            char? keySelect = null;
            while (!isSelect)
            {
                keySelect = Console.ReadKey(true).KeyChar;
                if (keySelect != null && list.Keys.Contains((char)keySelect))
                {
                    isSelect = true;
                }
            }
            return (char)keySelect;
        }
    }
}
