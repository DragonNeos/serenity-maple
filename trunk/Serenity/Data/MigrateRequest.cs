using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Data
{
    public class MigrateRequest
    {
        public DateTime Expiry { get; private set; }
        //public string Endpoint { get; private set; }
        public int CharacterId { get; private set; }
        public long SessionId { get; private set; }
        public bool CashShop { get; private set; }

        public MigrateRequest(/*string endpoint,*/int charId, long sessionId, bool cashShop)
        {
            Expiry = DateTime.Now;
            //Endpoint = endpoint;
            CharacterId = charId;
            SessionId = sessionId;
            CashShop = cashShop;
        }
    }
}
