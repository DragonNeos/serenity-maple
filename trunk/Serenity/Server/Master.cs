using Serenity.Data;
using Serenity.Database;
using Serenity.Game.Objects;
using Serenity.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Servers
{
    public class Master
    {
        public static Master Instance { get; set; }

        public Login Login { get; private set; }
        public List<World> Worlds { get; private set; }
        public CashShop CashShop { get; private set; }

        public Accessor Accessor { get; private set;}

        public DataProvider DataProvider { get; private set; }

        public Master()
        {
            Instance = this;

            Accessor = new Accessor();

            DataProvider = new DataProvider();

            Login = new Login();
            Worlds = new List<World>(Constants.WorldNames.Length);

            ushort Port = 8485;

            for (int i = 0; i < Worlds.Capacity; i++)
            {
                World World = new World((byte)i, Port, 2);
                World.Name = Constants.WorldNames[i];
                Port += 2;

                Worlds.Add(World);
            }

            CashShop = new CashShop(9000);
        }

        public void Run()
        {
            Console.WriteLine("");

            DataProvider.Load(AppDomain.CurrentDomain.BaseDirectory + @"\NX\");

            foreach (World World in Worlds)
            {
                foreach (Channel Channel in World.Channels)
                {
                    foreach (KeyValuePair<int, Map> Map in DataProvider.Maps)
                        Channel.Maps.Add(Map.Key, Map.Value);
                }
            }

            Login.Initalize();

            foreach (World World in Worlds)
                World.Initalize();

            CashShop.Initalize();

            Logger.WriteLog(Logger.LogTypes.Debug, "Server started succesfully.");
            Logger.Space(2);
        }
    }
}
