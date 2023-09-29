using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    class Logger
    {
        public enum LogType
        {
            Info = 0,
            Warn = 1,
            Error = 2,
            Fatal = 3,
            Success = 4,
            Message = 5
        }

        private static void ResetColor () 
        {
            Console.ForegroundColor = ConsoleColor.White;
        }  
        static public void Log(int type, string message)
        {
            if(type == (int)Logger.LogType.Success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[" + Logger.LogType.Success + "] " + message);
                ResetColor();
                
            }
            else if (type == (int)Logger.LogType.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[" + Logger.LogType.Error + "] " + message);
                ResetColor();

            }
            else if (type == (int)Logger.LogType.Warn)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[" + Logger.LogType.Warn + "] " + message);
                ResetColor();

            }
            else if (type == (int)Logger.LogType.Info)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("[" + Logger.LogType.Info + "] " + message);
                ResetColor();

            }
            else if (type == (int)Logger.LogType.Fatal)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("[" + Logger.LogType.Fatal + "] " + message);
                ResetColor();

            }
            else if (type == (int)Logger.LogType.Message)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("--> ["+ "Client " + Logger.LogType.Message + "] " + message);
                ResetColor();

            }
        }

    }
}
