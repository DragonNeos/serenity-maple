using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Game
{
    public interface LifeMovement : LifeMovementFragment
    {
        int GetNewState();

        int GetDuration();

        int GetType();
    }
}
