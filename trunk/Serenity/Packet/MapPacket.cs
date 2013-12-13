using Serenity;
using Serenity.Data;
using Serenity.Game.Objects;
using Serenity.Other;
using Serenity.Servers;
using Serenity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets
{
    public static class MapPacket
    {
        public static Packet EnterFieldNew(Character pCharacter)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Field_Enter);
            p.WriteShort(2);
            p.WriteLong(1L);
            p.WriteLong(2L);
            p.WriteLong(pCharacter.Client.Channel);
            p.WriteByte(0);
            p.WriteByte(1);
            p.WriteInt(0);
            p.WriteByte(1);
            p.WriteShort(0);

            PlayerRandomStream PRS = new PlayerRandomStream();
            PRS.ConnectData(p);
            HelpPacket.AddCharacterInformation(p, pCharacter);

            p.WriteInt(0);
            p.WriteLong(Tools.GetTime(Tools.CurrentTimeMillis()));
            p.WriteInt(100);
            p.WriteShort(0);
            p.WriteByte(1);

            return p;
        }

        public static Packet EnterField(Character pCharacter)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Field_Enter);
            p.WriteShort(2);
            p.WriteLong(1);
            p.WriteLong(2);
            p.WriteLong(pCharacter.Client.Channel);
            p.WriteByte(pCharacter.PortalCount);
            p.WriteByte(2);
            p.WriteBytes(new byte[8]);
            p.WriteInt(pCharacter.MapId);
            p.WriteByte(pCharacter.MapPosition);
            p.WriteInt(pCharacter.HP);
            p.WriteByte(0);
            p.WriteLong(Tools.GetTime(Tools.CurrentTimeMillis()));
            p.WriteInt(100);
            p.WriteShort(0);
            p.WriteByte(1);

            return p;
        }

        public static Packet CharacterEnter(Character pCharacter)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Player_Spawn);
            p.WriteInt(pCharacter.Id);
            p.WriteByte(pCharacter.Level);
            p.WriteMapleString(pCharacter.Name);

            // Quest 111111 -  I don't care for now.
            p.WriteMapleString("");

            // TODO: Guilds!
            p.WriteZero(8);
            p.WriteByte(0);

            // Buffs

            List<Pair<int, int>> BuffValue = new List<Pair<int, int>>();
            List<Pair<int, int>> BuffValueNew = new List<Pair<int, int>>();
            int[] Mask = new int[12];
            Mask[0] |= -33554432;
            Mask[1] |= 512;
            Mask[5] |= 163840;

            for (int i = 0; i < Mask.Length; i++)
            {
                p.WriteInt(Mask[i]);
            }
            foreach (Pair<int, int> i in BuffValue)
            {
                if (i.Right == 3)
                {
                    p.WriteInt(i.Left);
                }
                else if (i.Right == 2)
                {
                    p.WriteShort((short)i.Left);
                }
                else if (i.Right == 1)
                {
                    p.WriteByte((byte)i.Left);
                }
            }
            p.WriteInt(-1);
            if (BuffValueNew.Count < 1)
            {
                p.WriteZero(10);
            }
            else
            {
                p.WriteByte(0);
                foreach (Pair<int, int> i in BuffValueNew)
                {
                    if (i.Right == 4)
                    {
                        p.WriteInt(i.Left);
                    }
                    else if (i.Right == 2)
                    {
                        p.WriteShort((short)i.Left);
                    }
                    else if (i.Right == 1)
                    {
                        p.WriteByte((byte)i.Left);
                    }
                    else if (i.Right == 0)
                    {
                        p.WriteZero((byte)i.Left);
                    }
                }
            }

            p.WriteZero(20);

            int Magic_Spawn = new Random().Next();

            p.WriteByte(1);
            p.WriteInt(Magic_Spawn);
            p.WriteZero(10);
            p.WriteByte(1);
            p.WriteInt(Magic_Spawn);
            p.WriteZero(10);
            p.WriteByte(1);
            p.WriteInt(Magic_Spawn);
            p.WriteShort(0);

            // TODO: Mounts.

            p.WriteLong(0L);

            p.WriteByte(1);
            p.WriteInt(Magic_Spawn);
            p.WriteLong(0L);

            p.WriteByte(1);
            p.WriteInt(Magic_Spawn);
            p.WriteZero(15);

            p.WriteByte(1);
            p.WriteInt(Magic_Spawn);
            p.WriteZero(16);

            p.WriteByte(1);
            p.WriteInt(Magic_Spawn);
            p.WriteShort(0);

            p.WriteShort(pCharacter.Job);
            p.WriteShort(pCharacter.Subcategory);
            HelpPacket.AddCharacterLooks(p, pCharacter, true);
            p.WriteZero(8);

            p.WriteInt(0); // Valentine effect. TODO.
            p.WriteZero(20);
            // Quest 124000.
            p.WriteInt(0);
            p.WriteZero(8);
            p.WriteInt(0);
            p.WriteInt(0); // TODO: Item effect.
            p.WriteInt(0); // TODO: Chair.
            p.WriteInt(0);
            p.WritePosition(pCharacter.Position);
            p.WriteByte(pCharacter.Stance);
            p.WriteShort(pCharacter.Foothold);
            p.WriteByte(0);
            p.WriteByte(0);
            p.WriteByte(0);
            p.WriteShort(1);

            // TODO: Mounts.
            p.WriteInt(0);
            p.WriteInt(0);
            p.WriteInt(0);

            p.WriteByte(0);

            p.WriteByte(0); // TODO: Chalkboard.
            //p.WriteMapleString(Chalkboard Text);

            p.WriteByte(0);
            p.WriteByte(0);

            p.WriteByte(0);

            p.WriteByte(0); // TODO: Berserk.
            p.WriteInt(0);

            p.WriteMapleString("Creating...");
            p.WriteInt(0); // Waru.
            p.WriteInt(0); // Level
            p.WriteInt(0); // EXP
            p.WriteInt(0); // AestheticPoints
            p.WriteInt(0); // Gems
            p.WriteInt(0); // ?
            p.WriteZero(5);
            p.WriteInt(0);

            p.WriteByte(0xFF);
            p.WriteInt(0xFF);

            p.WriteZero(9);

            return p;
        }

        public static Packet CharacterLeave(int pCharacterId)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Player_Despawn);
            p.WriteInt(pCharacterId);

            return p;
        }

        public static Packet PlayerChat(int pCharacterId, string pMessage, bool pWhiteBackground, byte pShow)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Player_Chat);
            p.WriteInt(pCharacterId);
            p.WriteBool(pWhiteBackground);
            p.WriteMapleString(pMessage);
            p.WriteByte(pShow);

            return p;
        }

        public static Packet PlayerMove(int pCharacterId, Packet pPacket, Pos pStartPos)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Player_Movement);
            p.WriteInt(pCharacterId);
            p.WriteInt(0);
            p.WritePosition(pStartPos);
            p.WriteHexString("25 FD D0 77");
            p.WriteByte(1);

            return p;
        }

        public static Packet SpawnNPC(Life pNPC)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.NPC_Spawn);
            p.WriteUInt(pNPC.SpawnID);
            p.WriteInt(pNPC.ID);
            p.WriteShort(pNPC.X);
            p.WriteShort(pNPC.Cy);
            p.WriteByte((byte)(pNPC.FacesLeft ? 0 : 1));
            p.WriteUShort(pNPC.Foothold);
            p.WriteShort(pNPC.Rx0);
            p.WriteShort(pNPC.Rx1);
            p.WriteByte((byte)(pNPC.Hide == 0 ? 1 : 0));

            return p;
        }
    }
}