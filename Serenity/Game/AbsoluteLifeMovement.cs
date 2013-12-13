using Serenity.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Game
{
    public class AbsoluteLifeMovement : AbstractLifeMovement
    {
        private Pos PixelsPerSecond, Offset;
        private short Unknown, Foothold;

        public AbsoluteLifeMovement(int pType, Pos pPos, int pDuration, int pNewState) :
            base(pType, pPos, pDuration, pNewState)
        {
        }

        public void SetPixelsPerSecond(Pos pPos)
        {
            this.PixelsPerSecond = pPos;
        }

        public void SetOffset(Pos pWobble)
        {
            this.Offset = pWobble;
        }

        public void SetFoothold(short pFoothold)
        {
            this.Foothold = pFoothold;
        }

        public void SetUnknown(short pUnknown)
        {
            this.Unknown = pUnknown;
        }

        public short GetUnknown()
        {
            return this.Unknown;
        }

        public void SetDefault()
        {
            Unknown = 0;
            Foothold = 0;
            PixelsPerSecond = new Pos(0, 0);
            Offset = new Pos(0, 0);
        }

        public void Serialize(Packet pPacket)
        {
            pPacket.WriteByte((byte)GetType());
            pPacket.WritePosition(GetPosition());
            pPacket.WritePosition(this.PixelsPerSecond);
            pPacket.WriteShort(this.Unknown);

            if (GetType() == 14)
            {
                pPacket.WriteShort(this.Foothold);
            }

            if (GetType() != 44)
            {
                pPacket.WritePosition(this.Offset);
            }

            pPacket.WriteByte((byte)GetNewState());
            pPacket.WriteShort((short)GetDuration());
        }
    }
}
