using Serenity.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Data
{
    class CheatInspector
    {
        public static bool CheckSpeed(Pos PixelsPerSecond, float pAllowedSpeed)
        {
            float speedMod = Math.Abs(PixelsPerSecond.X) / 125f;
            return speedMod < pAllowedSpeed + 0.1f;
        }
    }
}
