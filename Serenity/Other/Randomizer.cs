using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Other
{
    public static class Randomizer
    {
        private static Random sRandom = new Random();

        public static void NextBytes(byte[] bytes)
        {
            sRandom.NextBytes(bytes);
        }

        public static int NextInt(int ints)
        {
            return sRandom.Next(ints);
        }

        public static long NextLong()
        {
            byte[] buf = new byte[8];
            sRandom.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return longRand;
        }

        public static int Generate()
        {
            return sRandom.Next();
        }
        public static int Generate(int high)
        {
            return sRandom.Next(high);
        }
        public static int Generate(int low, int high)
        {
            return sRandom.Next(low, high);
        }
    }
}
