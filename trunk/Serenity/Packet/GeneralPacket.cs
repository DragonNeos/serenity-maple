using Serenity.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets
{
    public static class GeneralPacket
    {
        public static Packet HeartbeatResponse(int pRequest)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Heartbeat_Response);
            p.WriteInt(Tools.HeartbeatAlgorithm(pRequest));

            return p;
        }

        public static Packet ChangeChannel(Client pClient, ushort pPort)
        {
            Packet p = new Packet();

            p.WriteShort((short)SendOpcodes.Channel_Change);
            p.WriteByte(1);
            p.WriteBytes(new byte[] { 8, 31, 99, 141 });
            p.WriteUShort(pPort);
            p.WriteByte(0);

            return p;
        }
    }
}
