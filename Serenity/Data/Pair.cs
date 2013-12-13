using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    class Pair<TLeft, TRight>
    {
        public Pair(TLeft pLeftValue, TRight pRightValue)
        {
            Left = pLeftValue;
            Right = pRightValue;
        }

        public TLeft Left { get; set; }
        public TRight Right { get; set; }

        public override bool Equals(object pObj)
        {
            if (pObj is Pair<Object, Object>)
            {
                var pairobj = pObj as Pair<Object, Object>;
                return pairobj.Left.Equals(this.Left) && pairobj.Right.Equals(this.Right);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() + Right.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Left: {0} ({1}); Right: {2} ({3})", Left.ToString(), Left.GetType().Name, Right.ToString(), Right.GetType().Name);
        }
    }
}

