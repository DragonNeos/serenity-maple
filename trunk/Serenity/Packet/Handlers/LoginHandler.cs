using Serenity;
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
    public static class GeneralHandler
    {
        public static void HandleValidate(Client pClient, Packet pPacket)
        {
            byte Locale = pPacket.ReadByte();
            short Major = pPacket.ReadShort();
            short Minor = pPacket.ReadShort();

            Console.WriteLine("Handshake received, Locale: {0}, Major: {1}, Minor: {2}.", Locale, Major, Minor);

            //if (Locale != 8 || Major != 141 || Minor != 2)
            //{
            //    pClient.Disconnect();
            //}
        }

        public static void HandleHeartbeat(Client pClient, Packet pPacket)
        {
            int Request = pPacket.ReadInt();

            pClient.SendPacket(GeneralPacket.HeartbeatResponse(Request));
        }
    }

    public static class LoginHandler
    {
        public static void HandleStart(Client pClient, Packet pPacket)
        {
            //pClient.Account = new Account();
            //pClient.Account.Username = "admin";
            //pClient.Account.LoggedIn = 1;
            //pClient.Account.Load();

            //pClient.SendPacket(LoginPacket.Login(pClient, pClient.SessionId));
        }

        public static void HandleLogin(Client pClient, Packet pPacket)
        {
            string Username = pPacket.ReadMapleString().Trim();
            string Password = pPacket.ReadMapleString();

            pClient.Account = new Account();
            pClient.Account.Username = Username;
            pClient.Account.Load();

            if (pClient.Account != null)
            {
                pClient.Account.LoggedIn = 1;
                pClient.SendPacket(LoginPacket.Login(pClient, pClient.SessionId));
            }
            else
            {
                pClient.SendPacket(LoginPacket.LoginStatus(1));
            }
        }

        public static void HandleWorldsRequest(Client pClient, Packet pPacket)
        {
            if (pClient.Account.Characters.Count > 0)
            {
                pClient.Account.Characters.Clear();
            }
            
            foreach (World World in Master.Instance.Worlds)
            {
                var Loads = World.GetChannelLoads();

                pClient.SendPacket(LoginPacket.Worldlist(World, Loads));
            }

            pClient.SendPacket(LoginPacket.WorldlistEnd());
        }

        public static void HandleCheckUserLimit(Client pClient, Packet pPacket)
        {
            if (pClient.Account.LoggedIn != 1) return;

            byte WorldId = pPacket.ReadByte();

            if (Master.Instance.Worlds[WorldId] == null)
            {
                pClient.Session.Disconnect();
                return;
            }

            int CurrentLoad = Master.Instance.Worlds[WorldId].CurrentLoad;

            if (CurrentLoad >= 100)
            { // TODO: Max players.
                pClient.SendPacket(LoginPacket.UserLimitResult(2));
            }
            else if (CurrentLoad * 2 >= 100)
            {
                pClient.SendPacket(LoginPacket.UserLimitResult(1));
            }
            else
            {
                pClient.SendPacket(LoginPacket.UserLimitResult(0));
            }
        }

        public static void HandleWorldSelect(Client pClient, Packet pPacket)
        {
            if (pClient.Account.LoggedIn != 1) return;

            pPacket.Skip(1);

            pClient.World = pPacket.ReadByte();
            pClient.Channel = pPacket.ReadByte();

            pClient.Account.WorldId = pClient.World;
            pClient.Account.ChannelId = pClient.Channel;

            if (Master.Instance.Worlds[pClient.World] == null || Master.Instance.Worlds[pClient.World].Channels[pClient.Channel] == null)
            {
                pClient.Session.Disconnect();
                return;
            }

            if (pClient.Account.Characters.Count == 0)
            {
                pClient.Account.LoadCharacters();

                foreach (Character Character in pClient.Account.Characters)
                {
                    if (!Character.Loaded)
                        Character.Load();
                }
            }

            pClient.SendPacket(LoginPacket.WorldSelectResult(pClient));
        }

        public static void HandleCheckNameDuplicate(Client pClient, Packet pPacket)
        {
            if (pClient.Account.LoggedIn != 1) return;

            string Name = pPacket.ReadMapleString();

            if (Master.Instance.Accessor.NameAvailable(Name))
                pClient.SendPacket(LoginPacket.CheckNameResult(Name, false));
            else
                pClient.SendPacket(LoginPacket.CheckNameResult(Name, true));
        }

        public static void HandleCreateCharacter(Client pClient, Packet pPacket)
        {
            if (pClient.Account.LoggedIn != 1) { return; }

            List<Equip> equips = new List<Equip>();

            string name = pPacket.ReadMapleString();
            pPacket.ReadInt();

            int startMap = 0;

            int jobType = pPacket.ReadInt();
            short jobSubtype = pPacket.ReadShort();
            byte gender = pPacket.ReadByte();
            byte skin = pPacket.ReadByte();
            pPacket.ReadByte();
            int face = pPacket.ReadInt();
            int hair = pPacket.ReadInt();
            int hairColor = -1, faceMark = -1, topId, bottomId = -1, capeId = -1, shoesId, weaponId, shieldId = -1;

            if (Constants.CharacterCreation.hasHairColor(jobType))
                hairColor = pPacket.ReadInt();
            if (Constants.CharacterCreation.hasSkinColor(jobType))
                pPacket.ReadInt();
            if (Constants.CharacterCreation.hasFaceMark(jobType))
                faceMark = pPacket.ReadInt();
            if (Constants.CharacterCreation.hasHat(jobType))
            {
                int capId = pPacket.ReadInt();
                Equip cap = new Equip();
                cap.ItemID = capId;
                cap.GiveStats(false);
                cap.InventorySlot = -1;

                equips.Add(cap);
            }
            topId = pPacket.ReadInt();
            Equip top = new Equip();
            top.ItemID = topId;
            top.GiveStats(false);
            top.InventorySlot = -5;

            equips.Add(top);
            if (Constants.CharacterCreation.hasBottom(jobType))
            {
                bottomId = pPacket.ReadInt();
                Equip bottom = new Equip();
                bottom.ItemID = bottomId;
                bottom.GiveStats(false);
                bottom.InventorySlot = -6;

                equips.Add(bottom);
            }
            if (Constants.CharacterCreation.hasCape(jobType))
            {
                capeId = pPacket.ReadInt();
                Equip cape = new Equip();
                cape.ItemID = capeId;
                cape.GiveStats(false);
                cape.InventorySlot = -9;

                equips.Add(cape);
            }
            shoesId = pPacket.ReadInt();
            Equip shoes = new Equip();
            shoes.ItemID = shoesId;
            shoes.GiveStats(false);
            shoes.InventorySlot = -7;

            equips.Add(shoes);
            weaponId = pPacket.ReadInt();
            Equip weapon = new Equip();
            weapon.ItemID = weaponId;
            weapon.GiveStats(false);
            weapon.InventorySlot = -11;

            equips.Add(weapon);
            if (jobType == (int)Constants.CharacterCreation.CreateTypes.Demon)
            {
                shieldId = pPacket.ReadInt();
                Equip shield = new Equip();
                shield.ItemID = shieldId;
                shield.GiveStats(false);
                shield.InventorySlot = -10;

                equips.Add(shield);
            }
            Character newChr = new Character();

            newChr.Client = pClient;
            newChr.AccountId = pClient.Account.Id;
            newChr.WorldId = pClient.World;

            newChr.Name = name;

            newChr.MapId = Constants.CharacterCreation.GetMapByType(jobType);
            if (hairColor < 0) hairColor = 0;
            if (jobType != (short)Constants.CharacterCreation.CreateTypes.Mihile)
            {
                hair += hairColor;
            }
            newChr.HairId = hair;
            newChr.FaceId = face;
            newChr.SkinColor = (byte)skin;
            newChr.Gender = gender;
            if (faceMark < 0) faceMark = 0;
            newChr.FaceMarking = faceMark;

            newChr.Job = Constants.CharacterCreation.GetJobByType(jobType);
            newChr.Level = 1;
            newChr.HP = 50;
            newChr.MaxHP = 50;
            newChr.MP = 50;
            newChr.MaxMP = 50;
            newChr.Str = 4;
            newChr.Dex = 4;
            newChr.Int = 4;
            newChr.Luk = 4;

            newChr.ApHp = 0;

            newChr.MapId = startMap;

            pClient.Account.Characters.Add(newChr);

            try
            {
                newChr.Create();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating character for {0}, Exception: {1}.", pClient.Session.IP, ex.Message);
                pClient.Session.Disconnect();
                return;
            }

            newChr.Id = Master.Instance.Accessor.GetLastId();

            Master.Instance.Accessor.AppendInventories(newChr, true);
            Master.Instance.Accessor.LoadInventoriesId(newChr);
            Master.Instance.Accessor.CreateEquips(newChr, equips);

            foreach (KeyValuePair<InventoryType, Inventory> inv in newChr.Inventory)
                inv.Value.Save();

            //TODO: Check for forbidden names and stuff like so.
            pClient.SendPacket(LoginPacket.CreateCharacterResult(newChr, true));
        }

        public static void HandleSelectCharacter(Client pClient, Packet pPacket)
        {
            if (pClient.Account.LoggedIn != 1) return;

            int CharacterId = pPacket.ReadInt();

            MigrateClient(pClient, CharacterId);
        }

        private static void MigrateClient(Client pClient, int pCharacterId)
        {
            int Count = pClient.Account.Characters.Count((Character) => Character.Id == pCharacterId);

            if (Count != 1)
            {
                pClient.Session.Disconnect();
                return;
            }

            byte[] IP = new byte[] { 8, 31, 99, 141 };
            ushort Port = Master.Instance.Worlds[pClient.World].Channels[pClient.Channel].Port;

            Master.Instance.Worlds[pClient.World].AddMigrationRequest(pCharacterId, pClient.SessionId,false);

            pClient.SendPacket(LoginPacket.Migrate(IP, Port, pCharacterId));
        }
    }
}
