using Serenity;
using Serenity.Connection;
using Serenity.Data;
using Serenity.Game.Objects;
using Serenity.Other;
using Serenity.Packets;
using Serenity.Packets.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Servers
{
    public class Channel
    {
        public byte Id { get; private set; }
        public byte WorldId { get; set; }

        public ushort Port { get; private set; }

        public List<Client> Clients { get; private set; }
        private PacketProcessor Processor;
        private Acceptor Acceptor;

        public Dictionary<int, Map> Maps = new Dictionary<int, Map>();

        public Channel(byte pId, ushort pPort)
        {
            this.Id = pId;
            this.Port = pPort;

            Clients = new List<Client>();
            Processor = new PacketProcessor("Channel " + this.Id);
            Acceptor = new Acceptor();
        }

        public void Initalize()
        {
            Acceptor.Start(this.Port, true);
            Acceptor.OnSocketAccept += new EventHandler<SocketEventArgs>(OnClientAccepted);

            //Console.WriteLine("{0}'s Channel {1} listening on Port {2}.", Constants.WorldNames[this.WorldId], this.Id, this.Port);

            Processor.AppendHandler((byte)RecvOpcodes.Heartbeat, GeneralHandler.HandleHeartbeat);
            Processor.AppendHandler((byte)RecvOpcodes.Migrate, InterserverHandler.HandleMigration);
            Processor.AppendHandler((byte)RecvOpcodes.Change_Map, GameHandler.HandleChangeMap);
            Processor.AppendHandler((byte)RecvOpcodes.Player_Damage, GameHandler.HandlePlayerDamage);
            Processor.AppendHandler((byte)RecvOpcodes.Player_Chat, GameHandler.HandlePlayerChat);
            Processor.AppendHandler((byte)RecvOpcodes.Enter_CashShop, InterserverHandler.HandleEnterCS);
            Processor.AppendHandler((byte)RecvOpcodes.Change_Channel, InterserverHandler.HandleChannelChange);
            Processor.AppendHandler((byte)RecvOpcodes.Player_Movement, GameHandler.HandlePlayerMovement);
        }

        private void OnClientAccepted(object sender, SocketEventArgs e)
        {
            Console.WriteLine("[{0}] Accepted connection from {1}.", Constants.WorldNames[this.Id] + " Channel " + this.Id, e.Socket.RemoteEndPoint.ToString());
            Client Client = new Client(e.Socket, WorldId, Id, "Channel");

            Client.SendHandshake(Constants.MajorVersion, Constants.MinorVersion, Constants.Locale);

            Clients.Add(Client);
        }

        public void OnClientDisconnected(Client pClient)
        {
            Console.WriteLine("[{0}] Lost connection from {1}.", Constants.WorldNames[this.Id] + " Channel " +this.Id, pClient.IP);

            if (pClient.Character != null)
            {
                if (pClient.Character.CurrentMap.Characters.Contains(pClient.Character))
                    pClient.Character.CurrentMap.RemovePlayer(pClient.Character);

                pClient.Character.Save();
            }

            if (pClient.Account != null)
                pClient.Account.Save();

            Clients.Remove(pClient);
            pClient = null;
        }

        public void OnPacketInbound(Client pClient, Packet pPacket)
        {
            byte Header = pPacket.ReadByte();

            PacketHandler Handler = Processor[Header];

            if (Handler != null)
            {
                Console.WriteLine("[{0}] Handling 0x{1:X4} with {2}.", Constants.WorldNames[WorldId] + " Channel " + this.Id, Header.ToString("X"), Handler.Method.Name);
                Handler(pClient, pPacket);
            }
            else
            {
                Console.WriteLine("[{0}] Unhandled 0x{1:X4} from {2}.", Constants.WorldNames[WorldId] + " Channel " + this.Id, Header.ToString("X"), pClient.IP);
            }
        }

        public int Load
        {
            get
            {
                return Clients.Count;
            }
        }

        public bool Contains(Client pClient)
        {
            return Clients.Contains(pClient);
        }
    }
}
