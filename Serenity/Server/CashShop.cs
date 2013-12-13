using Serenity.Connection;
using Serenity.Packets;
using Serenity.Packets.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Servers
{
    public class CashShop
    {
        public ushort Port { get; private set; }

        public List<Client> Clients { get; private set; }
        private PacketProcessor Processor;
        private Acceptor Acceptor;

        public CashShop(ushort pPort)
        {
            this.Port = pPort;

            Clients = new List<Client>();
            Processor = new PacketProcessor("CashShop");
            Acceptor = new Acceptor();
        }

        public void Initalize()
        {
            //Console.WriteLine("CashShop Server listening on Port {0}.", this.Port);
            Acceptor.Start(this.Port, true);
            Acceptor.OnSocketAccept += new EventHandler<SocketEventArgs>(OnClientAccepted);

            Processor.AppendHandler((byte)RecvOpcodes.Heartbeat, GeneralHandler.HandleHeartbeat);
            Processor.AppendHandler((byte)RecvOpcodes.Migrate, InterserverHandler.HandleMigration);
        }

        private void OnClientAccepted(object sender, SocketEventArgs e)
        {
            Console.WriteLine("[{0}] Accepted connection from {1}.", "CashShop", e.Socket.RemoteEndPoint.ToString());
            Client Client = new Client(e.Socket, 0, 0, "CashShop");

            Client.SendHandshake(Constants.MajorVersion, Constants.MinorVersion, Constants.Locale);

            Clients.Add(Client);
        }

        public void OnClientDisconnected(Client pClient)
        {
            Console.WriteLine("[{0}] Lost connection from {1}.", "CashShop", pClient.IP);

            Clients.Remove(pClient);
            pClient = null;
        }

        public void OnPacketInbound(Client pClient, Packet pPacket)
        {
            byte Header = pPacket.ReadByte();

            PacketHandler Handler = Processor[Header];

            if (Handler != null)
            {
                Console.WriteLine("[{0}] Handling 0x{1:X4} with {2}.", "CashShop", Header.ToString("X"), Handler.Method.Name);
                Handler(pClient, pPacket);
            }
            else
            {
                Console.WriteLine("[{0}] Unhandled 0x{1:X4} from {2}.", "CashShop", Header.ToString("X"), pClient.IP);
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
