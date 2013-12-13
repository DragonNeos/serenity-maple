using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Other
{
    public static class Logger
    {
        public static Dictionary<HackTypes, Client> Cheaters = new Dictionary<HackTypes, Client>();

        public enum LogTypes
        {
            Debug,
            Info,
            Warning,
            Error,
            Connect,
            Disconnect,
            NX
        }

        public enum HackTypes
        {
            Speed,
            Dupe,
            Warp,
            Login
        }

        public static bool mCommandEnabled = false;
        public static string mCommandBuffer = "";

        public static void WriteHeader()
        {
            ConsoleColor tmpColor = Console.ForegroundColor;
            Console.WriteLine("                                                                 ");
            Console.Write("                                    ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Serenity");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("                    ");
            Console.WriteLine("                             C# MapleStory Emulator                  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("                                     Fraysa          ");
            Console.WriteLine("                     ");
            Console.ForegroundColor = tmpColor;
            Console.WriteLine();
        }

        //public static void LogCheat(HackTypes type, MapleClient c, string msg = "")
        //{
        //    Cheaters.Add(type, c);
        //    //TODO:
        //    //Log msg and such to file.
        //}

        public static void WriteLog(LogTypes type, string msg, params object[] pObjects)
        {
            string tab = "";
            for (int i = 1; i > (type.ToString().Length / 8); i--)
            {
                tab += "\t";
            }
            if (type == LogTypes.Debug)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\tDebug" + tab + "| ");
            }
            else if (type == LogTypes.Info)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("\tInfo" + tab + "| ");
            }
            else if (type == LogTypes.Warning)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("\tWarning" + tab + "| ");
            }
            else if (type == LogTypes.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\tError" + tab + "| ");
            }
            else if (type == LogTypes.Connect)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("\tConnect" + tab + "| ");
            }
            else if (type == LogTypes.Disconnect)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("\tDisconnect" + tab + "| ");
            }
            else if (type == LogTypes.NX)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tNX" + tab + "| ");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg, pObjects);
            if (mCommandEnabled)
                Console.Write("> {0}", mCommandBuffer);
        }

        public static void Read()
        {
            Console.Read();
        }

        public static void Space(int pSpace)
        {
            for (int i = 0; i < pSpace; i++)
                Console.WriteLine("");
        }

        public static void UpdateTitle()
        {
            GC.Collect();
            //Console.Title = "Serenity - C# MapleStory Emulator | Memory Usage: " + Math.Round((double)GC.GetTotalMemory(false) / 1024) + "KB";
            Process Curr = Process.GetCurrentProcess();
            Console.Title = "Serenity - C# MapleStory Emulator | Memory Usage: " + Math.Round((double)(Curr.PrivateMemorySize64 / 1024)) + "KB";
        }
    }
}
