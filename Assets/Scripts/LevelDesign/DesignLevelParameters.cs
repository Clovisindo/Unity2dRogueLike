using Assets.Scripts.Entities.Enemies;
using Assets.Scripts.EnumTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Utilities.SerializableStructs;

namespace Assets.Scripts.LevelDesign
{
    [System.Serializable]
    public class DesignLevelParameters
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumTypeRoom typeRoom;
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumDificultyRoom dificultyRoom;
        [JsonConverter(typeof(StringEnumConverter))]
        public EnumTagRoom tagRoom;

        public string[] enemiesJson;
        public string[] puzzlesJson;

        [JsonIgnore]
        private List<Enemy> arrayEnemies;
        public List<SerializableVector3> arrayEnemiesPos;
        [JsonIgnore]
        private List<fFloorMechanic> arrayPuzzlePieces;
        public List<SerializableVector3> arrayPuzzlePiecesPos;

        public DesignLevelParameters()
        {
            arrayEnemies = new List<Enemy>();
            arrayPuzzlePieces = new List<fFloorMechanic>();
            arrayEnemiesPos = new List<SerializableVector3>();
            arrayPuzzlePiecesPos = new List<SerializableVector3>();
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
            arrayEnemiesPos = newDesignLD.arrayEnemiesPos;
            arrayPuzzlePiecesPos = newDesignLD.arrayPuzzlePiecesPos;
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

        public SerializableVector3 GetPositionEnemyByIndex(int index)
        {
            return arrayEnemiesPos[index];
        }

        public SerializableVector3 GetPositionPieceByIndex(int index)
        {
            return arrayPuzzlePiecesPos[index];
        }

        public List<Transform> GetTransformEnemyPositions()
        {
            List<Transform> list = new List<Transform>();
            foreach (var enemyPosSV3 in arrayEnemiesPos)
            {
                GameObject t1 = new GameObject() ;
                t1.transform.position = (Vector3)enemyPosSV3;
                list.Add(t1.transform);
            }
            return list;
        }

        public List<Transform> GetTransformPiecePositions()
        {
            List<Transform> list = new List<Transform>();
            foreach (var piecePosSV3 in arrayPuzzlePiecesPos)
            {
                GameObject t1 = new GameObject();
                t1.transform.position = (Vector3)piecePosSV3;
                list.Add(t1.transform);
            }
            return list;
        }
    }
}
