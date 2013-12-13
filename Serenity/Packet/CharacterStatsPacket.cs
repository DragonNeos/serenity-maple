using Serenity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets
{
    public static class CharacterStatsPacket
    {
        public enum Constants : uint
        {
            Skin = 0x01,
            Eyes = 0x02,
            Hair = 0x04,
            Pet = 0x08,
            Level = 0x10,
            Job = 0x20,
            Str = 0x40,
            Dex = 0x80,
            Int = 0x100,
            Luk = 0x200,
            HP = 0x400,
            MaxHP = 0x800,
            MP = 0x1000,
            MaxMP = 0x2000,
            AP = 0x4000,
            SP = 0x8000,
            Exp = 0x10000,
            Fame = 0x20000,
            Mesos = 0x40000
        };

        public static Packet StatChange(Character pCharacter, uint pFlag, short pValue, bool pSelf = false)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Stats_Update);
            p.WriteBool(pSelf);
            p.WriteUInt(pFlag);
            p.WriteShort(pValue);

            return p;
        }
    }
}
