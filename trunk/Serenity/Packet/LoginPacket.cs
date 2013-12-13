using Serenity.Servers;
using Serenity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets
{
    public static class LoginPacket
    {
        public static Packet Login(Client pClient, long pSessionId)
        {
            Packet p = new Packet();

            p.WriteByte((byte)SendOpcodes.Login_Status);
            p.WriteByte(0);
            p.WriteByte(0);
            p.WriteInt(0);

            p.WriteInt(pClient.Account.Id);
            p.WriteByte(0);
            p.WriteBool(false); // TODO: Admin.
            p.WriteByte(1);
            p.WriteMapleString(pClient.Account.Username);
            p.WriteInt(0);
            p.WriteInt(0);
            p.WriteInt(0);

            return p;
        }

        public static Packet LoginStatus(byte pReason)
        {
            Packet p = new Packet();

            p.WriteByte((byte)SendOpcodes.Login_Status);
            p.WriteByte(pReason);
            p.WriteByte(0);
            p.WriteInt(0);

            return p;
        }

        public static Packet LoginBackground(Dictionary<string, int> pBackgrounds)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Login_Background);

            foreach (KeyValuePair<string, int> Background in pBackgrounds)
            {
                p.WriteMapleString(Background.Key);
                p.WriteByte((byte)Background.Value);
            }

            return p;
        }

        public static Packet Worldlist(World pWorld, int[] pLoads)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.World_Information);

            p.WriteByte(pWorld.Id);
            p.WriteMapleString(pWorld.Name);
            p.WriteByte(2); // TODO: Flag.
            p.WriteMapleString(Constants.EventMessage);
            p.WriteShort(100);
            p.WriteShort(100);
            p.WriteByte(0);
            p.WriteByte((byte)pLoads.Length);

            int Id = 1;

            foreach (int Load in pLoads)
            {
                Console.WriteLine("Load: " + Load);
                p.WriteMapleString(String.Format("{0}-{1}", pWorld.Name, Id));
                p.WriteInt(Load * 200);
                p.WriteByte(pWorld.Id);
                p.WriteShort((short)(Id - 1));

                Id++;
            }


            p.WriteShort(0);
            p.WriteInt(0);
            p.WriteByte(0);

            return p;
        }

        public static Packet WorldlistEnd()
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.World_Information);
            p.WriteShort(255);

            return p;
        }

        /// <summary>
        /// 0 - Normal
        /// 1 - Highly populated
        /// 2 - Full
        /// </summary>
        /// <param name="pStatus">The status to display</param>
        public static Packet UserLimitResult(int pStatus)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.World_Status);
            p.WriteShort((short)pStatus);

            return p;
        }

        public static Packet WorldSelectResult(Client pClient)
        {
            return new Packet();
        }

        public static Packet CheckNameResult(string pName, bool pUsed)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Check_Name_Result);
            p.WriteMapleString(pName);
            p.WriteByte((byte)(pUsed ? 1 : 0));

            return p;
        }

        public static Packet CreateCharacterResult(Character pCharacter, bool pValid)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Create_Character_Result);
            p.WriteByte((byte)(pValid ? 0 : 1));
            HelpPacket.AddCharacterImage(p, pCharacter, false, false);

            return p;
        }

        public static Packet InvalidPIC()
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.PIC_Error);
            p.WriteByte(0x14);
            p.WriteByte(0);

            return p;
        }

        public static Packet Migrate(byte[] pIP, ushort pPort, int pCharacterId)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Migrate);
            p.WriteShort(0);
            p.WriteBytes(pIP);
            p.WriteUShort(pPort);
            p.WriteInt(pCharacterId);
            p.WriteInt(0);
            p.WriteShort(0);
            p.WriteBytes(new byte[] { 0x61, 0x31, 0x20, 0x6D, 0x73, 0x5D, 0x20, 0x5B, 0x47 });

            return p;
        }
    }
}
