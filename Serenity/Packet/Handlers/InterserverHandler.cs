using Serenity;
using Serenity.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets.Handlers
{
    public static class InterserverHandler
    {
        public static void HandleMigration(Client pClient, Packet pPacket)
        {
            int CharacterId = pPacket.ReadInt();
            pPacket.Skip(18);
            long SessionId = pPacket.ReadLong();

            if (Master.Instance.Worlds[pClient.World].EligableMigration(CharacterId, SessionId, false))
            {
                pClient.SessionId = SessionId;

                pClient.Character = Master.Instance.Accessor.GetCharacter(CharacterId);
                pClient.Account = Master.Instance.Accessor.GetAccount(pClient.Character.AccountId);

                if (pClient.Character.Id != CharacterId)
                {
                    pClient.Session.Disconnect();
                    return;
                }

                pClient.Character.Client = pClient;

                if (pClient.Account.LoggedIn == 2)
                {
                    pClient.Session.Disconnect();
                    return;
                }

                pClient.Account.LoggedIn = 2;

                if (Master.Instance.Worlds[pClient.World].CashShopMigration(pClient.Character.Id))
                {
                    CashShopHandler.HandleEnter(pClient, pPacket);
                }
                else
                {
                    pClient.SendPacket(MapPacket.EnterFieldNew(pClient.Character));
                    Master.Instance.Worlds[pClient.World].Channels[pClient.Channel].Maps[pClient.Character.MapId].AddPlayer(pClient.Character);
                }
            }
            else
            {
                Console.WriteLine("[{0}] Migration failed.", pClient.Session.IP);
            }
        }

        public static void HandleEnterCS(Client pClient, Packet pPacket)
        {
            int CharacterId = pClient.Character.Id;
            long SessionId = pClient.SessionId;

            pClient.Character.Save();

            Master.Instance.Worlds[pClient.World].AddMigrationRequest(CharacterId, SessionId, true);

            pClient.SendPacket(GeneralPacket.ChangeChannel(pClient, Master.Instance.CashShop.Port));
        }

        public static void HandleChannelChange(Client pClient, Packet pPacket)
        {
            byte ChannelId = pPacket.ReadByte();
            int CharacterId = pClient.Character.Id;
            long SessionId = pClient.SessionId;

            pClient.Character.Save();

            Master.Instance.Worlds[pClient.World].AddMigrationRequest(CharacterId, SessionId, false);

            pClient.SendPacket(GeneralPacket.ChangeChannel(pClient, Master.Instance.Worlds[pClient.World].Channels[ChannelId].Port));
        }
    }
}
