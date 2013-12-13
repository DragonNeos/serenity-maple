using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Other
{
    public static class Tools
    {
        public static int HeartbeatAlgorithm(int pRequest)
        {
            int Response;

            Response = ((pRequest >> 5) << 5) + (((((pRequest & 0x1F) >> 3) ^ 2) << 3) + (7 - (pRequest & 7)));
            Response |= ((pRequest >> 7) << 7);
            Response -= 2;

            return Response;
        }

        private static readonly DateTime Jan1st1970 = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public static long GetTime(long realTimestamp)
        {
            if (realTimestamp == -1L)
            {
                return 150842304000000000L;
            }
            if (realTimestamp == -2L)
            {
                return 94354848000000000L;
            }
            if (realTimestamp == -3L)
            {
                return 150841440000000000L;
            }
            return realTimestamp * 10000L + 116444592000000000L;
        }
    }
}
