using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets.Handlers
{
    public static class CashShopHandler
    {
        public static void HandleEnter(Client pClient, Packet pPacket)
        {
            pClient.SendPacket(CashShopPacket.Cash_Shop_Enter(pClient));
            pClient.SendPacket(CashShopPacket.Cash_Shop_Disable());
            pClient.SendPacket(CashShopPacket.Cash_Shop_Categories());
            pClient.SendPacket(CashShopPacket.Cash_Shop_Top_Items());
            pClient.SendPacket(CashShopPacket.Cash_Shop_Picture_Items());
            pClient.SendPacket(CashShopPacket.Cash_Shop_Special_Items());
            pClient.SendPacket(CashShopPacket.Cash_Shop_Featured_Items());
            pClient.SendPacket(CashShopPacket.Cash_Shop_Update(pClient));

            HelpPacket.CashShopPackets(pClient);
        }
    }
}