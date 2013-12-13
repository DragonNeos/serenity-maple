using Serenity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Servers
{
    public class World
    {
        public byte Id { get; set; }
        public string Name { get; set; }

        public List<Channel> Channels { get; private set; }

        public List<MigrateRequest> MigrateRequests;

        public World(byte pId, ushort pPort, int pChannels)
        {
            Id = pId;
            Channels = new List<Channel>(pChannels);

            for (int i = 0; i < pChannels; i++)
            {
                Channels.Add(new Channel((byte)i, pPort));
                Channels[i].WorldId = Id;
                pPort++;
            }

            MigrateRequests = new List<MigrateRequest>();
        }

        public void Initalize()
        {
            foreach (Channel Channel in Channels)
            {
                Channel.Initalize();
            }
        }

        public void AddMigrationRequest(int pCharacterId, long pSessionId, bool pCashShop)
        {
            lock (MigrateRequests)
            {
                MigrateRequests.Add(new MigrateRequest(pCharacterId, pSessionId, pCashShop));
            }
        }

        public bool EligableMigration(int pCharacterId, long pSessionId, bool pRemove)
        {
            lock (MigrateRequests)
            {
                for (int i = MigrateRequests.Count; i-- > 0; )
                {
                    MigrateRequest itr = MigrateRequests[i];

                    if ((DateTime.Now - itr.Expiry).Seconds > 30)
                    {
                        MigrateRequests.Remove(itr);
                        continue;
                    }

                    if (itr.CharacterId == pCharacterId && itr.SessionId == pSessionId)
                    {
                        if (pRemove)
                            MigrateRequests.Remove(itr);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CashShopMigration(int pCharacterId)
        {
            lock (MigrateRequests)
            {
                for (int i = MigrateRequests.Count; i-- > 0; )
                {
                    MigrateRequest itr = MigrateRequests[i];
                    if (itr.CharacterId == pCharacterId)
                    {
                        MigrateRequests.Remove(itr);
                        return itr.CashShop;
                    }
                }
            }
            return false;
        }

        public int[] GetChannelLoads()
        {
            var Final = new int[Channels.Count];

            for (int i = 0; i < Final.Length; i++)
            {
                Final[i] = Channels[i].Load;
            }

            return Final;
        }

        public int CurrentLoad
        {
            get
            {
                int Final = 0;

                foreach (Channel Channel in Channels)
                    Final += Channel.Load;

                return Final;
            }
        }
    }
}
