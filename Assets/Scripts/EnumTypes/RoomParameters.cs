using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EnumTypes
{
    /// <summary>
    /// Clase que gestiona los parametros de la habitacion a crear en LevelGeneration.cs
    /// </summary>
    public class RoomParameters
    {
        EnumTypeRoom typeRoom;
        Dictionary<LevelGeneration.doorDirection, EnumTypeDoor> roomDoors = new Dictionary<LevelGeneration.doorDirection, EnumTypeDoor>
        {
            { LevelGeneration.doorDirection.up,EnumTypeDoor.none},
            { LevelGeneration.doorDirection.down,EnumTypeDoor.none},
            { LevelGeneration.doorDirection.left,EnumTypeDoor.none},
            { LevelGeneration.doorDirection.right,EnumTypeDoor.none},
        };
        bool roomGenerated = false;


        public EnumTypeRoom TypeRoom { get => typeRoom; set => typeRoom = value; }
        public Dictionary<LevelGeneration.doorDirection, EnumTypeDoor> RoomDoors { get => roomDoors; set => roomDoors = value; }
        public bool RoomGenerated { get => roomGenerated; set => roomGenerated = value; }

        public RoomParameters (EnumTypeRoom TypeRoom, bool RoomGenerated)
        {
            typeRoom = TypeRoom;
            roomGenerated = RoomGenerated;
        }

        public void SetDoorTypeByDirection(LevelGeneration.doorDirection currentDoorDirection, EnumTypeDoor newTypeDoor )
        {
            roomDoors[currentDoorDirection] = newTypeDoor;
        }
    }
}
