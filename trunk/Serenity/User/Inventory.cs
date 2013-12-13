using Serenity.Game.Objects;
using Serenity.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.User
{
      public enum InventoryType
    {
        UNDEFINED,
        EQUIP,
        USE,
        SETUP,
        ETC,
        CASH ,
        EVAN,
        MECHANIC,
        HAKU,
        ANDROID,
        BITS,
        TOTEMS,
        EQUIPPED = -1
    }

      public class Inventory
      {
          public int Id { get; set; }

          public Dictionary<byte, Item> Items;
          public Dictionary<short, Equip> Equips;
          public Dictionary<int, short> ItemAmounts;

          public Character Owner { get; private set; }

          public byte SlotLimit { get; set; }
          public InventoryType InventoryType { get; private set; }

          public Inventory(Character pOwner, InventoryType invtype, byte slotLimit = 96)
          {
              Items = new Dictionary<byte, Item>(slotLimit);
              Equips = new Dictionary<short, Equip>();

              if ((int)invtype >= 2 || (int)invtype <= 5)
                  ItemAmounts = new Dictionary<int, short>();

              SlotLimit = slotLimit;
              InventoryType = invtype;
              Owner = pOwner;
          }

          public void Load()
          {
              Master.Instance.Accessor.LoadInventory(this);
          }

          public void Save()
          {
              Master.Instance.Accessor.SaveInventory(this);
          }
      }
}
