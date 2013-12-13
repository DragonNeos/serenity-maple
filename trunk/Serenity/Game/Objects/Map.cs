using Serenity;
using Serenity.Other;
using Serenity.Packets;
using Serenity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Game.Objects
{
    public class Map
    {
        public int ID { get; set; }
        public int ForcedReturn { get; set; }
        public int ReturnMap { get; set; }
        public bool Town { get; set; }
        public bool HasClock { get; set; }
        public byte FieldType { get; set; }
        public bool Fly { get; set; }
        public bool Swim { get; set; }
        public int MobRate { get; set; }
        public string OnFirstUserEnter { get; set; }
        public string OnUserEnter { get; set; }

        public int WeatherID { get; set; }
        public string WeatherMessage { get; set; }
        public bool WeatherAdmin { get; set; }

        const uint NpcStart = 100;
        const uint ReactorStart = 200;

        public LoopingID ObjectIDs { get; set; }

        private List<Foothold> _FHs = new List<Foothold>();
        public List<Foothold> Footholds { get { return _FHs; } }

        private List<Life> _Respawns = new List<Life>();
        public List<Life> Respawns { get { return _Respawns; } }

        private Dictionary<string, List<Life>> _Life = new Dictionary<string, List<Life>>();
        public Dictionary<string, List<Life>> Life { get { return _Life; } }

        private Dictionary<string, Portal> _Portal = new Dictionary<string, Portal>();
        public Dictionary<string, Portal> Portals { get { return _Portal; } }

        private Dictionary<int, Portal> _SpawnPoints = new Dictionary<int, Portal>();
        public Dictionary<int, Portal> SpawnPoints { get { return _SpawnPoints; } }

        private Dictionary<int, Seat> _Seat = new Dictionary<int, Seat>();
        public Dictionary<int, Seat> Seats { get { return _Seat; } }

        public List<short> UsedSeats { get; set; }

        private List<Mob> _Mobs = new List<Mob>();
        public List<Mob> Mobs { get { return _Mobs; } }

        private Dictionary<int, Drop> _Drops = new Dictionary<int, Drop>();
        public Dictionary<int, Drop> Drops { get { return _Drops; } }

        public List<Character> Characters { get; set; }

        public Map(int id)
        {
            ID = id;
            ObjectIDs = new LoopingID();
            Characters = new List<Character>();
            _Life.Add("m", new List<Life>());
            _Life.Add("n", new List<Life>());
            _Life.Add("r", new List<Life>());
            UsedSeats = new List<short>();
        }

        public void MapTimer()
        {
            DateTime expire = DateTime.Now.Subtract(new TimeSpan(0, 3, 0));
            Dictionary<int, Drop> dropBackup = new Dictionary<int, Drop>(Drops);
            lock (dropBackup)
            {
                foreach (KeyValuePair<int, Drop> kvp in dropBackup)
                {
                    if (kvp.Value != null && expire > kvp.Value.Droptime)
                    {
                        kvp.Value.RemoveDrop(true);
                    }
                }
            }
        }

        public void ClearDrops()
        {
            Dictionary<int, Drop> dropBackup = new Dictionary<int, Drop>(Drops);
            lock (dropBackup)
            {
                foreach (KeyValuePair<int, Drop> kvp in dropBackup)
                {
                    if (kvp.Value != null)
                    {
                        kvp.Value.RemoveDrop(true);
                    }
                }
            }
        }

        internal Mob GetMob(int SpawnID) { return Mobs.Find(m => m.SpawnID == SpawnID); }
        internal Life GetNPC(int SpawnID) { return Life["n"].Find(n => n.SpawnID == SpawnID); }

        public void AddDrop(Drop drop)
        {
            int id = ObjectIDs.NextValue();
            _Drops.Add(id, drop);
            drop.ID = id;
        }
        public void RemoveDrop(Drop drop)
        {
            _Drops.Remove(drop.ID);
            drop = null;
        }

        public void AddSeat(Seat ST)
        {
            _Seat.Add(ST.ID, ST);
        }

        public uint makeNPCID(uint id)
        {
            return (id - NpcStart);
        }

        public uint makeNPCID()
        {
            return (uint)(Life["n"].Count - 1 + NpcStart);
        }

        public uint makeReactorID(uint id)
        {
            return (id - ReactorStart);
        }

        public uint makeReactorID()
        {
            return (uint)(Life["r"].Count - 1 + ReactorStart);
        }

        public void AddFoothold(Foothold FH)
        {
            _FHs.Add(FH);
        }

        public void AddLife(Life LF)
        {
            _Life[LF.Type].Add(LF);
            if (LF.Type == "n") LF.SpawnID = makeNPCID();
            else if (LF.Type == "r") LF.SpawnID = makeReactorID();
            else LF.SpawnID = (uint)(_Life[LF.Type].Count + 2);

            if (LF.Type == "m")
            {
                spawnMob((int)LF.SpawnID, LF);
            }
        }

        public void AddPortal(Portal PT)
        {
            if (PT.Name == "sp")
            {
                _SpawnPoints.Add(PT.ID, PT);
            }
            else if (PT.Name == "tp")
            {
                // TownPortal: Mystic Door?
            }
            else
            {
                if (_Portal.ContainsKey(PT.Name))
                {
                    Logger.WriteLog(Logger.LogTypes.Warning, "Duplicate portal, Name {0}, Map ID: {1}.", PT.Name, ID);
                }
                else
                {
                    _Portal.Add(PT.Name, PT);
                }
            }
        }

        public Character GetPlayer(int id)
        {
            try
            {
                Character ret = null;
                Characters.ForEach(c => { if (c.Id == id) ret = c; });
                return ret;
            }
            catch { }
            return null;
        }

        public void RemovePlayer(Character chr)
        {
            if (Characters.Contains(chr))
            {
                Characters.Remove(chr);

                UpdateMobControl(chr);
                //PetsPacket.SendRemovePet(chr);
                Characters.ForEach(p => {
                    if (p != chr)
                        p.Client.SendPacket(MapPacket.CharacterLeave(chr.Id));
                });
            }
        }

        public void AddPlayer(Character chr)
        {
            Characters.Add(chr);
            ShowObjects(chr);
        }

        public void SendPacket(Packet pPacket)
        {
            SendPacket(pPacket, null, false);
        }

        public void SendPacket(Packet pPacket, Character skipme, bool log)
        {
            Characters.ForEach(p =>
            {
                if (p != skipme)
                    p.Client.SendPacket(pPacket);
                else if (log)
                    Console.WriteLine("Not sending packet to charid {0} (skipme: {1})", p.Id, skipme.Id);
            });
        }

        public void ShowObjects(Character pCharacter)
        {
            pCharacter.Client.SendPacket(MapPacket.CharacterEnter(pCharacter));
            Characters.ForEach(p =>
            {
                if (p != pCharacter)
                {
                    p.Client.SendPacket(MapPacket.CharacterEnter(pCharacter));
                }
            });
            Life["n"].ForEach(n => {
                pCharacter.Client.SendPacket(MapPacket.SpawnNPC(n));
            });

            Mobs.ForEach(m =>
            {
                if (m.HP != 0)
                {
                    if (m.ControlStatus == MobControlStatus.ControlNone)
                    {
                        UpdateMobControl(m, true, pCharacter);
                    }
                    else
                    {
                        pCharacter.Client.SendPacket(MobPacket.SpawnMonster(m, 0, 0, false));
                        UpdateMobControl(m, false, null);
                    }
                }
                else
                {
                    Console.WriteLine("Mob is dead.");
                }
            });

            foreach (KeyValuePair<int, Drop> drop in Drops)
            {
                drop.Value.ShowDrop(pCharacter);
            }

            //if (WeatherID != 0) MapPacket.SendWeatherEffect(ID, chr);
        }

        public bool MakeWeatherEffect(int itemID, string message, TimeSpan time, bool admin = false)
        {
            if (WeatherID != 0) return false;
            WeatherID = itemID;
            WeatherMessage = message;
            WeatherAdmin = admin;
            //MapPacket.SendWeatherEffect(ID);
            //TimerFunctions.AddTimedFunction(delegate { StopWeatherEffect(); }, time, new TimeSpan(), BetterTimerTypes.Weather, ID, 0);
            return true;
        }

        public void StopWeatherEffect()
        {
            WeatherID = 0;
            WeatherMessage = "";
            WeatherAdmin = false;
            //MapPacket.SendWeatherEffect(ID);
        }

        public int KillAllMobs(Character chr, bool damage)
        {
            int amount = 0;

            try
            {
                List<Mob> mobsBackup = new List<Mob>(Mobs);

                lock (mobsBackup)
                {
                    foreach (Mob mob in mobsBackup)
                    {
                        if (mob != null)
                        {
                            if (damage)
                                //MobPacket.SendMobDamageOrHeal(chr, mob.SpawnID, mob.HP, false);
                                if (mob.giveDamage(chr, mob.HP))
                                {
                                    mob.CheckDead();
                                    amount++;
                                }
                        }
                    }
                }
                mobsBackup.Clear();
                mobsBackup = null;
            }
            catch { }
            return amount;
        }


        public void RemoveMob(Mob mob)
        {
            _Mobs.Remove(mob);
        }

        public void RespawnMob(int spawnid)
        {
            Mob mob = GetMob(spawnid);
            mob.DoesRespawn = true;
            //MobPacket.SendMobSpawn(null, mob, 0, null, true, false);

            UpdateMobControl(mob, true, null);
        }

        public int spawnMobNoRespawn(int mobid, Pos position, short foothold)
        {
            int id = ObjectIDs.NextValue();
            Mob mob = new Mob(id, ID, mobid, position, foothold, MobControlStatus.ControlNormal);
            mob.DoesRespawn = false;
            _Mobs.Add(mob);
            //MobPacket.SendMobSpawn(null, mob, 0, null, true, false);

            UpdateMobControl(mob, true, null);
            return id;
        }

        public int spawnMobNoRespawn(int mobid, Pos position, short foothold, Mob owner, byte summonEffect)
        {
            int id = ObjectIDs.NextValue();
            Mob mob = new Mob(id, ID, mobid, position, foothold, MobControlStatus.ControlNormal);
            mob.DoesRespawn = false;
            _Mobs.Add(mob);
            if (summonEffect != 0)
            {
                mob.Owner = owner;
                if (owner != null)
                {
                    mob.DoesRespawn = false; // Is mob of owner
                    owner.SpawnedMobs.Add(id, mob);
                }
            }
            //MobPacket.SendMobSpawn(null, mob, summonEffect, owner, (owner == null), false);

            UpdateMobControl(mob, true, null);
            return id;
        }

        public int spawnMob(int mobid, Pos position, short foothold, Mob owner, byte summonEffect)
        {
            int id = ObjectIDs.NextValue();
            Mob mob = new Mob(id, ID, mobid, position, foothold, MobControlStatus.ControlNormal);
            _Mobs.Add(mob);
            if (summonEffect != 0)
            {
                mob.Owner = owner;
                if (owner != null)
                {
                    mob.DoesRespawn = false; // Is mob of owner
                    owner.SpawnedMobs.Add(id, mob);
                }
            }
            //MobPacket.SendMobSpawn(null, mob, summonEffect, owner, (owner == null), false);

            UpdateMobControl(mob, true, null);
            return id;
        }

        public int spawnMob(int spawnid, Life life)
        {
            int id = ObjectIDs.NextValue();
            Mob mob = new Mob(id, ID, life.ID, new Pos((short)life.X, (short)life.Y), (short)life.Foothold, MobControlStatus.ControlNormal);
            mob.SetSpawnData(life);
            _Mobs.Add(mob);
            SendPacket(MobPacket.SpawnMonster(mob, 0, 0, false));
            UpdateMobControl(mob, true, null);
            return id;
        }


        public void UpdateMobControl(Character who)
        {
            Mobs.ForEach(m => { if (m.Controller == who) UpdateMobControl(m, false, null); });
        }

        public void UpdateMobControl(Mob mob, bool spawn, Character chr)
        {
            int maxpos = 200000;
            foreach (Character c in Characters)
            {
                int curpos = mob.Position - c.Position;
                if (curpos < maxpos)
                {
                    mob.setControl(c, spawn, chr);
                    return;
                }
            }
            mob.setControl(null, spawn, chr);
        }


        public Pos FindFloor(Pos mainPos)
        {
            short x = mainPos.X;
            short y = (short)(mainPos.Y - 100);
            short maxy = mainPos.Y;

            bool firstCheck = true;

            for (int i = 0; i < Footholds.Count; i++)
            {
                Foothold fh = Footholds[i];
                if ((x > fh.X1 && x <= fh.X2) || (x > fh.X2 && x <= fh.X1))
                {
                    short cmax = (short)((float)(fh.Y1 - fh.Y2) / (fh.X1 - fh.X2) * (x - fh.X1) + fh.Y1);
                    if ((cmax <= maxy || (maxy == mainPos.Y && firstCheck)) && cmax >= y)
                    {
                        maxy = cmax;
                        firstCheck = false;
                    }
                }
            }
            return new Pos(x, maxy);
        }

    }

    public class Foothold
    {
        public ushort ID { get; set; }
        public short X1 { get; set; }
        public short Y1 { get; set; }
        public short X2 { get; set; }
        public short Y2 { get; set; }
    }

    public class Life
    {
        public int ID { get; set; }
        public uint SpawnID { get; set; }
        public string Type { get; set; }
        public int RespawnTime { get; set; }
        public ushort Foothold { get; set; }
        public bool FacesLeft { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short Cy { get; set; }
        public short Rx0 { get; set; }
        public short Rx1 { get; set; }
        public byte Hide { get; set; }
    }

    public class Portal
    {
        public byte ID { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public string Name { get; set; }
        public int ToMapID { get; set; }
        public string ToName { get; set; }
    }

    public class Seat
    {
        public byte ID { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
    }
}
