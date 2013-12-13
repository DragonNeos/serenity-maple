using Serenity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Packets
{
    public delegate void PacketHandler(Client c, Packet p);

    public sealed class PacketProcessor
    {
        public string Label { get; private set; }

        private PacketHandler[] m_handlers;
        private int m_count;

        public int Count
        {
            get
            {
                return m_count;
            }
        }

        public PacketProcessor(string label)
        {
            Label = label;
            m_handlers = new PacketHandler[0xFFFF + 1];
        }

        public void AppendHandler(byte opcode, PacketHandler handler)
        {
            m_handlers[opcode] = handler;
            m_count++;
        }

        public PacketHandler this[byte opcode]
        {
            get
            {
                try
                {
                    return m_handlers[opcode];
                }
                catch
                {
                    Console.WriteLine("No handler for {0}.", opcode.ToString("X"));
                    return null;
                }
            }
        }
    }
}
