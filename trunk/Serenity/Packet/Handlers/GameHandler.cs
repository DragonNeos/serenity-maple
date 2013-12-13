using Serenity.Game;
using Serenity.Game.Objects;
using Serenity.Servers;
using Serenity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets.Handlers
{
    public static class GameHandler
    {
        public static void HandleChangeMap(Client pClient, Packet pPacket)
        {
            Console.WriteLine(pPacket.ToString());
            Map Map = pClient.Character.ChannelMaps[pClient.Character.MapId];

            byte Unk = pPacket.ReadByte();
            int Opcode = pPacket.ReadInt();
            int ToMap = pPacket.ReadInt();

            switch (Opcode)
            {
                case 0:
                    {
                        if (pClient.Character.HP == 0)
                        {
                            //pClient.Character.HandleDeath();
                        }
                        break;
                    }
                case -1:
                    {
                        string PortalName = pPacket.ReadMapleString();

                        if (Map.Portals.ContainsKey(PortalName)){
                            Portal Portal = Map.Portals[PortalName];
                            Portal To = Master.Instance.DataProvider.Maps[Portal.ToMapID].Portals[Portal.ToName];
                            pClient.Character.ChangeMap(Portal.ToMapID, To);
                        }
                        break;
                    }
                default:
                    {
                        // Change to map if admin.
                    }
                    break;
            }
        }

        public static void HandlePlayerMovement(Client pClient, Packet pPacket)
        {
            Character Character = pClient.Character;

            pPacket.Skip(14);
            pPacket.ReadInt();
            pPacket.ReadInt();

            Pos StartPos = Character.Position;

            if (Character == null)
                return;

            List<LifeMovementFragment> Res = new List<LifeMovementFragment>();

            try
            {
                Res = MovementParse.ParseMovement(pPacket, 1);
            }
            catch
            {
                Console.WriteLine("AIOBE Type: 1\r\n" + pPacket.ToString());
                return;
            }

            //Character.CurrentMap.SendPacket(MapPacket.PlayerMove(Character.Id, Res, StartPos));

            MovementParse.UpdatePosition(Res, Character, 0);
            Pos Pos = Character.Position;

            //Character.CurrentMap.MovePlayer(Character, Pos);
        }

        public static void HandlePlayerChat(Client pClient, Packet pPacket)
        {
            int Tick = pPacket.ReadInt();
            string Message = pPacket.ReadMapleString();
            byte Unknown = pPacket.ReadByte();

            if (Message.Length >= 80)
                return;

            if (Message.StartsWith("!"))
            {
                string[] Sub = Message.Split(' ');

                switch (Sub[0].ToLower())
                {
                    case "!map":
                        int Id = int.Parse(Sub[1]);
                        pClient.Character.ChangeMap(Id);
                        break;
                }
            }
            else
            {
                pClient.Character.CurrentMap.SendPacket(MapPacket.PlayerChat(pClient.Character.Id, Message, false, 1));
            }
        }

        public static void HandlePlayerDamage(Client pClient, Packet pPacket)
        {
            pPacket.Skip(4);
            pPacket.ReadInt(); // Tick.
            sbyte Type = pPacket.ReadSByte();
            pPacket.Skip(1);
            int Damage = pPacket.ReadInt();
            pPacket.Skip(2);
           int MobId = 0;

           if (Type != -2 && Type != -3 && Type != -4)
               MobId = pPacket.ReadInt();

           pClient.Character.ModifyHP((short)-Damage);
        }
    }
}

