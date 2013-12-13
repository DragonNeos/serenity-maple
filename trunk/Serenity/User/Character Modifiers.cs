using Serenity.Game.Objects;
using Serenity.Packets;
using Serenity.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.User
{
    public partial class Character
    {
        public void ChangeMap(int pMapId)
        {
            Map Map = ChannelMaps[MapId];
            Map NewMap = ChannelMaps[pMapId];

            Map.RemovePlayer(this);
            PortalCount++;
            MapId = pMapId;

            Portal Portal;
            Random Random = new Random();
            Portal = NewMap.SpawnPoints[Random.Next(0, NewMap.SpawnPoints.Count)];
            MapPosition = Portal.ID;
            Position = new Pos(Portal.X, (short)(Portal.Y - 40));
            Stance = 0;
            Foothold = 0;

            Client.SendPacket(MapPacket.EnterField(this));

            NewMap.AddPlayer(this);
        }

        public void ChangeMap(int pMapId, Portal pTo)
        {
            Map Map = ChannelMaps[MapId];
            Map NewMap = ChannelMaps[pMapId];

            Map.RemovePlayer(this);
            PortalCount++;
            MapId = pMapId;

            MapPosition = pTo.ID;

            Position = new Pos(pTo.X, (short)(pTo.Y - 40));
            Stance = 0;
            Foothold = 0;

            Client.SendPacket(MapPacket.EnterField(this));

            NewMap.AddPlayer(this);
        }

        public void ModifyHP(short pValue, bool pPacket = true)
        {
            if ((HP + pValue) < 0)
            {
                HP = 0;
            }
            else if ((HP + pValue) > MaxHP)
            {
                HP = MaxHP;
            }
            else
            {
                HP = (short)(HP + pValue);
            }

            if (pPacket)
            {
                Client.SendPacket(CharacterStatsPacket.StatChange(this, (uint)CharacterStatsPacket.Constants.HP, (short)HP));
            }

            ModifiedHP();
        }

        public void ModifiedHP()
        {
            if (HP == 0)
            {
                // Lose EXP and Remove summons.
            }
        }
    }
}
