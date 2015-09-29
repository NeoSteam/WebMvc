using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Global
{
    public enum ConsoleMessageType
    {
        Message,
        Success,
        Fail,
        Warning,
        Error
    }

    public static class ConsoleShow
    {
        public static void WriteLine(ConsoleMessageType emMessType, string strMessage)
        {
            //输出时间
            string strDatetimeNow = "";
            strDatetimeNow = DateTime.Now.ToString("HH:mm:ss ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(strDatetimeNow);
            SetConsoleColorDefault();

            //输出内容
            switch (emMessType)
            {
                case ConsoleMessageType.Message:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case ConsoleMessageType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ConsoleMessageType.Fail:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case ConsoleMessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case ConsoleMessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            Console.WriteLine(strMessage);
            SetConsoleColorDefault();
        }

        private static void SetConsoleColorDefault()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

    }
}
