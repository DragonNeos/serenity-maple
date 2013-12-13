using Serenity;
using Serenity.Connection;
using Serenity.Database;
using Serenity.Game.Data;
using Serenity.Game.Objects;
using Serenity.Other;
using Serenity.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Serenity
{
    class Program
    {

        #region Variables

        #endregion

        #region Console Methods

        static void Main(string[] args)
        {
            Logger.WriteHeader();

            Initalize();

            while (true)
            {
                Logger.Read();
            }
        }

        static void Write(string pText, params object[] pObjects)
        {
            Console.WriteLine(pText, pObjects);
        }

        #endregion

        #region Essential Methods

        static void Initalize()
        {
            Logger.UpdateTitle();

            GMSKeys.Initialize();

            Master Master = new Master();

            Master.Run();

            InitalizeTimedFunctions();
        }

        #endregion

        #region Other Methods

        private static void InitalizeTimedFunctions()
        {
            Timer MemoryTimer = new Timer();
            MemoryTimer.Interval = 15000;
            MemoryTimer.Elapsed += MemoryTimer_Elapsed;
            MemoryTimer.Start();
        }

        private static void MemoryTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Logger.UpdateTitle();
        }

        #endregion

    }
}
