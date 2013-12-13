using Serenity.Game.Objects;
using Serenity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serenity.Game
{
    public class MovementParse
    {
        public static List<LifeMovementFragment> ParseMovement(Packet pPacket, int pKind)
        {
            List<LifeMovementFragment> Res = new List<LifeMovementFragment>();
            byte CommandCount = pPacket.ReadByte();

            for (byte i = 0; i < CommandCount; i++)
            {
                byte Command = pPacket.ReadByte();
                switch (Command)
                {
                    case 0:
                    case 7:
                    case 14:
                    case 16:
                    case 44:
                    case 45:
                    case 46:
                        {
                            short X = pPacket.ReadShort();
                            short Y = pPacket.ReadShort();
                            short XWobble = pPacket.ReadShort();
                            short YWobble = pPacket.ReadShort();
                            short Unknown = pPacket.ReadShort();
                            short Foothold = 0, XOffset = 0, YOffset = 0;

                            if (Command == 14)
                                Foothold = pPacket.ReadShort();

                            if (Command != 44)
                            {
                                XOffset = pPacket.ReadShort();
                                YOffset = pPacket.ReadShort();
                            }

                            byte NewState = pPacket.ReadByte();
                            short Duration = pPacket.ReadShort();

                            AbsoluteLifeMovement ALM = new AbsoluteLifeMovement(Command, new Pos(X, Y), Duration, NewState);
                            //ALM.Unknown = Unknown;
                            //ALM.Foothold = Foothold;
                            //ALM.PixelsPerSecond = new Pos(XWobble, YWobble);
                            //ALM.Offset = new Pos(XOffset, YOffset);

                            Res.Add(ALM);

                            break;
                        }
                }
            }

            return Res;
        }

        public static void UpdatePosition(List<LifeMovementFragment> pMovement, Character pCharacter, short pYOffset)
        {
            if (pMovement == null)
            {
                return;
            }

            foreach (LifeMovementFragment Move in pMovement)
            {
                Pos Position = ((LifeMovement)Move).GetPosition();
                Position.Y += pYOffset;
                pCharacter.Position = Position;
                pCharacter.Stance = (byte)((LifeMovement)Move).GetNewState();
            }
        }
    }
}
