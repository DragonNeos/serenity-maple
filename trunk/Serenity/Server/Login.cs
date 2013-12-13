using Serenity;
using Serenity.Connection;
using Serenity.Other;
using Serenity.Packets;
using Serenity.Packets.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Servers
{
    public class Login
    {
        public List<Client> Clients { get; private set; }
        private PacketProcessor Processor;
        private Acceptor Acceptor;

        public Login()
        {
            Clients = new List<Client>();
            Processor = new PacketProcessor("Login");
            Acceptor = new Acceptor();
        }

        public void Initalize()
        {
            Acceptor.Start(8484, true);
            Acceptor.OnSocketAccept += new EventHandler<SocketEventArgs>(OnClientAccepted);

            Processor.AppendHandler((byte)RecvOpcodes.Validate, GeneralHandler.HandleValidate);
            Processor.AppendHandler((byte)RecvOpcodes.Heartbeat, GeneralHandler.HandleHeartbeat);
            Processor.AppendHandler((byte)RecvOpcodes.Start, LoginHandler.HandleStart);
            Processor.AppendHandler((byte)RecvOpcodes.Login, LoginHandler.HandleLogin);
            Processor.AppendHandler((byte)RecvOpcodes.World_Info_Request, LoginHandler.HandleWorldsRequest);
            Processor.AppendHandler((byte)RecvOpcodes.World_Info_Rerequest, LoginHandler.HandleWorldsRequest);
            Processor.AppendHandler((byte)RecvOpcodes.Check_User_Limit, LoginHandler.HandleCheckUserLimit);
            Processor.AppendHandler((byte)RecvOpcodes.World_Select, LoginHandler.HandleWorldSelect);
            Processor.AppendHandler((byte)RecvOpcodes.Check_Name_Duplicate, LoginHandler.HandleCheckNameDuplicate);
            Processor.AppendHandler((byte)RecvOpcodes.Create_Character, LoginHandler.HandleCreateCharacter);
            Processor.AppendHandler((byte)RecvOpcodes.Select_Character, LoginHandler.HandleSelectCharacter);

            //Console.WriteLine("Login Server listening on Port {0}.", 8484);
        }

        public void OnClientAccepted(object sender, SocketEventArgs e)
        {
            Console.WriteLine("[{0}] Accepted connection from {1}.", "Login", e.Socket.RemoteEndPoint.ToString());
            Client Client = new Client(e.Socket, 0, 0, "Login");

            Client.SessionId = Randomizer.NextLong();

            Client.SendHandshake(Constants.MajorVersion, Constants.MinorVersion, Constants.Locale);

            Clients.Add(Client);
        }

        public void OnClientDisconnected(Client pClient)
        {
            Console.WriteLine("[{0}] Lost connection from {1}.", "Login", pClient.IP);
            Clients.Remove(pClient);
            pClient = null;
        }

        public void OnPacketInbound(Client pClient, Packet pPacket)
        {
            byte Header = pPacket.ReadByte();

            PacketHandler Handler = Processor[Header];

            if (Handler != null)
            {
                Console.WriteLine("[{0}] Handling 0x{1:X4} with {2}.", "Login", Header.ToString("X"), Handler.Method.Name);
                Handler(pClient, pPacket);
            }
            else
            {
                Console.WriteLine("[{0}] Unhandled 0x{1:X4} from {2}.", "Login", Header.ToString("X"), pClient.IP);
                Console.WriteLine("Full packet: " + pPacket.ToString());
            }
        }
    }
}
