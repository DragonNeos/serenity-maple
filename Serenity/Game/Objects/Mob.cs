using Serenity.Data;
using Serenity.Servers;
using Serenity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Game.Objects
{
    public enum MobControlStatus
    {
        ControlNormal,
        ControlNone
    }

    public class Mob : MovableLife
    {
        public Life SpawnData { get; set; }
        public int MobID { get; set; }
        public int MapID { get; set; }
        public int SpawnID { get; set; }
        public int EXP { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public Mob Owner { get; set; }
        public Character Controller { get; set; }
        public short OriginalFoothold { get; set; }
        public Dictionary<int, ulong> Damages { get; set; }
        public MobControlStatus ControlStatus { get; set; }
        public float AllowedSpeed { get; set; }
        public DateTime lastMove { get; set; }
        public bool DoesRespawn { get; set; }
        public bool IsDead { get; set; }
        public Pos OriginalPosition { get; set; }

        public Map Map { get; set; }

        public Dictionary<byte, DateTime> LastSkillUse { get; set; }
        public Dictionary<int, Mob> SpawnedMobs { get; set; }

        private int DeadsInFiveMinutes { get; set; }

        internal Mob(int id, int mapid, int mobid, Pos position, int spawnid, byte direction, short foothold, MobControlStatus controlStatus) :
            base(foothold, position, 2)
        {
            Damages = new Dictionary<int, ulong>();
            OriginalFoothold = foothold;
            MobID = mobid;
            MapID = mapid;
            SpawnID = id;
            ControlStatus = controlStatus;
            DoesRespawn = true;
            OriginalPosition = position;
            Position = position;
            DeadsInFiveMinutes = 0;
            Init();
        }

        internal Mob(int id, int mapid, int mobid, Pos position, short foothold, MobControlStatus controlStatus) :
            base(foothold, position, 2)
        {
            Damages = new Dictionary<int, ulong>();
            OriginalFoothold = foothold;
            MobID = mobid;
            MapID = mapid;
            SpawnID = id;
            ControlStatus = controlStatus;
            DoesRespawn = true;
            OriginalPosition = position;
            Position = position;
            DeadsInFiveMinutes = 0;
            Init();
        }

        public void Clearup()
        {
            //RunTimedFunction.RemoveTimer(BetterTimerTypes.MobKillTimer, MapID, SpawnID);
            //OriginalPosition = null;
            //Owner = null;
            //Controller = null;
            //Damages.Clear();
        }

        public void SetSpawnData(Life sd) { SpawnData = sd; }

        public void InitAndSpawn()
        {
            Init();

           // MobPacket.SendMobSpawn(null, this, 0, null, true, false);

            //DataProvider.Maps[MapID].UpdateMobControl(this, true, null);
        }

        public void Init()
        {
            IsDead = false;

            if (LastSkillUse != null)
                LastSkillUse.Clear();
            else
                LastSkillUse = new Dictionary<byte, DateTime>();

            if (SpawnedMobs != null)
                SpawnedMobs.Clear();
            else
                SpawnedMobs = new Dictionary<int, Mob>();

            MobData data = Master.Instance.DataProvider.Mobs[MobID];
            HP = data.MaxHP;
            MP = data.MaxMP;
            EXP = data.EXP;
            Owner = null;
            Controller = null;
            AllowedSpeed = (100 + data.Speed) / 100.0f;
            lastMove = DateTime.Now;

            //RunTimedFunction.AddTimedFunction(delegate
            //{
            //    if (DeadsInFiveMinutes > 0)
            //        DeadsInFiveMinutes--;
            //}, new TimeSpan(), new TimeSpan(0, 5, 0), BetterTimerTypes.MobKillTimer, MapID, SpawnID);
        }

        public bool giveDamage(Character fucker, int amount)
        {
            //if (HP == 0) return false;
            //if (!Damages.ContainsKey(fucker.mID))
            //    Damages.Add(fucker.mID, 0);
            //Damages[fucker.mID] += (ulong)amount;

            //if (HP < amount)
            //    HP = 0;
            //else
            //    HP -= amount;

            return true;
        }

        public void CheckDead(Pos killPos = null)
        {
            //    if (HP == 0 && !IsDead)
            //    {
            //        IsDead = true;
            //        if (killPos != null) mPosition = killPos;
            //        setControl(null, false, null);
            //        MobPacket.SendMobDeath(this, 1);
            //        Character maxDmgChar = null;
            //        ulong maxDmgAmount = 0;
            //        MobData md = DataProvider.Mobs[MobID];
            //        DeadsInFiveMinutes++;

            //        foreach (KeyValuePair<int, ulong> dmg in Damages)
            //        {
            //            if (maxDmgAmount < dmg.Value && Server.CharacterList.ContainsKey(dmg.Key))
            //            {
            //                Character chr = Server.CharacterList[dmg.Key];
            //                if (chr.mMap == MapID)
            //                {
            //                    maxDmgAmount = dmg.Value;
            //                    maxDmgChar = chr;
            //                }
            //            }
            //        }
            //        if (maxDmgChar != null)
            //        {
            //            maxDmgChar.AddEXP((uint)EXP);
            //        }

            //        DropPacket.HandleDrops(maxDmgChar, MapID, Constants.getDropName(MobID, true), SpawnID, mPosition, false, false, false);

            //        foreach (int mobid in md.Revive)
            //        {
            //            DataProvider.Maps[MapID].spawnMobNoRespawn(mobid, mPosition, mFoothold);
            //        }

            //        Damages.Clear();

            //        if (DoesRespawn)
            //        {
            //            mPosition = OriginalPosition;
            //            mFoothold = OriginalFoothold;
            //            int derp = (int)(((((SpawnData == null ? 10 : SpawnData.RespawnTime + 1) / DataProvider.Maps[MapID].MobRate)) * DeadsInFiveMinutes) * 5);
            //            TimeSpan respawn = new TimeSpan(0, 0, derp);

            //            RunTimedFunction.AddTimedFunction(delegate
            //            {
            //                InitAndSpawn();
            //            }, respawn, new TimeSpan(), BetterTimerTypes.MobRespawn, MapID, SpawnID);
            //        }
            //        else
            //        {
            //            Clearup();
            //            DataProvider.Maps[MapID].RemoveMob(this);
            //        }
            //    }
        }

        public void setControl(Character control, bool spawn, Character display)
        {
            //Controller = control;
            //if (HP == 0) return;
            //if (control != null)
            //{
            //    MobPacket.SendMobRequestControl(control, this, spawn, null);
            //}
            //else if (ControlStatus == MobControlStatus.ControlNone)
            //{
            //    MobPacket.SendMobRequestControl(control, this, spawn, display);
            //}
        }

        public void endControl()
        {
            //if (Controller != null && Controller.mMap == MapID)
            //{
            //    MobPacket.SendMobRequestEndControl(Controller, this);
            //}
        }

        public void setControlStatus(MobControlStatus mcs)
        {
            //MobPacket.SendMobRequestEndControl(null, this);
            //MobPacket.SendMobSpawn(null, this, 0, null, false, false);
            //ControlStatus = mcs;
            //DataProvider.Maps[MapID].UpdateMobControl(this, false, null);
        }
    }
}
