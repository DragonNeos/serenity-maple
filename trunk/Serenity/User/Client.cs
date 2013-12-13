using Serenity;
using Serenity.Packets;
using Serenity.Servers;
using Serenity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serenity
{
    public class Client : Session
    {
        public Session Session;
        public Account Account;

        public long SessionId { get; set; }

        public string Type { get; set; }

        public byte World { get; set; }
        public byte Channel { get; set; }

        public Character Character { get; set; }

        public static Session SessionTemp;

        public Client(Socket pSocket, byte pWorld, byte pChannel, string pType) :
            base(pSocket, pSocket.RemoteEndPoint.ToString())
        {
            this.World = pWorld;
            this.Channel = pChannel;
            this.Type = pType;
        }

        public override void OnDisconnect(Session pSession)
        {
            if (Type.Equals("Login"))
                Master.Instance.Login.OnClientDisconnected(this);
            else if (Type.Equals("Channel"))
                Master.Instance.Worlds[World].Channels[Channel].OnClientDisconnected(this);
            else if (Type.Equals("CashShop"))
                Master.Instance.CashShop.OnClientDisconnected(this);
        }

        public override void OnPacketInbound(Session pSession, Packet pPacket)
        {
            if (Type.Equals("Login"))
                Master.Instance.Login.OnPacketInbound(this, pPacket);
            else if (Type.Equals("Channel"))
                Master.Instance.Worlds[World].Channels[Channel].OnPacketInbound(this, pPacket);
            else if (Type.Equals("CashShop"))
                Master.Instance.CashShop.OnPacketInbound(this, pPacket);
        }

        public override void SendPacket(Packet pPacket)
        {
            base.SendPacket(pPacket);
        }
    }
}
