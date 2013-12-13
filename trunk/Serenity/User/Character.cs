using Serenity.Game.Objects;
using Serenity.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.User
{
    public partial class Character : MovableLife
    {
        public int Id { get; set; }
        public bool Loaded { get; set; }

        public Character()
        {
            SP = new byte[10];

            for (int i = 0; i < 10; i++)
                SP[i] = 0;
            Inventory = new Dictionary<InventoryType, Inventory>();
        }

        public Dictionary<InventoryType, Inventory> Inventory { get; private set; }

        public Client Client { get; set; }

        public int AccountId { get; set; }
        public byte WorldId { get; set; }

        public string Name { get; set; }
        public byte Gender { get; set; }
        public byte SkinColor { get; set; }
        public int FaceId { get; set; }
        public int HairId { get; set; }

        public long Meso { get; set; }

        public byte Level { get; set; }
        public short Job { get; set; }
        public short Str { get; set; }
        public short Dex { get; set; }
        public short Int { get; set; }
        public short Luk { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }
        public short AP { get; set; }
        public byte[] SP { get; set; }
        public short ApHp { get; set; }
        public long EXP { get; set; }
        public int Fame { get; set; }
        public int FaceMarking { get; set; }
        public byte Fatigue { get; set; }
        public byte BattleRank { get; set; }
        public int BattlePoints { get; set; }
        public int BattleEXP { get; set; }

        public int MapId { get; set; }
        public byte MapPosition { get; set; }
        public byte Subcategory { get; set; }

        public byte PortalCount { get; set; }

        public Map CurrentMap
        {
            get
            {
                return Master.Instance.Worlds[Client.World].Channels[Client.Channel].Maps[MapId];
            }
        }

        public Dictionary<int, Map> ChannelMaps
        {
            get
            {
                return Master.Instance.Worlds[Client.World].Channels[Client.Channel].Maps;
            }
        }

        public int Ambition { get; set; }
        public int Insight { get; set; }
        public int Willpower { get; set; }
        public int Diligence { get; set; }
        public int Empathy { get; set; }
        public int Charm { get; set; }
        public short AmbitionGained { get; set; }
        public short InsightGained { get; set; }
        public short WillpowerGained { get; set; }
        public short DiligenceGained { get; set; }
        public short EmpathyGained { get; set; }
        public short CharmGained { get; set; }

        public int HonorLevel { get; set; }
        public int HonorEXP { get; set; }

        public void Load()
        {
            Master.Instance.Accessor.LoadCharacter(this);
        }

        public void Save()
        {
            Master.Instance.Accessor.SaveCharacter(this);
        }

        public void Create()
        {
            Master.Instance.Accessor.CreateCharacter(this);
        }
    }
}
