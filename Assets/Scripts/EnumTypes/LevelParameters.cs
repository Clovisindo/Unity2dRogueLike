using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EnumTypes
{
    public class LevelParameters
    {
        EnumTypeRoom typeRoom;
        bool roomGenerated;

        public EnumTypeRoom TypeRoom { get => typeRoom; set => typeRoom = value; }
        public bool RoomGenerated { get => roomGenerated; set => roomGenerated = value; }

        public LevelParameters ( EnumTypeRoom TypeRoom, bool RoomGenerated)
        {
            typeRoom = TypeRoom;
            roomGenerated = RoomGenerated;
        }

    }
}
