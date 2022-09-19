using Assets.Scripts.EnumTypes;
using UnityEngine;

namespace Assets.Scripts.LevelDesign
{
    public class CountRoomsParam
    {
        private const int dificultMinInclusive = 0;
        private const int dificultMaxInclusive = 100;
        private readonly int CAP_ENEMIES_TO_FOUNTAIN = 3;
        private bool nextIsFountainRoom = false;

        public int CountEasyRooms { get; set; }
        public int CountMediumRooms { get; set; }
        public int CountHardRooms { get; set; }

        public int CountNoneRooms { get; set; }
        public int CountMainRooms { get; set; }
        public int CountSecundaryRooms { get; set; }
        public int CountSecretRooms { get; set; }

        public int countPotionTreasureRooms { get; set; }
        public int countSwitchsRooms { get; set; }
        public int countButtonsRooms { get; set; }
        public int countFountainRooms { get; set; }
        public int countEnemiesRooms { get; set; }
        public int countTrapsRooms { get; set; }
        public int countTreasuresRooms { get; set; }
        public int countKeyRooms { get; set; }
        public int countMimicsRooms { get; set; }
        public int countUnknownRooms { get; set; }


        public CountRoomsParam()
        {
            CountEasyRooms = 0;
            CountMediumRooms = 0;
            CountHardRooms = 0;

            CountNoneRooms = 0;
            CountMainRooms = 0;
            CountSecundaryRooms = 0;
            CountSecretRooms = 0;

            countButtonsRooms = 0;
            countEnemiesRooms = 0;
            countFountainRooms = 0;
            countKeyRooms = 0;
            countMimicsRooms = 0;
            countPotionTreasureRooms = 0;
            countSwitchsRooms = 0;
            countTrapsRooms = 0;
            countTreasuresRooms = 0;
            countUnknownRooms = 0;
        }


        public void SetDificultByCount(EnumDificultyRoom dificultyRoom)
        {
            switch (dificultyRoom)
            {
                case EnumDificultyRoom.easy:
                    CountEasyRooms++;
                    break;
                case EnumDificultyRoom.medium:
                    CountMediumRooms++;
                    break;
                case EnumDificultyRoom.hard:
                    CountHardRooms++;
                    break;
                default:
                    break;
            }
        }

        public void SetTypeRoomByCount(EnumTypeRoom typeRoom)
        {
            switch (typeRoom)
            {
                case EnumTypeRoom.none:
                    CountNoneRooms++;
                    break;
                case EnumTypeRoom.main:
                    CountMainRooms++;
                    break;
                case EnumTypeRoom.secundary:
                    CountSecundaryRooms++;
                    break;
                case EnumTypeRoom.secret:
                    CountSecretRooms++;
                    break;
                default:
                    break;
            }
        }

        public void SetTagRoomByCount(EnumTagRoom tagRoom)
        {
            switch (tagRoom)
            {
                case EnumTagRoom.potionsTreasure:
                    countPotionTreasureRooms++;
                    break;
                case EnumTagRoom.switches:
                    countSwitchsRooms++;
                    break;
                case EnumTagRoom.buttons:
                    countButtonsRooms++;
                    break;
                case EnumTagRoom.fountain:
                    countFountainRooms++;
                    break;
                case EnumTagRoom.enemies:
                    countEnemiesRooms++;
                    break;
                case EnumTagRoom.traps:
                    countTrapsRooms++;
                    break;
                case EnumTagRoom.treasures:
                    countTreasuresRooms++;
                    break;
                case EnumTagRoom.key:
                    countKeyRooms++;
                    break;
                case EnumTagRoom.mimics:
                    countMimicsRooms++;
                    break;
                case EnumTagRoom.unknown:
                    countUnknownRooms++;
                    break;
                default:
                    break;
            }
        }

        internal float SetModifierDificulty()
        {
            float modifier = 0;
            if (CountEasyRooms > 0)
            {
                modifier += CountEasyRooms / 2 * 20;
            }
            if (CountMediumRooms > 0)
            {
                modifier += CountMediumRooms / 2 * 10;
            }
            if (CountHardRooms > 0)
            {
                modifier += CountHardRooms / 2 * 5;
            }
            return modifier;
        }

        internal void SetModifierTagRoom()
        {
            // cada X condicion, forzar una habitacion de tag especifica

            // aqui calculamos y marcamos unos checks
            //luego metodo externo previo a coger filtro, para forzar este tag

            //ejemplos querer hacer una habitacion de fuente cada 3 de enemigos
            if (CountEasyRooms > CAP_ENEMIES_TO_FOUNTAIN)
            {
                nextIsFountainRoom = true;
                Debug.Log("Forzamos fuente por cap enemies to fountain.");
            }
        }

        internal EnumTagRoom? CheckIfNextTagRoomByCount()
        {
            if (nextIsFountainRoom)
            {
                return EnumTagRoom.fountain;
            }
            return null;
        }

        internal float CalculateDificultPlusModifier( float dificultModifier)
        {
            float random = UnityEngine.Random.Range(dificultMinInclusive, dificultMaxInclusive) + dificultModifier;
            if (random > dificultMaxInclusive)
            {
                random = dificultMaxInclusive;
            }
            if (random < dificultMinInclusive)
            {
                random = dificultMinInclusive;
            }
            return random;
        }
    }
}
