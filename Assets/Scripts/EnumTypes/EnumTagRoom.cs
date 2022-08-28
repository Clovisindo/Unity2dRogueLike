using Assets.Scripts.LevelDesign;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.EnumTypes
{
    [System.Serializable]
    public enum EnumTagRoom
    {
        potionsTreasure,
        switches,
        buttons,
        fountain,
        enemies,
        traps,
        treasures,
        key,
        mimics,
        unknown
    }

    public class TypeTagRoom
    {
        private EnumTagRoom enumTagRoom;
        private EnumTypeRange typeRange;

        public static readonly List<TypeTagRoom> types = new List<TypeTagRoom>()
        {
            new TypeTagRoom(EnumTagRoom.enemies, 0, 55),
            new TypeTagRoom(EnumTagRoom.traps, 56, 75),
            new TypeTagRoom(EnumTagRoom.fountain, 76, 100),

            new TypeTagRoom(EnumTagRoom.potionsTreasure, 101, 120),
            new TypeTagRoom(EnumTagRoom.switches, 121, 140),
            new TypeTagRoom(EnumTagRoom.buttons, 141, 160),
            new TypeTagRoom(EnumTagRoom.treasures, 161, 180),
            new TypeTagRoom(EnumTagRoom.mimics, 191, 200),
        };

        public EnumTagRoom EnumTagRoom { get => enumTagRoom; }
        public EnumTypeRange TypeRange { get => typeRange; set => typeRange = value; }

        TypeTagRoom(EnumTagRoom enumTagRoom, int minProbRange, int maxProbRange)
        {
            this.enumTagRoom = enumTagRoom;
            this.typeRange = new EnumTypeRange(minProbRange, maxProbRange);
        }

        public static int GetTagRoomByRandomRange(List<DesignLevelParameters> listDL)
        {
            var availableTagRooms = types.Where(t => listDL.Select(d => d.tagRoom).Contains(t.EnumTagRoom)).ToList();

            return EnumTypeRange.RandomValueFromRanges(availableTagRooms.Select(tag => tag.typeRange).ToArray());
        }
    }
}

