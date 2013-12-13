using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Game.Objects
{
    public class Item // TODO: Full inhereit!
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int ItemID { get; set; }
        public byte InventorySlot { get; set; }
        public short Quantity { get; set; }
        public int AccountId { get; set; }
        public int PackageId { get; set; }
        public string Owner { get; set; }
        public string Log { get; set; }
        public long UniqueId { get; set; }
        public int Flag { get; set; }
        public long Expiration { get; set; }
        public string Sender { get; set; }

        public Item()
        {
            InventoryId = 0;
            ItemID = 0;
            Quantity = 0;
            AccountId = 0;
            PackageId = 0;
            Owner = "";
            Log = "";
            UniqueId = 0;
            Flag = 0;
            Expiration = 150842304000000000L;
            Sender = "";
        }

        public Item(Item pBaseItem)
        {
            ItemID = pBaseItem.ItemID;
            InventoryId = pBaseItem.InventoryId;
            InventorySlot = pBaseItem.InventorySlot;
            Quantity = pBaseItem.Quantity;
            AccountId = pBaseItem.AccountId;
            PackageId = pBaseItem.PackageId;
            Owner = pBaseItem.Owner;
            Log = pBaseItem.Log;
            UniqueId = pBaseItem.UniqueId;
            Flag = pBaseItem.Flag;
            Expiration = pBaseItem.Expiration;
            Sender = pBaseItem.Sender;
        }

    }
}
