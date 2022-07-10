using Assets.Scripts.EnumTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.LevelDesign
{
    [System.Serializable]
    public class DesignLevelParameters
    {
        public EnumTypeRoom typeRoom;
        public EnumDificultyRoom dificultyRoom;
        public EnumTagRoom tagRoom;

        public string[] enemiesJson;
        public string[] puzzlesJson;

        private List<Enemy> arrayEnemies;
        private List<fFloorMechanic> arrayPuzzlePieces;

        public DesignLevelParameters()
        {
            arrayEnemies = new List<Enemy>();
            arrayPuzzlePieces = new List<fFloorMechanic>();
        }

        public DesignLevelParameters(DesignLevelParameters newDesignLD)
        {
            typeRoom = newDesignLD.typeRoom;
            dificultyRoom = newDesignLD.dificultyRoom;
            tagRoom = newDesignLD.tagRoom;
            enemiesJson = newDesignLD.enemiesJson;
            puzzlesJson = newDesignLD.puzzlesJson;
            arrayEnemies = new List<Enemy>();
            arrayPuzzlePieces = new List<fFloorMechanic>();
        }

        public void SetTypeRoomParameter(EnumTypeRoom newTypeRoom)
        {
            typeRoom = newTypeRoom;
        }

        public void AddEnemy(Enemy newEnemy)
        {
            arrayEnemies.Add(newEnemy);
        }

        public List<Enemy> GetEnemies()
        {
            return arrayEnemies;
        }

        public void AddPuzzlePiece(fFloorMechanic newfFloorMechanic)
        {
            arrayPuzzlePieces.Add(newfFloorMechanic);
        }

        public List<fFloorMechanic> GetPuzzlePieces()
        {
            return arrayPuzzlePieces;
        }
    }
}
