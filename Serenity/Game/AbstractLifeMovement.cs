using Serenity.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Game
{
    public abstract class AbstractLifeMovement : LifeMovement
    {
        private Pos Position;
        private int Duration;
        private int NewState;
        private int Type;

        public AbstractLifeMovement(int pType, Pos pPos, int pDuration, int pNewState) :
            base()
        {
            this.Type = pType;
            this.Position = pPos;
            this.Duration = pDuration;
            this.NewState = pNewState;
        }

        public int GetType()
        {
            return this.Type;
        }

        public int GetNewState()
        {
            return this.NewState;
        }

        public int GetDuration()
        {
            return this.NewState;
        }

        public Pos GetPosition()
        {
            return this.Position;
        }

        public void Serialize(Packet pPacket)
        {
            //pPacket.WriteByte((byte)GetType());
            //pPacket.WritePosition(GetPosition());
            //pPacket.WritePosition(this.PixelsPerSecond);
            //pPacket.WriteShort(this.Unknown);

            //if (GetType() == 14)
            //{
            //    pPacket.WriteShort(this.Foothold);
            //}

            //if (GetType() != 44)
            //{
            //    pPacket.WritePosition(this.Offset);
            //}

            //pPacket.WriteByte((byte)GetNewState());
            //pPacket.WriteShort((short)GetDuration());
        }
    }
}
