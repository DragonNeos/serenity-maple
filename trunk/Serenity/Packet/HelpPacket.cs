using Serenity.Data;
using Serenity.Game.Objects;
using Serenity.Other;
using Serenity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets
{
    public static class HelpPacket
    {
        public static void AddCharacterImage(Packet pPacket, Character pCharacter, bool pRanked, bool pViewAll)
        {
            AddCharacterStats(pPacket, pCharacter);
            AddCharacterLooks(pPacket, pCharacter, true);

            if (!pViewAll)
                pPacket.WriteByte(0);

            pPacket.WriteByte((byte)(pRanked ? 1 : 0));

            if (pRanked)
            {
                //TODO: Ranking Stats.
                pPacket.WriteInt(100);
                pPacket.WriteInt(100);
                pPacket.WriteInt(200);
                pPacket.WriteInt(200);
            }
        }

        public static void AddCharacterStats(Packet pPacket, Character pCharacter)
        {
            pCharacter.Subcategory = Constants.JobConstants.GetSubcategory(pCharacter.Job);

            pPacket.WriteInt(pCharacter.Id);
            pPacket.WritePaddedString(pCharacter.Name, 13);
            pPacket.WriteByte(pCharacter.Gender);
            pPacket.WriteByte(pCharacter.SkinColor);
            pPacket.WriteInt(pCharacter.FaceId);
            pPacket.WriteInt(pCharacter.HairId);
            pPacket.WriteZero(24);

            pPacket.WriteByte(pCharacter.Level);
            pPacket.WriteShort(pCharacter.Job);
            pPacket.WriteShort(pCharacter.Str);
            pPacket.WriteShort(pCharacter.Dex);
            pPacket.WriteShort(pCharacter.Int);
            pPacket.WriteShort(pCharacter.Luk);
            pPacket.WriteInt(pCharacter.HP);
            pPacket.WriteInt(pCharacter.MaxHP);
            pPacket.WriteInt(pCharacter.MP);
            pPacket.WriteInt(pCharacter.MaxMP);
            pPacket.WriteShort(pCharacter.AP);

            if (Constants.JobConstants.isSeparatedSp(pCharacter.Job))
            {
                byte length = (byte)pCharacter.SP.Count((b) => b > 0);

                pPacket.WriteByte(length);

                foreach (int i in pCharacter.SP)
                {
                    if (i > 0)
                    {
                        pPacket.WriteByte((byte)(i + 1));
                        pPacket.WriteInt(pCharacter.SP[i]);
                    }
                }
            }
            else
            {
                pPacket.WriteShort(pCharacter.SP[0]);
            }

            pPacket.WriteLong(pCharacter.EXP);
            pPacket.WriteInt(pCharacter.Fame);
            pPacket.WriteShort(0); // Migration Data, v141.
            pPacket.WriteShort(-1800); // Migration Data, v141.
            pPacket.WriteInt(0); // Gachapon EXP.
            pPacket.WriteInt(pCharacter.MapId);
            pPacket.WriteByte(pCharacter.MapPosition);
            pPacket.WriteInt(0);
            pPacket.WriteShort(pCharacter.Subcategory);

            if (Constants.JobConstants.isDemonAvenger(pCharacter.Job) || Constants.JobConstants.isDemonSlayer(pCharacter.Job) || Constants.JobConstants.isXenon(pCharacter.Job))
                pPacket.WriteInt(pCharacter.FaceMarking);

            pPacket.WriteByte(pCharacter.Fatigue);
            pPacket.WriteInt(Constants.GetCurrentDate());

            pPacket.WriteInt(pCharacter.Ambition);
            pPacket.WriteInt(pCharacter.Insight);
            pPacket.WriteInt(pCharacter.Willpower);
            pPacket.WriteInt(pCharacter.Diligence);
            pPacket.WriteInt(pCharacter.Empathy);
            pPacket.WriteInt(pCharacter.Charm);

            pPacket.WriteZero(13);

            pPacket.WriteLong(Tools.GetTime(Tools.CurrentTimeMillis()));

            pPacket.WriteInt(pCharacter.BattleEXP);
            pPacket.WriteByte(pCharacter.BattleRank);
            pPacket.WriteInt(pCharacter.BattlePoints);
            pPacket.WriteByte(5);
            pPacket.WriteByte(6);
            pPacket.WriteByte(0);
            pPacket.WriteInt(0);

            pPacket.WriteBytes(new byte[] { 59, 55, 79, 1, 0, 64 });
            pPacket.WriteSByte(-32);
            pPacket.WriteSByte(-3);

            pPacket.WriteShort(0);
            pPacket.WriteZero(3);

            for (int i = 0; i < 9; i++)
            {
                pPacket.WriteInt(0);
                pPacket.WriteByte(0);
                pPacket.WriteInt(0);
            }

            pPacket.WriteReversedLong(Tools.GetTime(Tools.CurrentTimeMillis()));
        }

        public static void AddCharacterLooks(Packet pPacket, Character pCharacter, bool pMega)
        {
            pPacket.WriteByte(pCharacter.Gender);
            pPacket.WriteByte(pCharacter.SkinColor);
            pPacket.WriteInt(pCharacter.FaceId);
            pPacket.WriteInt(pCharacter.Job);
            pPacket.WriteByte((byte)(pMega ? 0 : 1));
            pPacket.WriteInt(pCharacter.HairId);

            foreach (KeyValuePair<short, Equip> kvp in pCharacter.Inventory[InventoryType.EQUIPPED].Equips)
            {
                pPacket.WriteByte((byte)(Math.Abs(kvp.Key)));
                pPacket.WriteInt(kvp.Value.ItemID);
            }

            pPacket.WriteByte(255); // End of Regular Equips.
            pPacket.WriteByte(255); // End of Masked Equips. TODO.
            pPacket.WriteByte(255); // v140.1 - Unknown equip type.

            pPacket.WriteInt(0); // TODO: Cash Weapon.
            pPacket.WriteInt(0); // TODO: Reg Weapon.
            pPacket.WriteInt(0); // TODO: Shield.
            pPacket.WriteByte(0); // Elf Ears (isMercedes).

            pPacket.WriteZero(12);

            if (Constants.JobConstants.isDemonAvenger(pCharacter.Job) || Constants.JobConstants.isDemonAvenger(pCharacter.Job) || Constants.JobConstants.isXenon(pCharacter.Job))
                pPacket.WriteInt(pCharacter.FaceMarking);
        }

        public static void AddCharacterInformation(Packet pPacket, Character pCharacter)
        {
            pPacket.WriteInt(-1);
            pPacket.WriteInt(-2097153);
            pPacket.WriteZero(20);
            AddCharacterStats(pPacket, pCharacter);
            pPacket.WriteByte(20); // TODO: Buddylist class.

            pPacket.WriteByte(0);
            pPacket.WriteByte(0);
            pPacket.WriteByte(0);

            AddInventoryInfo(pPacket, pCharacter);
            AddSkillInfo(pPacket, pCharacter);
            AddCooldownInfo(pPacket, pCharacter);
            AddQuestInfo(pPacket, pCharacter);
            AddRingInfo(pPacket, pCharacter);
            AddRocksInfo(pPacket, pCharacter);
            AddMonsterBookInfo(pPacket, pCharacter);

            pPacket.WriteShort(0);
            pPacket.WriteShort(0);

            // TODO: Quest info packet.
            pPacket.WriteShort(0);

            if (Constants.JobConstants.isWildHunter(pCharacter.Job))
                AddJaguarInfo(pPacket, pCharacter);

            pPacket.WriteByte(0);
            AddStealSkills(pPacket, pCharacter);
            AddInnerStats(pPacket, pCharacter);

            pPacket.WriteLong(1);
            pPacket.WriteLong(1);
            pPacket.WriteLong(0);
            pPacket.WriteLong(0);
            pPacket.WriteByte(0);
            pPacket.WriteLong(Tools.GetTime(-2));
            pPacket.WriteInt(0);
            pPacket.WriteByte(0);

            //TODO: Farm Info.
            pPacket.WriteMapleString("Creating...");
            pPacket.WriteInt(0); // Waru.
            pPacket.WriteInt(0); // Level
            pPacket.WriteInt(0); // EXP
            pPacket.WriteInt(0); // AestheticPoints
            pPacket.WriteInt(0); // Gems
            pPacket.WriteInt(0); // ?
            pPacket.WriteZero(5);
            pPacket.WriteInt(0);

            pPacket.WriteZero(13);

            pPacket.WriteLong(Tools.GetTime(-2));

            pPacket.WriteInt(0);
            pPacket.WriteInt(322037760);
            pPacket.WriteZero(68);
            pPacket.WriteLong(Tools.GetTime(Tools.CurrentTimeMillis()));
            pPacket.WriteInt(0);
            pPacket.WriteByte(1);
            pPacket.WriteShort(0);
            pPacket.WriteInt(41870555);
            pPacket.WriteInt(6550069);
            pPacket.WriteLong(4);

            for (int i = 0; i < 4; i++)
                pPacket.WriteLong(9410165 + i);
        }

        public static void AddInventoryInfo(Packet p, Character c)
        {
            p.WriteLong(c.Meso);
            p.WriteLong(0);

            p.WriteByte(c.Inventory[InventoryType.EQUIP].SlotLimit);
            p.WriteByte(c.Inventory[InventoryType.USE].SlotLimit);
            p.WriteByte(c.Inventory[InventoryType.SETUP].SlotLimit);
            p.WriteByte(c.Inventory[InventoryType.ETC].SlotLimit);
            p.WriteByte(c.Inventory[InventoryType.CASH].SlotLimit);

            //Quest:122700, idc.
            p.WriteLong(Tools.GetTime(-2L));

            //foreach (KeyValuePair<short, Equip> equip in c.Inventory[InventoryType.EQUIPPED].Equips)
            //{
            //    if (equip.Key < 0 && equip.Key > -100)
            //    {
            //        AddEquipPosition(p, equip.Value, false, false);
            //        AddEquipInfo(p, equip.Value, c);
            //    }
            //}
            p.WriteShort(0);

            //foreach (KeyValuePair<short, Equip> equip in c.Inventory[InventoryType.EQUIPPED].Equips)
            //{
            //    if (equip.Key <= -100 && equip.Key > -1000)
            //    {
            //        AddEquipPosition(p, equip.Value, false, false);
            //        AddEquipInfo(p, equip.Value, c);
            //    }
            //}
            p.WriteShort(0);

            //foreach (KeyValuePair<short, Equip> equip in c.Inventory[InventoryType.EQUIP].Equips)
            //{
            //        AddEquipPosition(p, equip.Value, false, false);
            //        AddEquipInfo(p, equip.Value, c);
            //}
            p.WriteShort(0);

            //foreach (KeyValuePair<short, Equip> equip in c.Inventory[InventoryType.EQUIPPED].Equips)
            //{
            //    if (equip.Key <= -1000 && equip.Key > - 1100) {
            //        AddEquipPosition(p, equip.Value, false, false);
            //        AddEquipInfo(p, equip.Value, c);
            //    }
            //}
            p.WriteShort(0);

            //foreach (KeyValuePair<short, Equip> equip in c.Inventory[InventoryType.EQUIPPED].Equips)
            //{
            //    if (equip.Key <= -1100 && equip.Key > - 1200) {
            //        AddEquipPosition(p, equip.Value, false, false);
            //        AddEquipInfo(p, equip.Value, c);
            //    }
            //}
            p.WriteShort(0);

            //foreach (KeyValuePair<short, Equip> equip in c.Inventory[InventoryType.EQUIPPED].Equips)
            //{
            //    if (equip.Key >= -1200) {
            //        AddEquipPosition(p, equip.Value, false, false);
            //        AddEquipInfo(p, equip.Value, c);
            //    }
            //}

            p.WriteShort(0);
            p.WriteShort(0);
            p.WriteShort(0);

            //foreach (KeyValuePair<short, Equip> equip in c.Inventory[InventoryType.EQUIPPED].Equips)
            //{
            //    if (equip.Key <= -5000 && equip.Key >= -5002) {
            //        AddEquipPosition(p, equip.Value, false, false);
            //        AddEquipInfo(p, equip.Value, c);
            //    }
            //}


            p.WriteShort(0);
            p.WriteShort(0);
            p.WriteShort(0);
            p.WriteShort(0);
            p.WriteShort(0);

            //foreach (KeyValuePair<byte, Item> item in c.Inventory[InventoryType.USE].Items)
            //{
            //    AddItemPosition(p, item.Value, false, false);
            //    AddItemInfo(p, item.Value, c);
            //}
            p.WriteByte(0);
            //foreach (KeyValuePair<byte, Item> item in c.Inventory[InventoryType.SETUP].Items)
            //{
            //    AddItemPosition(p, item.Value, false, false);
            //    AddItemInfo(p, item.Value, c);
            //}
            p.WriteByte(0);
            //foreach (KeyValuePair<byte, Item> item in c.Inventory[InventoryType.ETC].Items)
            //{
            //    AddItemPosition(p, item.Value, false, false);
            //    AddItemInfo(p, item.Value, c);
            //}
            p.WriteByte(0);
            //foreach (KeyValuePair<byte, Item> item in c.Inventory[InventoryType.CASH].Items)
            //{
            //    AddItemPosition(p, item.Value, false, false);
            //    AddItemInfo(p, item.Value, c);
            //}
            p.WriteByte(0);

            //TODO: Extended slots~

            p.WriteZero(17);
        }

        public static void AddItemPosition(Packet p, Item item, bool trade, bool bagSlot)
        {
            if (item == null)
            {
                p.WriteByte(0);
                return;
            }

            short pos = item.InventorySlot;
            if (pos <= -1)
            {
                pos = (short)(pos * -1);
                if ((pos > 100) && (pos < 1000))
                {
                    pos = (short)(pos - 100);
                }
            }
            if (bagSlot)
            {
                p.WriteInt(pos % 100 - 1);
            }
            else if ((!trade) && (Constants.ItemConstants.isEquip(item.ItemID)))
            {
                p.WriteShort(pos);
            }
            else
            {
                p.WriteByte((byte)pos);
            }
        }

        public static void AddEquipPosition(Packet p, Equip equip, bool trade, bool bagSlot)
        {
            if (equip == null)
            {
                p.WriteByte(0);
                return;
            }

            short pos = equip.InventorySlot;
            if (pos <= -1)
            {
                pos = (short)(pos * -1);
                if ((pos > 100) && (pos < 1000))
                {
                    pos = (short)(pos - 100);
                }
            }
            if (bagSlot)
            {
                p.WriteInt(pos % 100 - 1);
            }
            else if ((!trade) && (equip.Type == 1))
            {
                p.WriteShort(pos);
            }
            else
            {
                p.WriteByte((byte)pos);
            }
        }

        public static void AddItemInfo(Packet p, Item item, Character c)
        {
            //TODO: This shit.
        }

        public static void AddEquipInfo(Packet p, Equip equip, Character c)
        {
            p.WriteByte(1); // Item type. 2 - An item, 3 - A pet. TODO: Pets.
            p.WriteInt(equip.ItemID);

            p.WriteByte(0); // todo: unique id. 
            AddExpirationTime(p, equip.Expiration);

            p.WriteByte(0); // TODO: Extended slots.

            p.WriteInt(0); // All values of all stats.

            p.WriteByte(equip.Slots);
            p.WriteByte(equip.Level);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);
            p.WriteShort(equip.Str);

            p.WriteByte(0); // Inc skills?

            p.WriteByte(equip.ItemLevel);
            p.WriteInt(equip.ItemEXP * 100000);
            p.WriteInt(0); // Durability.
            p.WriteInt(equip.ViciousHammer);
            p.WriteShort(0); // PVP Damage.

            p.WriteInt(4);
            p.WriteByte(0xFF);
            p.WriteMapleString(""); // Owner.
            p.WriteByte(0); // State.
            p.WriteByte(0); // Enhance.
            p.WriteShort(0); // ptl1
            p.WriteShort(0); // ptl2
            p.WriteShort(0); // ptl3
            p.WriteShort(0); // bonus ptl 1
            p.WriteShort(0); // bonus ptl2
            p.WriteShort(0);
            p.WriteShort(0);
            p.WriteShort(0); // socket state
            p.WriteShort(0); // socket1
            p.WriteShort(0); // socket 2
            p.WriteShort(0); // socket 3
            //if (equip.CashId > 0)
            //p.writelong
            p.WriteLong(Tools.GetTime(-2));
            p.WriteInt(-1);
        }

        public static void AddExpirationTime(Packet p, long time)
        {
            p.WriteLong(Tools.GetTime(time));
        }

        public static void AddSkillInfo(Packet p, Character c)
        {
            //Dictionairy of skill, skillentry of character's skills.

            p.WriteByte(1);
            p.WriteShort(0); // Skills size.

            //For each skill - writeInt(SkillID), WriteInt(SkillLevel), addExpirationTime of skill, If skill is fourth job skill - WriteInt(MasterLevel).
        }

        public static void AddCooldownInfo(Packet p, Character c)
        {
            // Insert cooldowns of character to a list.

            p.WriteShort(0); // Cooldowns size.

            //For each cd - WriteInt(SkillID), WriteInt(TimeLeft).

            p.WriteShort(0); // CD is empty.
        }

        public static void AddQuestInfo(Packet p, Character c)
        {
            // Started quests's list.
            p.WriteByte(1);

            p.WriteShort(0); // Started quests size.
            // For each started quest, writeShort(id), if the quest has mob kills - build a string of kills and shit, and write the string as maple string. If not, get custom data as maplestring.

            p.WriteShort(0);
            p.WriteByte(1);
            //List of completed quests.
            p.WriteShort(0); // Completed quests size.
            // For each completed quest, write short the ID and writeInt the completion time.
        }

        public static void AddAnnounceBox(Packet p, Character c)
        {
            //TODO.
            p.WriteByte(0);
        }

        public static void AddRingInfo(Packet p, Character c)
        {
            p.WriteShort(0);

            //Triple<List<MpaleRing>, List<MapleRing>, List<MapleRing>> - of character's rings.
            //The following applies 3 times: 3 ring slots.
            p.WriteShort(0); // Rings size.
            p.WriteShort(0); // Rings size.
            p.WriteShort(0); // Rings size.
            // For each ring, writeInt(partnerCharacterID), writeString(ParterName, 13), writeLong(RingID), writeLong(PartnerRingID).
        }

        public static void AddRocksInfo(Packet p, Character c)
        {
            //Put reg rocks maps into array. Write map ID of each one
            //In order: Regular Rocks, VIP Rocks, Hyper Rocks.
            for (int i = 0; i < 5; i++)
            {
                p.WriteInt(999999999);
            }

            for (int i = 0; i < 10; i++)
            {
                p.WriteInt(999999999);
            }

            for (int i = 0; i < 13; i++)
            {
                p.WriteInt(999999999);
            }

            for (int i = 0; i < 13; i++)
            {
                p.WriteInt(999999999);
            }
        }

        public static void AddMonsterBookInfo(Packet p, Character c)
        {
            p.WriteInt(0);

            //Getscore>0, writeFinished. Else, writeUnfinished.
            p.WriteByte(0);
            p.WriteShort(0);

            p.WriteInt(0);
        }

        public static void AddJaguarInfo(Packet p, Character c)
        {
            p.WriteByte(0); // Record of quest: 111112.
            p.WriteZero(20);
        }

        public static void AddStealSkills(Packet p, Character c)
        {
            for (int i = 1; i <= 4; i++)
            {
                AddStolenSkills(p, c, i, false);
            }

            AddChosenSkills(p, c);
        }

        public static void AddStolenSkills(Packet p, Character c, int jobNum, bool writeJob)
        {
            if (writeJob)
            {
                p.WriteInt(jobNum);
            }

            int count = 0;
            while (count < Constants.SkillConstants.getNumSteal(jobNum))
            {
                p.WriteInt(0);
                count++;
            }
        }

        public static void AddChosenSkills(Packet p, Character c)
        {
            //TODO: Chosen skills.
            for (int i = 1; i <= 4; i++)
            {
                p.WriteInt(0);
            }
        }

        public static void AddInnerStats(Packet p, Character c)
        {
            p.WriteShort(0); //Inner skills count.
            //TODO: Inner skills.

            p.WriteInt(c.HonorLevel);
            p.WriteInt(c.HonorEXP);
        }

        public static bool ParseMovementData(MovableLife pLife, Packet pPacket)
        {
            byte amount = pPacket.ReadByte();

            byte type, Stance = pLife.Stance;

            short Foothold = pLife.Foothold, X = pLife.Position.X, Y = pLife.Position.Y;
            short startX = pLife.Position.X;
            short startY = pLife.Position.Y;

            bool toRet = true;
            bool needCheck = true;

            for (byte i = 0; i < amount; i++)
            {
                type = pPacket.ReadByte();
                switch (type)
                {
                    case 0x00: //normal move
                    case 0x05:
                        {
                            pLife.Position.X = pPacket.ReadShort();
                            pLife.Position.Y = pPacket.ReadShort();
                            pLife.Wobble.X = pPacket.ReadShort();
                            pLife.Wobble.Y = pPacket.ReadShort();
                            pLife.Foothold = pPacket.ReadShort();
                            pLife.Stance = pPacket.ReadByte();

                            if (Stance < 5)
                                pLife.Jumps = 0;
                            if (pLife.GetType() == typeof(Character) && Stance < 14 && Stance != 6 && Stance != 7)
                            {
                                float Speed = 100 / 100.0f;
                                toRet = CheatInspector.CheckSpeed(pLife.Wobble, (Speed));
                            }
                            else if (pLife.GetType() == typeof(Mob) && pLife.Wobble.Y != 0)
                            {
                                if (Stance == 7 || Stance == 6)
                                    needCheck = false;
                                toRet = CheatInspector.CheckSpeed(pLife.Wobble, ((Mob)pLife).AllowedSpeed);
                            }

                            pPacket.Skip(2);
                            break;
                        }
                    case 0x01: //jump, here we check for jumpingshit
                        {

                            if (pLife.Jumps > 5) toRet = false;

                            X = pPacket.ReadShort();
                            Y = pPacket.ReadShort();
                            Stance = pPacket.ReadByte();
                            Foothold = pPacket.ReadShort();
                            pLife.Jumps++;
                            break;
                        }
                    case 0x02:
                    case 0x06:
                        {
                            X = pPacket.ReadShort();
                            Y = pPacket.ReadShort();
                            Stance = pPacket.ReadByte();
                            Foothold = pPacket.ReadShort();
                            break;
                        }
                    case 0x03:
                    case 0x04: //tele
                    case 0x07: //assaulter
                        {
                            X = pPacket.ReadShort();
                            Y = pPacket.ReadShort();
                            pLife.Wobble.X = pPacket.ReadShort();
                            pLife.Wobble.Y = pPacket.ReadShort();
                            Stance = pPacket.ReadByte();

                            if (type == 0x03 && pLife.GetType() == typeof(Character) && Stance != 7 && Stance != 6)
                            {
                                float Speed = 100 / 100.0f;
                                toRet = CheatInspector.CheckSpeed(pLife.Wobble, Speed);
                            }

                            break;
                        }
                    case 0x08:
                        {
                            pPacket.Skip(1);
                            break;
                        }
                    default:
                        {
                            Stance = pPacket.ReadByte();
                            //packet.Skip(2);
                            Foothold = pPacket.ReadShort();
                            break;
                        }
                }
            }
            /*
            if (!toRet) {
                Console.WriteLine("return toRet called 1");
                return toRet;
            }
            */
            if (pLife.GetType() == typeof(Mob) && needCheck)
            {
                int PastMS = (int)(DateTime.Now - ((Mob)pLife).lastMove).TotalMilliseconds;
                ((Mob)pLife).lastMove = DateTime.Now;
                int allowedDistance = (int)((((Mob)pLife).AllowedSpeed + 0.5f) * PastMS);
                ushort Walkeddistance = (ushort)Math.Abs(X - startX);
                if ((allowedDistance) < Walkeddistance)
                {
                    Console.WriteLine("return false 1");
                    return false;
                }
            }

            pLife.Foothold = Foothold;
            pLife.Position.X = X;
            pLife.Position.Y = Y;
            pLife.Stance = Stance;
            return toRet;
        }

        public static void CashShopPackets(Client pClient)
        {
            pClient.SendPacket(CashShopPacket.Cash_Shop_Inventory(pClient));
            pClient.SendPacket(CashShopPacket.Cash_Shop_Magic());
            pClient.SendPacket(CashShopPacket.Cash_Shop_Gifts(pClient));
            pClient.SendPacket(CashShopPacket.Cash_Shop_Wishlist(pClient, false));
        }
    }
}
