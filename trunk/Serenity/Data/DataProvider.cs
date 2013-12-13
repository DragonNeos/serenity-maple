using reNX;
using reNX.NXProperties;
using Serenity.Game.Objects;
using Serenity.Other;
using Serenity.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Data
{
    public sealed class DataProvider
    {
        public Dictionary<int, Map> Maps { get; set; }
        public Dictionary<int, EquipData> Equips { get; set; }
        public Dictionary<int, NPCData> NPCs { get; set; }
        public Dictionary<int, ItemData> Items { get; set; }
        public Dictionary<int, PetData> Pets { get; set; }
        public Dictionary<int, MobData> Mobs { get; set; }
        public Dictionary<int, Dictionary<byte, SkillLevelData>> Skills { get; set; }
        public Dictionary<byte, Dictionary<byte, MobSkillLevelData>> MobSkills { get; set; }
        public Dictionary<string, List<DropData>> Drops { get; set; }
        public Dictionary<int, TamingMobData> TamingMobs { get; set; }

        private NXFile CharacterNX, EtcNX, ItemNX, MapNX, MobNX, QuestNX, ReactorNX, SkillNX, StringNX, TamingMobNX;

        public void Load(string pPath)
        {
            DateTime Start = DateTime.Now;
            Logger.WriteLog(Logger.LogTypes.NX, "Caching NX files...");

            Equips = new Dictionary<int, EquipData>();
            Maps = new Dictionary<int, Map>();
            Mobs = new Dictionary<int, MobData>();
            TamingMobs = new Dictionary<int, TamingMobData>();

            CharacterNX = new NXFile(pPath + "\\Character.NX");
            //EtcNX = new NXFile("\\Etc.NX");
            //ItemNX = new NXFile("\\Item.NX");
            MobNX = new NXFile(pPath + "\\Mob.NX");
            MapNX = new NXFile(pPath + "\\Map.NX");
            //QuestNX = new NXFile("\\Quest.NX");
            //ReactorNX = new NXFile("\\Reactor.NX");
            //SkillNX = new NXFile("\\Skill.NX");
            //StringNX = new NXFile("\\String.NX");
            //TamingMobNX = new NXFile("\\TamingMob.NX");

            LoadMobs();
            LoadMaps();
            LoadEquips();
            LoadTamingMobs();

            CharacterNX.Dispose();
            MapNX.Dispose();
            MobNX.Dispose();

            DateTime Finish = DateTime.Now;

            Logger.WriteLog(Logger.LogTypes.NX, "Cached NX files in {0}ms.", (Finish-Start).Milliseconds);
        }

        public void LoadMaps()
        {
            if (MapNX == null)
            {
                Logger.WriteLog(Logger.LogTypes.Error, "Unable to load Map NX file.");
                return;
            }

            foreach (NXNode baseNode in MapNX.BaseNode)
            {
                if (baseNode.Name == "Map")
                {
                    foreach (NXNode Map in baseNode)
                    {
                        if (Map.Name.Contains("Map"))
                        {
                            foreach (NXNode MapNode in Map)
                            {
                                NXNode info = MapNode["info"];

                                Map map = new Map(int.Parse(MapNode.Name.Replace(".img", "")));
                                if (info.ContainsChild("fly"))
                                    map.Fly = info["fly"].ValueOrDefault<int>(0) > 0;
                                if (info.ContainsChild("forcedReturn"))
                                    map.ForcedReturn = info["forcedReturn"].ValueOrDefault<int>(999999999);
                                if (info.ContainsChild("onFirstUserEnter"))
                                    map.OnFirstUserEnter = info["onFirstUserEnter"].ValueOrDefault<string>(null);
                                if (info.ContainsChild("onUserEnter"))
                                    map.OnUserEnter = info["onUserEnter"].ValueOrDefault<string>(null);
                                if (info.ContainsChild("swim"))
                                    map.Swim = info["swim"].ValueOrDefault<int>(0) > 0;

                                if (MapNode.ContainsChild("life"))
                                {
                                    foreach (NXNode LifeNode in MapNode["life"])
                                    {
                                        Life lf = new Life();
                                        if (LifeNode.ContainsChild("id"))
                                            lf.ID = int.Parse(LifeNode["id"].ValueOrDefault<string>(null));
                                        if (LifeNode.ContainsChild("x"))
                                            lf.X = (Int16)LifeNode["x"].ValueOrDefault<Int64>(0);
                                        if (LifeNode.ContainsChild("y"))
                                            lf.Y = (Int16)LifeNode["y"].ValueOrDefault<Int64>(0);
                                        if (LifeNode.ContainsChild("cy"))
                                            lf.Cy = (Int16)LifeNode["cy"].ValueOrDefault<Int64>(0);
                                        if (LifeNode.ContainsChild("fh"))
                                            lf.Foothold = (UInt16)LifeNode["fh"].ValueOrDefault<Int64>(0);
                                        if (LifeNode.ContainsChild("rx0"))
                                            lf.Rx0 = (Int16)LifeNode["rx0"].ValueOrDefault<Int64>(0);
                                        if (LifeNode.ContainsChild("rx1"))
                                            lf.Rx0 = (Int16)LifeNode["rx1"].ValueOrDefault<Int64>(0);
                                        if (LifeNode.ContainsChild("type"))
                                            lf.Type = (LifeNode["type"].ValueOrDefault<string>(null));
                                        if (LifeNode.ContainsChild("f"))
                                            lf.FacesLeft = Convert.ToBoolean(LifeNode["f"].ValueOrDefault<Int64>(0));
                                        if (LifeNode.ContainsChild("mobTime"))
                                            lf.RespawnTime = (int)LifeNode["mobTime"].ValueOrDefault<Int64>(0);
                                        if (LifeNode.ContainsChild("hide"))
                                            lf.Hide = Convert.ToByte(LifeNode["hide"].ValueOrDefault<Int64>(0));

                                        if (lf.ID != 0)
                                            map.AddLife(lf);
                                    }
                                }

                                if (MapNode.ContainsChild("portal"))
                                {
                                    foreach (NXNode PortalNode in MapNode["portal"])
                                    {
                                        Portal pt = new Portal();
                                        pt.ID = byte.Parse(PortalNode.Name);
                                        pt.Name = PortalNode["pn"].ValueOrDefault<string>(null);
                                        pt.ToMapID = (int)PortalNode["tm"].ValueOrDefault<Int64>(999999999);
                                        pt.ToName = PortalNode["tn"].ValueOrDefault<string>(null);
                                        pt.X = (short)PortalNode["x"].ValueOrDefault<Int64>(0);
                                        pt.Y = (short)PortalNode["y"].ValueOrDefault<Int64>(0);

                                        map.AddPortal(pt);
                                    }
                                }

                                Maps.Add(map.ID, map);
                            }
                        }
                    }
                }
            }
        }

        public void LoadMobs()
        {
            if (MobNX == null)
            {
                Logger.WriteLog(Logger.LogTypes.Error, "Unable to load Mob NX file.");
                return;
            }

            foreach (NXNode MobNode in MobNX.BaseNode)
            {
                if (MobNode.Name.Contains(".img"))
                {
                    MobData mob = new MobData();

                    mob.ID = int.Parse(MobNode.Name.Replace(".img", ""));
                    if (MobNode["info"].ContainsChild("exp"))
                        mob.EXP = (int)MobNode["info"]["exp"].ValueOrDefault<Int64>(0);
                    if (MobNode["info"].ContainsChild("level"))
                        mob.Level = (byte)MobNode["info"]["level"].ValueOrDefault<Int64>(0);
                    mob.MaxHP = (int)MobNode["info"]["maxHP"].ValueOrDefault<Int64>(0);
                    if (MobNode["info"].ContainsChild("speed"))
                        mob.Speed = (short)MobNode["info"]["speed"].ValueOrDefault<Int64>(0);
                    if (MobNode["info"].ContainsChild("summonType"))
                        mob.SummonType = (byte)MobNode["info"]["summonType"].ValueOrDefault<Int64>(0);
                    if (MobNode["info"].ContainsChild("undead"))
                        mob.Undead = Convert.ToBoolean(MobNode["info"]["undead"].ValueOrDefault<Int64>(0));
                    if (MobNode["info"].ContainsChild("boss"))
                        mob.Boss = Convert.ToBoolean(MobNode["info"]["boss"].ValueOrDefault<Int64>(0));
                    if (MobNode["info"].ContainsChild("skill"))
                    {
                        NXNode skillNode = MobNode["info"]["skill"];
                        foreach (NXNode SkillNode in skillNode)
                        {
                            MobSkillData msd = new MobSkillData();
                            msd.SkillID = (byte)SkillNode["skill"].ValueOrDefault<Int64>(0);
                            msd.Level = (byte)SkillNode["level"].ValueOrDefault<Int64>(0);
                            if (SkillNode.ContainsChild("effectAfter"))
                                msd.EffectAfter = (short)SkillNode["effectAfter"].ValueOrDefault<Int64>(0);
                            if (SkillNode.ContainsChild("skillAfter"))
                                msd.SkillAfter = (short)SkillNode["skillAfter"].ValueOrDefault<Int64>(0);
                        }
                    }

                    if (MobNode["info"].ContainsChild("revive"))
                    {
                        mob.Revive = new List<int>();
                        foreach (NXNode ReviveNode in MobNode["info"]["revive"])
                        {
                            int reviveID = (int)ReviveNode.ValueOrDefault<Int64>(0);
                            mob.Revive.Add(reviveID);
                        }
                    }

                    Mobs.Add(mob.ID, mob);
                }
            }
        }

        public void LoadEquips()
        {
            if (CharacterNX == null)
            {
                Logger.WriteLog(Logger.LogTypes.Error, "Unable to load Character NX file.");
                return;
            }

            foreach (NXNode BaseNode in CharacterNX.BaseNode)
            {
                if (!BaseNode.Name.Contains(".img") && !BaseNode.Name.Equals("Afterimage") && !BaseNode.Name.Equals("Bits") && !BaseNode.Name.Equals("Hair") && !BaseNode.Name.Equals("Face") && !BaseNode.Name.Equals("Familiar"))
                {
                    if (!BaseNode.Name.Equals("PetEquip"))
                    {
                        foreach (NXNode EquipNode in BaseNode)
                        {
                            if (EquipNode.Name.Contains(".img") && EquipNode.ContainsChild("info"))
                            {
                                NXNode Info = EquipNode["info"];
                                EquipData ed = new EquipData();
                                ed.ID = int.Parse(EquipNode.Name.Replace(".img", ""));

                                //Some chairs are located in TamingMob. Fuck you Nexon. You failed me again.
                                if (ed.ID >= 1983002 && ed.ID <= 1983099)
                                    break;

                                if (Info.ContainsChild("isCash"))
                                    ed.isCash = Convert.ToBoolean(Info["cash"].ValueOrDefault<Int64>(0));
                                else
                                    ed.isCash = false;
                                ed.Type = Info["islot"].ValueOrDefault<string>(null);
                                if (Info.ContainsChild("tuc"))
                                    ed.Scrolls = (byte)Info["tuc"].ValueOrDefault<Int64>(0);

                                if (Info.ContainsChild("reqDEX"))
                                    ed.RequiredDexterity = (ushort)Info["reqDEX"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("reqINT"))
                                    ed.RequiredIntellect = (ushort)Info["reqINT"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("reqJob"))
                                    ed.RequiredJob = (ushort)Info["reqJob"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("reqLevel"))
                                    ed.RequiredLevel = (byte)Info["reqLevel"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("reqLUK"))
                                    ed.RequiredLuck = (ushort)Info["reqLUK"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("reqSTR"))
                                    ed.RequiredStrength = (ushort)Info["reqSTR"].ValueOrDefault<Int64>(0);

                                if (Info.ContainsChild("price"))
                                    ed.Price = (int)Info["price"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incSTR"))
                                    ed.Strength = (short)Info["incSTR"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incDEX"))
                                    ed.Dexterity = (short)Info["incDEX"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incINT"))
                                    ed.Intellect = (short)Info["incINT"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incLUK"))
                                    ed.Luck = (short)Info["incLUK"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incMDD"))
                                    ed.MagicDefense = (byte)Info["incMDD"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incPDD"))
                                    ed.WeaponDefense = (byte)Info["incPDD"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incPAD"))
                                    ed.WeaponAttack = (byte)Info["incPAD"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incMAD"))
                                    ed.MagicAttack = (byte)Info["incMAD"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incSpeed"))
                                    ed.Speed = (byte)Info["incSpeed"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incJump"))
                                    ed.Jump = (byte)Info["incJump"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incACC"))
                                    ed.Accuracy = (byte)Info["incACC"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incEVA"))
                                    ed.Avoidance = (byte)Info["incEVA"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incHP"))
                                    ed.HP = (byte)Info["incHP"].ValueOrDefault<Int64>(0);
                                if (Info.ContainsChild("incMP"))
                                    ed.MP = (byte)Info["incMP"].ValueOrDefault<Int64>(0);

                                if (!Equips.ContainsKey(ed.ID))
                                    Equips.Add(ed.ID, ed);
                            }
                        }
                    }
                    else
                    {
                        // Pet equip data, different from others.
                        // WE SKIP PET EQUIPS FOR NOW~

                        //foreach (NXNode PetEquipImage in BaseNode["PetEquip"])
                        //{
                        //    foreach (NXNode EquipNode in PetEquipImage)
                        //    {
                        //        NXNode Info = EquipNode["info"];
                        //        EquipData ed = new EquipData();
                        //        ed.ID = int.Parse(EquipNode.Name);
                        //        Console.WriteLine(ed.ID);
                        //        if (Info.ContainsChild("isCash"))
                        //            ed.isCash = Convert.ToBoolean(Info["cash"].ValueOrDefault<Int64>(0));
                        //        else
                        //            ed.isCash = false;
                        //        ed.Type = Info["islot"].ValueOrDefault<string>(null);
                        //        if (Info.ContainsChild("tuc"))
                        //            ed.Scrolls = (byte)Info["tuc"].ValueOrDefault<Int64>(0);

                        //        if (Info.ContainsChild("reqDEX"))
                        //            ed.RequiredDexterity = (ushort)Info["reqDEX"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("reqINT"))
                        //            ed.RequiredIntellect = (ushort)Info["reqINT"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("reqJob"))
                        //            ed.RequiredJob = (ushort)Info["reqJob"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("reqLevel"))
                        //            ed.RequiredLevel = (byte)Info["reqLevel"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("reqLUK"))
                        //            ed.RequiredLuck = (ushort)Info["reqLUK"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("reqSTR"))
                        //            ed.RequiredStrength = (ushort)Info["reqSTR"].ValueOrDefault<Int64>(0);

                        //        if (Info.ContainsChild("price"))
                        //            ed.Price = (int)Info["price"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incSTR"))
                        //            ed.Strength = (short)Info["incSTR"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incDEX"))
                        //            ed.Dexterity = (short)Info["incDEX"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incINT"))
                        //            ed.Intellect = (short)Info["incINT"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incLUK"))
                        //            ed.Luck = (short)Info["incLUK"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incMDD"))
                        //            ed.MagicDefense = (byte)Info["incMDD"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incPDD"))
                        //            ed.WeaponDefense = (byte)Info["incPDD"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incPAD"))
                        //            ed.WeaponAttack = (byte)Info["incPAD"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incMAD"))
                        //            ed.MagicAttack = (byte)Info["incMAD"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incSpeed"))
                        //            ed.Speed = (byte)Info["incSpeed"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incJump"))
                        //            ed.Jump = (byte)Info["incJump"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incACC"))
                        //            ed.Accuracy = (byte)Info["incACC"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incEVA"))
                        //            ed.Avoidance = (byte)Info["incEVA"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incHP"))
                        //            ed.HP = (byte)Info["incHP"].ValueOrDefault<Int64>(0);
                        //        if (Info.ContainsChild("incMP"))
                        //            ed.MP = (byte)Info["incMP"].ValueOrDefault<Int64>(0);

                        //        if (!Equips.ContainsKey(ed.ID))
                        //            Equips.Add(ed.ID, ed);
                        //    }
                    }
                }
            }
        }

        public void LoadTamingMobs()
        {
            if (TamingMobNX == null)
            {
                Logger.WriteLog(Logger.LogTypes.Error, "Unable to load Taming Mob NX file.");
                return;
            }

            foreach (NXNode tmImg in TamingMobNX.BaseNode)
            {
                NXNode Info = tmImg["info"];
                TamingMobData tmd = new TamingMobData();
                tmd.ID = int.Parse(tmImg.Name.Replace(".img", ""));
                tmd.Fatigue = (byte)Info["fatigue"].ValueOrDefault<Int64>(0) ;
                tmd.Jump = (byte)Info["jump"].ValueOrDefault<Int64>(0);
                tmd.Speed = (byte)Info["speed"].ValueOrDefault<Int64>(0);
                tmd.Swim = (byte)Info["swim"].ValueOrDefault<Int64>(0);

                TamingMobs.Add(tmd.ID, tmd);
            }
        }
    }

    public class EquipData
    {
        public int ID { get; set; }
        public bool isCash { get; set; }
        public string Type { get; set; }
        public byte HealHP { get; set; }
        public byte Scrolls { get; set; }
        public byte RequiredLevel { get; set; }
        public ushort RequiredStrength { get; set; }
        public ushort RequiredDexterity { get; set; }
        public ushort RequiredIntellect { get; set; }
        public ushort RequiredLuck { get; set; }
        public ushort RequiredJob { get; set; }
        public int Price { get; set; }
        public byte RequiredFame { get; set; }
        public short HP { get; set; }
        public short MP { get; set; }
        public short Strength { get; set; }
        public short Dexterity { get; set; }
        public short Intellect { get; set; }
        public short Luck { get; set; }
        public byte Hands { get; set; }
        public byte WeaponAttack { get; set; }
        public byte MagicAttack { get; set; }
        public byte WeaponDefense { get; set; }
        public byte MagicDefense { get; set; }
        public byte Accuracy { get; set; }
        public byte Avoidance { get; set; }
        public byte Speed { get; set; }
        public byte Jump { get; set; }
    }

    public class ShopItemData
    {
        public int ID { get; set; }
        public int Stock { get; set; }
        public int Price { get; set; }
    }

    public class NPCData
    {
        public int ID { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public string Quest { get; set; }
        public int Trunk { get; set; }
        public List<ShopItemData> Shop { get; set; }
    }

    public class ItemData
    {
        public int ID { get; set; }
        public int Price { get; set; }
        public bool Cash { get; set; }
        public ushort MaxSlot { get; set; }
        public short HP { get; set; }
        public short MP { get; set; }
        public short HPRate { get; set; }
        public short MPRate { get; set; }
        public short WeaponAttack { get; set; }
        public short MagicAttack { get; set; }
        public short Accuracy { get; set; }
        public short Avoidance { get; set; }
        public short Speed { get; set; }
        public int BuffTime { get; set; }

        public byte CureFlags { get; set; }

        public int MoveTo { get; set; }
        public int Mesos { get; set; }

        public byte ScrollSuccessRate { get; set; }
        public byte ScrollCurseRate { get; set; }
        public byte IncStr { get; set; }
        public byte IncDex { get; set; }
        public byte IncInt { get; set; }
        public byte IncLuk { get; set; }
        public byte IncMHP { get; set; }
        public byte IncMMP { get; set; }
        public byte IncWAtk { get; set; }
        public byte IncMAtk { get; set; }
        public byte IncWDef { get; set; }
        public byte IncMDef { get; set; }
        public byte IncAcc { get; set; }
        public byte IncAvo { get; set; }
        public byte IncJump { get; set; }
        public byte IncSpeed { get; set; }

        public List<ItemSummonInfo> Summons { get; set; }
    }

    public class ItemSummonInfo
    {
        public int MobID { get; set; }
        public byte Chance { get; set; }
    }

    public class MobSkillLevelData
    {
        public byte SkillID { get; set; }
        public byte Level { get; set; }
        public short Time { get; set; }
        public short MPConsume { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public byte Prop { get; set; }
        public short Cooldown { get; set; }

        public short LTX { get; set; }
        public short LTY { get; set; }
        public short RBX { get; set; }
        public short RBY { get; set; }

        public byte HPLimit { get; set; }
        public byte SummonLimit { get; set; }
        public byte SummonEffect { get; set; }
        public List<int> Summons { get; set; }
    }

    public class MobSkillData
    {
        public byte Level { get; set; }
        public byte SkillID { get; set; }
        public short EffectAfter { get; set; }
        public short SkillAfter { get; set; }
    }

    public class MobAttackData
    {
        public byte ID { get; set; }
        public short MPConsume { get; set; }
        public short MPBurn { get; set; }
        public short SkillID { get; set; }
        public byte SkillLevel { get; set; }
    }

    public class MobData
    {
        public int ID { get; set; }
        public byte Level { get; set; }
        public bool Boss { get; set; }
        public bool Undead { get; set; }
        public bool BodyAttack { get; set; }
        public int EXP { get; set; }
        public int MaxHP { get; set; }
        public int MaxMP { get; set; }
        public int HPRecoverAmount { get; set; }
        public int MPRecoverAmount { get; set; }
        public short Speed { get; set; }
        public byte SummonType { get; set; }
        public byte removeAfter { get; set; }
        public List<int> Revive { get; set; }
        public Dictionary<byte, MobAttackData> Attacks { get; set; }
        public List<MobSkillData> Skills { get; set; }
    }

    public class SkillLevelData
    {
        public byte MobCount { get; set; }
        public byte HitCount { get; set; }

        public int BuffTime { get; set; }
        public short Damage { get; set; }
        public short AttackRange { get; set; }
        public byte Mastery { get; set; }

        public short HPProperty { get; set; }
        public short MPProperty { get; set; }
        public short Property { get; set; }

        public short HPUsage { get; set; }
        public short MPUsage { get; set; }
        public int ItemIDUsage { get; set; }
        public short ItemAmountUsage { get; set; }
        public short BulletUsage { get; set; }
        public short MesosUsage { get; set; }

        public short XValue { get; set; }
        public short YValue { get; set; }

        public short Speed { get; set; }
        public short Jump { get; set; }
        public short WeaponAttack { get; set; }
        public short MagicAttack { get; set; }
        public short WeaponDefence { get; set; }
        public short MagicDefence { get; set; }
        public short Accurancy { get; set; }
        public short Avoidability { get; set; }

        public short LTX { get; set; }
        public short LTY { get; set; }
        public short RBX { get; set; }
        public short RBY { get; set; }
    }

    public class DropData
    {
        public int ItemID { get; set; }
        public int Mesos { get; set; }
        public short Min { get; set; }
        public short Max { get; set; }
        public int Chance { get; set; }
    }

    public class PetData
    {
        public int ItemID { get; set; }
        public byte Hungry { get; set; }
        public byte Life { get; set; }
        public Dictionary<byte, PetReactionData> Reactions { get; set; }
    }

    public class PetReactionData
    {
        public byte ReactionID { get; set; }
        public byte Inc { get; set; }
        public byte Prob { get; set; }
    }

    public class TamingMobData
    {
        public int ID { get; set; }
        public byte Fatigue { get; set; }
        public byte Jump { get; set; }
        public byte Speed { get; set; }
        public byte Swim { get; set; }
    }
}
