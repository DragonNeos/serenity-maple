using MySql.Data.MySqlClient;
using Serenity.Game.Objects;
using Serenity.User;
using Serenity.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serenity.Servers;

namespace Serenity.Database
{
    public class Accessor
    {
        private MySQL_Connection Database;

        public Accessor()
        {
            Database = new MySQL_Connection("root", "", "serenity", "localhost");
        }

        /// <summary>
        /// Inserts a summarized insert query.
        /// </summary>
        /// <param name="pTable">The name of the table to insert</param>
        /// <param name="pWhereVariable">The name of the WHERE variable</param>
        /// <param name="pWhereValue">THE value of the WHERE variable</param>
        /// <param name="pObjects">The objects to insert, format: Variables:Values:End</param>
        public void InsertQuery(string pTable, string pWhereVariable = "", string pWhereValue = "", params object[] pObjects)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO ");
            sb.Append(pTable);
            sb.Append("(");

            foreach (object str in pObjects)
            {
                if (str.Equals(":"))
                {
                    sb.Append(")");
                    sb.Remove(sb.Length - 3, 2);
                    if (!sb.ToString().Contains("VALUES"))
                        sb.Append(" VALUES(");
                    else
                    {
                        if (pWhereVariable != "" && pWhereValue != "")
                        {
                            sb.Append(String.Format(" WHERE {0} = {1}", pWhereVariable, pWhereValue));
                        }

                    }
                }
                else
                    sb.Append(str + ", ");
            }

            Database.RunQuery(sb.ToString());
        }

        /// <summary>
        /// Inserts a summarized updating query.
        /// </summary>
        /// <param name="pTable">The name of the table to update</param>
        /// <param name="pWhereVariable">The name of the WHERE variable</param>
        /// <param name="pWhereValue">The value of the WHERE variable</param>
        /// <param name="pObjects">The objects to update, format: Variables:Values@End</param>
        public void UpdateQuery(string pTable, string pWhereVariable = "", string pWhereValue = "", params object[] pObjects)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE ");
            sb.Append(pTable);
            sb.Append("SET ");

            bool Value = false;

            foreach (object str in pObjects)
            {
                if (str.Equals(":"))
                {
                    Value = true;
                    continue;
                }

                if (str.Equals("@"))
                {
                    if (pWhereValue != "" && pWhereValue != "")
                    {
                        sb.Append(String.Format(" WHERE {0} = {1}", pWhereVariable, pWhereValue));
                    }
                }

                if (Value)
                {
                    sb.Append(str + ", ");
                    Value = false;
                }
                else
                {
                    sb.Append(str + " = ");
                }
            }
        }

        /// <summary>
        /// Inserts a summarized deleting query.
        /// </summary>
        /// <param name="pTable">The name of the table to delete</param>
        /// <param name="pWhereVariable">The name of the WHERE variabl</param>
        /// <param name="pWhereValue">The valu of the WHERE variable</param>
        public void DeleteQuery(string pTable, string pWhereVariable = "", string pWhereValue = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE * FROM ");
            sb.Append(pTable);

            if (pWhereVariable != "" && pWhereValue != "")
                sb.Append(String.Format(" WHERE {0} = {1}", pWhereVariable, pWhereValue));
        }

        /// <summary>
        /// Loads the given account from the database.
        /// </summary>
        /// <param name="pAccount">The account to load</param>
        public void LoadAccount(Account pAccount)
        {
            Database.RunQuery("SELECT * FROM accounts WHERE username = '" + pAccount.Username + "'");
            MySqlDataReader Reader = Database.Reader;

            Reader.Read();
            if (!Reader.HasRows)
            {
                pAccount = null;
                return;
            }

            pAccount.Id = Reader.GetInt32("id");
            pAccount.Password = Reader.GetString("password");
            pAccount.Gender = Reader.GetByte("gender");

            pAccount.Characters = new List<Character>(6); // TODO: Max slots.

            Reader.Close();
        }

        /// <summary>
        /// Adds empty characters objects to the account.
        /// </summary>
        /// <param name="pAccount">The account to load characters of</param>
        public void LoadCharacters(Account pAccount)
        {
            Database.RunQuery("SELECT * FROM characters WHERE accountId = " + pAccount.Id + " AND world = " + pAccount.WorldId);
            MySqlDataReader Reader = Database.Reader;

            Reader.Read();
            if (!Reader.HasRows)
            {
                pAccount.Characters.Clear();
                return;
            }

            while (Reader.Read())
            {
                Character Character = new Character();
                Character.AccountId = pAccount.Id;
                Character.Id = Reader.GetInt32("id");

                pAccount.Characters.Add(Character);
            }

            Reader.Close();
        }

        /// <summary>
        /// Loads the given character from the database.
        /// </summary>
        /// <param name="pCharacter">The character to load</param>
        public void LoadCharacter(Character pCharacter)
        {
            Database.RunQuery("SELECT * FROM characters WHERE id = " + pCharacter.Id);
            MySqlDataReader Reader = Database.Reader;

            Reader.Read();
            if (!Reader.HasRows)
            {
                pCharacter = null;
                return;
            }

            Reader.Read();

            pCharacter.WorldId = Reader.GetByte("world");
            pCharacter.Name = Reader.GetString("name");
            pCharacter.Level = Reader.GetByte("level");
            pCharacter.EXP = Reader.GetInt64("exp");
            pCharacter.Str = Reader.GetInt16("str");
            pCharacter.Dex = Reader.GetInt16("dex");
            pCharacter.Int = Reader.GetInt16("int");
            pCharacter.Luk = Reader.GetInt16("luk");
            pCharacter.HP = Reader.GetInt32("hp");
            pCharacter.MP = Reader.GetInt32("mp");
            pCharacter.MaxHP = Reader.GetInt32("maxHp");
            pCharacter.MaxMP = Reader.GetInt32("maxMp");
            pCharacter.Meso = Reader.GetInt64("meso");
            pCharacter.ApHp = Reader.GetInt16("hpApUsed");
            pCharacter.Job = Reader.GetInt16("job");
            pCharacter.SkinColor = Reader.GetByte("skinColor");
            pCharacter.Gender = Reader.GetByte("gender");
            pCharacter.Fame = Reader.GetInt32("fame");
            pCharacter.HairId = Reader.GetInt32("hair");
            pCharacter.FaceId = Reader.GetInt32("face");
            pCharacter.FaceMarking = Reader.GetInt32("faceMarking");
            pCharacter.AP = Reader.GetInt16("ap");
            
            pCharacter.MapId = Reader.GetInt32("map");
            if (!Master.Instance.DataProvider.Maps.ContainsKey(pCharacter.MapId))
            {
                pCharacter.MapId = 0;
                pCharacter.MapPosition = 0;
            }
            else
            {
                if (Master.Instance.DataProvider.Maps[pCharacter.MapId].ForcedReturn != 999999999)
                {
                    pCharacter.MapId = Master.Instance.DataProvider.Maps[pCharacter.MapId].ForcedReturn;
                    pCharacter.MapPosition = 0;
                }
                else
                {
                    pCharacter.MapPosition = (byte)Reader.GetInt16("pos");
                }
            }

            Random Random = new Random();
            Portal Portal = Master.Instance.DataProvider.Maps[pCharacter.MapId].SpawnPoints[Random.Next(0, Master.Instance.DataProvider.Maps[pCharacter.MapId].SpawnPoints.Count)];

            pCharacter.Position = new Pos(Portal.X, (short)(Portal.Y - 40));
            pCharacter.Stance = 0;
            pCharacter.Foothold = 0;

            pCharacter.Loaded = true;

            Reader.Close();

            AppendInventories(pCharacter, false);
        }

        /// <summary>
        /// Loads the given character.
        /// </summary>
        /// <param name="pCharacter">The character to load</param>
        public void SaveCharacter(Character pCharacter)
        {
            MySqlDataReader Reader = Database.Reader;

            if (!Reader.IsClosed) Reader.Close();

            UpdateQuery("characters", "id", Convert.ToString(pCharacter.Id), "level", ":", pCharacter.Level, "exp", ":", pCharacter.EXP, "str", ":", pCharacter.Str, "dex", ":", pCharacter.Dex, "`int`", ":", pCharacter.Int, "luk", ":", pCharacter.Luk, "hp", ":", pCharacter.HP, "mp", ":", pCharacter.MP, "maxHp", ":", pCharacter.MaxHP, "maxMp", ":", pCharacter.MaxMP, "meso", ":", pCharacter.Meso, "job", ":", pCharacter.Job, "skinColor", ":", pCharacter.SkinColor, "fame", ":", pCharacter.Fame, "hair", ":", pCharacter.HairId, "face", ":", pCharacter.FaceId, "faceMarking", ":", pCharacter.FaceMarking, "ap", ":", pCharacter.AP, "map", ":", pCharacter.MapId, "pos", ":", pCharacter.MapPosition, "@");

            foreach (KeyValuePair<InventoryType, Inventory> Inventory in pCharacter.Inventory)
                SaveInventory(Inventory.Value);
        }

        /// <summary>
        /// Creates a new character in the database according to object's values.
        /// </summary>
        /// <param name="pCharacter"></param>
        public void CreateCharacter(Character pCharacter)
        {
            InsertQuery("characters", "", "","accountId", "world", "name", "level", "exp", "str", "dex", "`int`", "luk", "hp", "mp", "maxHp", "maxMp", "meso", "hpApUsed", "job", "skinColor", "gender", "fame", "hair", "face", "faceMarking", "ap", "map", "pos", ":", pCharacter.AccountId, pCharacter.WorldId, "'" + pCharacter.Name + "'", pCharacter.Level, pCharacter.EXP, pCharacter.Str, pCharacter.Dex, pCharacter.Int, pCharacter.Luk, pCharacter.HP, pCharacter.MP, pCharacter.MaxHP, pCharacter.MaxMP, pCharacter.Meso, pCharacter.ApHp, pCharacter.Job, pCharacter.SkinColor, pCharacter.Gender, pCharacter.Fame, pCharacter.HairId, pCharacter.FaceId, pCharacter.FaceMarking, pCharacter.AP, pCharacter.MapId, pCharacter.MapPosition, ":");
        }

        /// <summary>
        /// Creates new equips for a new character.
        /// </summary>
        /// <param name="pCharacter">The character to create equips to</param>
        /// <param name="pEquips">The list of equips to create</param>
        public void CreateEquips(Character pCharacter, List<Equip> pEquips)
        {
            if (pCharacter.Inventory.Count == 0)
                return;

            foreach (Equip Equip in pEquips)
            {
                Equip.InventoryId = pCharacter.Inventory[InventoryType.EQUIPPED].Id;
                pCharacter.Inventory[InventoryType.EQUIPPED].Equips.Add(Math.Abs(Equip.InventorySlot), Equip);
            }
        }

        /// <summary>
        /// Loads a character's inventories's Ids.
        /// </summary>
        /// <param name="pCharacter">The character to load inventories's Ids to</param>
        public void LoadInventoriesId(Character pCharacter)
        {
            Database.RunQuery("SELECT * FROM inventories WHERE characterId = " + pCharacter.Id);
            MySqlDataReader Reader = Database.Reader;

            while (Reader.Read())
            {
                int id = Reader.GetInt32(0);
                string type = Reader.GetString("type");
                byte slotlimit = Reader.GetByte("slotlimit");

                pCharacter.Inventory[(InventoryType)Enum.Parse(typeof(InventoryType), type)].Id = id;
            }

            Reader.Close();
        }

        /// <summary>
        /// Loads an inventory.
        /// </summary>
        /// <param name="pInventory">The inventory to load</param>
        public void LoadInventory(Inventory pInventory)
        {
            Database.RunQuery("SELECT * FROM inventories WHERE characterId = " + pInventory.Owner.Id + " AND type = '" + pInventory.InventoryType.ToString() + "'");
            MySqlDataReader Reader = Database.Reader;

            Reader.Read();
            if (!Reader.HasRows)
            {
                pInventory = null;
                return;
            }

            pInventory.Id = Reader.GetInt32("id");
            pInventory.SlotLimit = Reader.GetByte("slotLimit");

            Reader.Close();

            Database.RunQuery("SELECT * FROM inventoryequips WHERE inventoryId = " + pInventory.Id);
            Reader = Database.Reader;

            while (Reader.Read())
            {
                Equip Equip = new Equip();
                Equip.Id = Reader.GetInt32(0);
                Equip.InventoryId = Reader.GetInt32("inventoryid");
                Equip.ItemID = Reader.GetInt32("itemid");
                Equip.InventorySlot = Reader.GetInt16("slot");
                Equip.Scrolls = Reader.GetByte("upgradeslots");
                Equip.Level = Reader.GetByte("level");
                Equip.Str = Reader.GetInt16("str");
                Equip.Dex = Reader.GetInt16("dex");
                Equip.Int = Reader.GetInt16("int");
                Equip.Luk = Reader.GetInt16("luk");
                Equip.HP = Reader.GetInt16("hp");
                Equip.MP = Reader.GetInt16("mp");
                Equip.Watk = Reader.GetInt16("watk");
                Equip.Matk = Reader.GetInt16("matk");
                Equip.Wdef = Reader.GetInt16("wdef");
                Equip.Mdef = Reader.GetInt16("mdef");
                Equip.Acc = Reader.GetInt16("acc");
                Equip.Avo = Reader.GetInt16("avoid");
                Equip.Hands = Reader.GetInt16("hands");
                Equip.Speed = Reader.GetInt16("speed");
                Equip.Jump = Reader.GetInt16("jump");
                Equip.Locked = Reader.GetByte("locked") > 0;
                Equip.ViciousHammer = Reader.GetByte("vicioushammer");
                Equip.ItemLevel = Reader.GetByte("itemlevel");
                Equip.ItemEXP = Reader.GetInt32("itemexp");
                Equip.Expiration = Reader.GetInt64("expiration");

                if (!pInventory.Equips.ContainsKey(Equip.InventorySlot))
                    pInventory.Equips.Add(Equip.InventorySlot, Equip);
            }

            Reader.Close();

            Database.RunQuery("SELECT * FROM inventoryitems WHERE inventoryId = " + pInventory.Id);
            Reader = Database.Reader;

            while (Reader.Read())
            {
                Item Item = new Item();
                Item.Id = Reader.GetInt32("id");
                Item.InventoryId = Reader.GetInt32("inventoryId");
                Item.ItemID = Reader.GetInt32("itemId");
                Item.InventorySlot = Reader.GetByte("slot");
                Item.Quantity = Reader.GetInt16("quantity");

                pInventory.Items.Add(Item.InventorySlot, Item);
            }

            Reader.Close();
        }

        /// <summary>
        /// Saves an inventory.
        /// </summary>
        /// <param name="pInventory">The inventory to save</param>
        public void SaveInventory(Inventory pInventory)
        {
            DeleteQuery("inventories", "characterId", Convert.ToString(pInventory.Owner.Id));
            InsertQuery("inventories", "", "", "characterId", "type", "slotLimit", ":", Convert.ToString(pInventory.Owner.Id), "'" + pInventory.InventoryType.ToString() + "'", Convert.ToString(pInventory.SlotLimit), ":");

            DeleteQuery("inventoryequips", "inventoryId", Convert.ToString(pInventory.Id));

            foreach (KeyValuePair<short, Equip> Equip in pInventory.Equips)
                InsertQuery("inventoryequips", "", "", "inventoryId", "itemId", "slot", "upgradeSlots", "str", "dex", "`int`", "luk", "hp", "mp", "watk", "matk", "wdef", "mdef", "acc", "avoid", "hands", "speed", "jump", "locked", "viciousHammer", "itemLevel", "itemExp", "expiration", ":", Convert.ToString(pInventory.Id), Convert.ToString(Equip.Value.ItemID), Convert.ToString(Equip.Value.InventorySlot), Convert.ToString(Equip.Value.Slots), Convert.ToString(Equip.Value.Str), Convert.ToString(Equip.Value.Dex), Convert.ToString(Equip.Value.Int), Convert.ToString(Equip.Value.Luk), Convert.ToString(Equip.Value.HP), Convert.ToString(Equip.Value.MP), Convert.ToString(Equip.Value.Watk), Convert.ToString(Equip.Value.Matk), Convert.ToString(Equip.Value.Wdef), Convert.ToString(Equip.Value.Mdef), Convert.ToString(Equip.Value.Acc), Convert.ToString(Equip.Value.Avo), Convert.ToString(Equip.Value.Hands), Convert.ToString(Equip.Value.Speed), Convert.ToString(Equip.Value.Jump), Convert.ToString(Equip.Value.Locked), Convert.ToString(Equip.Value.ViciousHammer), Convert.ToString(Equip.Value.ItemLevel), Convert.ToString(Equip.Value.ItemEXP), Convert.ToString(Equip.Value.Expiration), ":");

            DeleteQuery("inventoryitems", "inventoryId", Convert.ToString(pInventory.Id));

            foreach (KeyValuePair<byte, Item> Item in pInventory.Items)
            {
                InsertQuery("inventoryitems", "", "", "inventoryId", "itemId", "slot", "quantity", "accountId", "packageId", "owner", "log", "uniqueId", "flag", "expiration", "sender", ":", Convert.ToString(Item.Value.Id), Convert.ToString(Item.Value.ItemID), Convert.ToString(Item.Value.InventorySlot), Convert.ToString(Item.Value.Quantity), Convert.ToString(Item.Value.PackageId), "'" + Convert.ToString(Item.Value.Owner) + "'", "'" + Convert.ToString(Item.Value.Log) + "'", Convert.ToString(Item.Value.UniqueId), Convert.ToString(Item.Value.Flag), Convert.ToString(Item.Value.Expiration), "'" + Convert.ToString(Item.Value.Sender) + "'", ":");
            }
        }

        /// <summary>
        /// Adds inventories to the given character and loads them.
        /// </summary>
        /// <param name="pCharacter">The character to append to.</param>
        public void AppendInventories(Character pCharacter, bool pNewCharacter)
        {
            if (pCharacter.Inventory.Count > 0)
                pCharacter.Inventory.Clear();

            pCharacter.Inventory.Add(InventoryType.EQUIP, new Inventory(pCharacter, InventoryType.EQUIP, 96));
            pCharacter.Inventory.Add(InventoryType.USE, new Inventory(pCharacter, InventoryType.USE, 96));
            pCharacter.Inventory.Add(InventoryType.SETUP, new Inventory(pCharacter, InventoryType.SETUP, 96));
            pCharacter.Inventory.Add(InventoryType.ETC, new Inventory(pCharacter, InventoryType.ETC, 96));
            pCharacter.Inventory.Add(InventoryType.CASH, new Inventory(pCharacter, InventoryType.CASH, 96));
            pCharacter.Inventory.Add(InventoryType.TOTEMS, new Inventory(pCharacter, InventoryType.TOTEMS, 2));
            pCharacter.Inventory.Add(InventoryType.ANDROID, new Inventory(pCharacter, InventoryType.ANDROID, 2));
            pCharacter.Inventory.Add(InventoryType.BITS, new Inventory(pCharacter, InventoryType.BITS, 4));
            pCharacter.Inventory.Add(InventoryType.EVAN, new Inventory(pCharacter, InventoryType.EVAN, 6));
            pCharacter.Inventory.Add(InventoryType.MECHANIC, new Inventory(pCharacter, InventoryType.MECHANIC, 6));
            pCharacter.Inventory.Add(InventoryType.HAKU, new Inventory(pCharacter, InventoryType.HAKU, 3));
            pCharacter.Inventory.Add(InventoryType.EQUIPPED, new Inventory(pCharacter, InventoryType.EQUIPPED, 50));

            if (!pNewCharacter)
                LoadInventoriesId(pCharacter);

            foreach (KeyValuePair<InventoryType, Inventory> inv in pCharacter.Inventory)
            {
                if (pNewCharacter)
                    inv.Value.Save();
                else
                    inv.Value.Load();
            }
        }

        /// <summary>
        /// Returns the last insereted ID.
        /// </summary>
        /// <returns></returns>
        public int GetLastId()
        {
            return Database.GetLastInsertId();
        }

        /// <summary>
        /// Returns if the name is available or not.
        /// </summary>
        /// <param name="pName">The name to check</param>
        /// <returns></returns>
        public bool NameAvailable(string pName)
        {
            Database.RunQuery("SELECT * FROM accounts WHERE username = '" + pName + "'");
            MySqlDataReader Reader = Database.Reader;

            if (!Reader.HasRows)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns the account with the given character id.
        /// </summary>
        /// <param name="pCharacterId"></param>
        /// <returns></returns>
        public Character GetCharacter(int pCharacterId)
        {
            Character Character = new Character();
            Character.Id = pCharacterId;

            LoadCharacter(Character);

            return Character;
        }

        /// <summary>
        /// Returns the account with the given account id.
        /// </summary>
        /// <param name="pAccountId"></param>
        /// <returns></returns>
        public Account GetAccount(int pAccountId)
        {
            Account Account = new Account();
            Account.Id = pAccountId;

            LoadAccount(Account);

            return Account;
        }
    }
}
