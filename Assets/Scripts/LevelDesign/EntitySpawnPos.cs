using Assets.Scripts.EnumTypes;
using System;
using static Assets.Utilities.SerializableStructs;

namespace Assets.Scripts.LevelDesign
{
    public class EntitySpawnPos
    {
        public EnumTagRoom tagRoom;
        public SerializableVector3[] arrayPositionsEnemies;
        public SerializableVector3[] arrayPositionsPieces;
        public int countEnemies;
        public int countPieces;

        public EntitySpawnPos()
        {
            this.tagRoom = EnumTagRoom.unknown;
            this.countEnemies = 0;
            this.countPieces = 0;
        }

        public EntitySpawnPos(EntitySpawnPos entitySpawn)
        {
            this.tagRoom = entitySpawn.tagRoom;
            this.arrayPositionsEnemies = entitySpawn.arrayPositionsEnemies ?? throw new ArgumentNullException(nameof(entitySpawn.arrayPositionsEnemies));
            this.arrayPositionsPieces = entitySpawn.arrayPositionsPieces ?? throw new ArgumentNullException(nameof(entitySpawn.arrayPositionsPieces));
            this.countEnemies = entitySpawn.countEnemies;
            this.countPieces = entitySpawn.countPieces;
        }

        //public void AddPositionsEnemies(Vector3[] _positions)
        //{
        //    arrayPositionsEnemies =  _positions;
        //}

        //public void AddPositionsPieces(Vector3[] _positions)
        //{
        //    arrayPositionsPieces = _positions;
        //}

        //public Vector3[] GetEnemies()
        //{
        //    return arrayPositionsEnemies;
        //}

        //public Vector3[] GetPieces()
        //{
        //    return arrayPositionsPieces;
        //}
    }
}
