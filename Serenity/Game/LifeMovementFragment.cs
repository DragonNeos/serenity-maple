using Serenity.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Game
{
    public interface LifeMovementFragment
    {
        void Serialize(Packet pPacket);

        Pos GetPosition(); 
    }
}
