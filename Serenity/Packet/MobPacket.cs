using Serenity.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets
{
    public static class MobPacket
    {
        public static Packet SpawnMonster(Mob pMob, int pSpawnType, int pLink, bool pAzwan)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Monster_Spawn);
            p.WriteInt(pMob.SpawnID);
            p.WriteByte(1);
            p.WriteInt(pMob.MobID);
            MobPacketHelper.AddMonsterStatus(p, pMob);
            p.WriteShort(pMob.Position.X);
            p.WriteShort(pMob.Position.Y);
            p.WriteByte(pMob.Stance); // Stance, TODO.
            p.WriteShort(pMob.Foothold);
            p.WriteShort(pMob.OriginalFoothold);
            p.WriteByte((byte)pSpawnType);
            if (pSpawnType == -3 || pSpawnType >= 0)
                p.WriteInt(pLink);
            p.WriteByte(0); // TODO: Carnival.
            p.WriteInt(125);
            p.WriteZero(16);
            p.WriteByte(0);
            p.WriteInt(-1);
            p.WriteInt(0);
            p.WriteInt(46);
            p.WriteInt(0);
            p.WriteByte(0);
            p.WriteByte(0xFF);

            return p;
        }
    }

    public static class MobPacketHelper
    {
        public static void AddMonsterStatus(Packet pPacket, Mob pMob)
        {
            pPacket.WriteByte(0);
            pPacket.WriteZero(40);
            pPacket.WriteShort(5088);
            pPacket.WriteShort(72);
            pPacket.WriteZero(3);
            pPacket.WriteByte(136);
            for (int i = 0; i < 4; i++)
            {
                pPacket.WriteLong(0);
                pPacket.WriteHexString("30 3B");
            }
            pPacket.WriteZero(7);
            pPacket.WriteZero(12);
        }
    }
}
