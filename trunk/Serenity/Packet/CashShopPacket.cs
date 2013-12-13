using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets
{
    public static class CashShopPacket
    {
        private static byte Opeartion_Code = 0x6D;

        public static Packet Cash_Shop_Enter(Client pClient)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop_Enter);
            HelpPacket.AddCharacterInformation(p, pClient.Character);
            p.WriteLong(1);
            p.WriteInt(0);
            p.WriteHexString("67 00 20 00 74 00 68 00 65 00 20 00 73 00 6B 00 69 00 6C 00 6C 00 2C 00 20 00 74 00 68 00 65 00 20 00 6E 00 65 00 61 00 72 00 62 00 79 00 20 00 70 00 72 00 65 00 73 00 65 00 74 00 2D 00 75 00 70 00 20 00 50 00 6F 00 69 00 73 00 6F 00 6E 00 20 00 4D 00 69 00 73 00 74 00 73 00 20 00 65 00 78 00 70 00 6C 00 6F 00 64 00 65 00 2C 00 20 00 64 00 65 00 61 00 6C 00 69 00 6E 00 67 00 20 00 66 00 61 00 74 00 61 00 6C 00 20 00 64 00 61 00 6D 00 61 00 67 00 65 00 20 00 74 00 6F 00 20 00 65 00 6E 00 65 00 6D 00 69 00 65 00 73 00 2E 00 20 00 54 00 68 00 65 00 20 00 64 00 61 00 6D 00 61 00 67 00 65 00 20 00 77 00 69 00 6C 00 6C 00 20 00 69 00 6E 00 63 00 72 00 65 00 61 00 73 00 65 00 20 00 70 00 72 00 6F 00 70 00 6F 00 72 00 74 00 69 00 6F 00 6E 00 61 00 74 00 65 00 20 00 74 00 6F 00 20 00 74 00 68 00 65 00 20 00 6E 00 75 00 6D 00 62 00 65 00 72 00 20 00 6F 00 66 00 20 00 63 00 6F 00 6E 00 74 00 69 00 6E 00 75 00 6F 00 75 00 73 00 20 00 64 00 61 00 6D 00 61 00 67 00 65 00 20 00 65 00 66 00 66 00 65 00 63 00 74 00 73 00 20 00 61 00 70 00 70 00 6C 00 69 00 65 00 64 00 20 00 74 00 6F 00 20 00 74 00 68 00 65 00 20 00 74 00 61 00 72 00 67 00 65 00 74 00 2E 00 20 00 59 00 6F 00 75 00 20 00 63 00 61 00 6E 00 6E 00 6F 00 74 00 20 00 64 00 65 00 74 00 6F 00 6E 00 61 00 74 00 65 00 20 00 4D 00 69 00 73 00 74 00 73 00 20 00 73 00 65 00 74 00 2D 00 75 00 70 00 20 00 62 00 79 00 20 00 6F 00 74 00 68 00 65 00 72 00 73 00 2E 00 20 00 54 00 68 00 65 00 20 00 64 00 61 00 6D 00 61 00 67 00 65 00 20 00 62 00 6F 00 6F 00 73 00 74 00 20 00 61 00 70 00 70 00 6C 00 69 00 65 00 73 00 20 00 75 00 70 00 20 00 74 00 6F 00 20 00 74 00 68 00 65 00 20 00 35 00 74 00 68 00 20 00 65 00 66 00 66 00 65 00 63 00 74 00 2E 00 5C 00 6E 00 52 00 65 00 71 00 75 00 69 00 72 00 65 00 64 00 20 00 53 00 6B 00 69 00 6C 00 6C 00 3A 00 20 00 23 00 63 00 50 00 6F 00 69 00 73 00 6F 00 6E 00 20 00 4D 00 69 00 73 00 74 00 20 00 4C 00 76 00 2E 00 20 00 32 00 30 00 23 00 00 00 49 00 01 48 98 15 5D 0E 34 02 00 00 4D 00 50 00 20 00 43 00 6F 00 73 00 74 00 3A 00 20 00 23 00 6D 00 70 00 43 00 6F 00 6E 00 2C 00 20 00 44 00 61 00 6D 00 61 00 67 00 65 00 3A 00 20 00 23 00 64 00 61 00 6D 00 61 00 67 00 65 00 25 00 2C 00 20 00 4D 00 61 00 78 00 20 00 45 00 6E 00 65 00 6D 00 69 00 65 00 73 00 20 00 48 00 69 00 74 00 3A 00 20 00 23 00 6D 00 6F 00 62 00 43 00 6F 00 75 00 6E 00 74 00 2C 00 20 00 4E 00 75 00 6D 00 62 00 65 00 72 00 20 00 6F 00 66 00 20 00 41 00 74 00 74 00 61 00 63 00 6B 00 73 00 3A 00 20 00 23 00 61 00 74 00 74 00 61 00 63 00 6B 00 43 00 6F 00 75 00 6E 00 74 00 2C 00 20 00 44 00 61 00 6D 00 61 00 67 00 65 00 20 00 4F 00 76 00 65 00 72 00 20 00 54 00 69 00 6D 00 65 00 3A 00 20 00 23 00 64 00 6F 00 74 00 25 00 20 00 64 00 61 00 6D 00 61 00 67 00 65 00 20 00 65 00 76 00 65 00 72 00 79 00 20 00 23 00 64 00 6F 00 74 00 49 00 6E 00 74 00 65 00 72 00 76 00 61 00 6C 00 20 00 73 00 65 00 63 00 20 00 66 00 6F 00 72 00 20 00 23 00 64 00 6F 00 74 00 54 00 69 00 6D 00 65 00 20 00 73 00 65 00 63 00 2C 00 20 00 46 00 72 00 65 00 65 00 7A 00 65 00 20 00 43 00 68 00 61 00 6E 00 63 00 65 00 3A 00 20 00 23 00 68 00 63 00 50 00 72 00 6F 00 70 00 25 00 2C 00 20 00 46 00 72 00 65 00 65 00 7A 00 65 00 20 00 44 00 75 00 72 00 61 00 74 00 69 00 6F 00 6E 00 3A 00 20 00 00 00 00 00 00 00 00 A1 00 00 00 00 10 57 68 63 2B B8 CE 01");

            return p;
        }

        public static Packet Cash_Shop_Disable()
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop_Use);
            p.WriteZero(5);

            return p;
        }

        public static Packet Cash_Shop_Categories()
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop);
            p.WriteByte(3);
            p.WriteByte(1);
            p.WriteByte(0);

            return p;
        }

        public static Packet Cash_Shop_Top_Items()
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop);
            p.WriteByte(5);
            p.WriteByte(3);
            p.WriteByte(0);

            return p;
        }

        public static Packet Cash_Shop_Picture_Items()
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop);
            p.WriteByte(4);
            p.WriteByte(3);
            p.WriteByte(0);

            return p;
        }

        public static Packet Cash_Shop_Special_Items()
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop);
            p.WriteByte(6);
            p.WriteByte(3);
            p.WriteByte(0);

            return p;
        }

        public static Packet Cash_Shop_Featured_Items()
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop);
            p.WriteByte(8);
            p.WriteByte(3);
            p.WriteByte(0);

            return p;
        }

        public static Packet Cash_Shop_Update(Client pClient)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop_Update);
            p.WriteInt(0); // NX Credit.
            p.WriteInt(0); // Maple Points.
            p.WriteInt(1000); // NX Prepaid.

            return p;
        }

        public static Packet Cash_Shop_Inventory(Client pClient)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop_Opeartion);
            p.WriteByte((byte)(Opeartion_Code + 3));
            p.WriteByte(0);
            p.WriteShort(0);
            p.WriteInt(0);
            p.WriteShort(16); // Slots
            p.WriteShort(6);
            p.WriteShort(0);
            p.WriteShort(1);

            return p;
        }

        public static Packet Cash_Shop_Magic()
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop_Opeartion);
            p.WriteByte((byte)(Opeartion_Code + 4));
            p.WriteInt(0);

            return p;
        }

        public static Packet Cash_Shop_Gifts(Client pClient)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop_Opeartion);
            p.WriteByte((byte)(Opeartion_Code + 6));
            p.WriteShort(0); // Gifts size.

            return p;
        }

        public static Packet Cash_Shop_Wishlist(Client pClient, bool pUpdate)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Cash_Shop_Opeartion);
            p.WriteByte((byte)(Opeartion_Code + (pUpdate ? 15 : 8)));

            return p;
        }
    }
}
