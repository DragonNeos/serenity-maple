using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Game.Data
{
    public enum BuffStat
    {
        //menu 1
        WATK = 0 + 0x1F * (1 - 1),
        WDEF = 1 + 0x1F * (1 - 1),
        MATK = 2 + 0x1F * (1 - 1),
        MDEF = 3 + 0x1F * (1 - 1),
        // if buffmenu isnt 1 we add +1

        //menu 2
        UNKNOWN2 = 0 + 0x1F * (2 - 1) + 1,
        UNKNOWN22 = 1 + 0x1F * (2 - 1) + 1,

        //menu 3
        UNKNOWN3 = 0 + 0x1F * (3 - 1) + 1,
        UNKNOWN33 = 4 + 0x1F * (3 - 1) + 1,

        //menu 4
        UNKNOWN4 = 0 + 0x1F * (4 - 1) + 1,
        UNKNOWN44 = 7 + 0x1F * (4 - 1) + 1,
    }

    public static class BuffTest
    {
        public static int getBuffStat(int val)
        {
            if (val > 0x1F)
                val -= 0x1F * (val / 0x1F - 1);
            return 1 << val;
        }
    }
}
