using System.Collections.Generic;

namespace Assets.Scripts.EnumTypes
{
    [System.Serializable]
    public enum EnumDificultyRoom
    {
        easy,
        medium,
        hard
    }


    public class TypeDificultyRoom
    {
        private EnumDificultyRoom enumDificultyRoom;
        private EnumTypeRange typeRange;

        public static readonly List<TypeDificultyRoom> types = new List<TypeDificultyRoom>()
        {
            new TypeDificultyRoom(EnumDificultyRoom.easy, 0, 50),
            new TypeDificultyRoom(EnumDificultyRoom.medium, 51, 80),
            new TypeDificultyRoom(EnumDificultyRoom.hard, 81, 100)
        };

        public EnumDificultyRoom EnumDificultyRoom { get => enumDificultyRoom;}
        public EnumTypeRange TypeRange { get => typeRange;}

        TypeDificultyRoom(EnumDificultyRoom enumDificultyRoom, int minProbRange, int maxProbRange)
        {
            this.enumDificultyRoom = enumDificultyRoom;
            this.typeRange = new EnumTypeRange(minProbRange, maxProbRange);
        }


        //public TypeDificultyRoom GetParamDificulty(EnumDificultyRoom dificultyRoom)
        //{
        //    switch (dificultyRoom)
        //    {
        //        case EnumDificultyRoom.easy:
        //            return new TypeDificultyRoom(EnumDificultyRoom.easy, 0, 50);
        //        case EnumDificultyRoom.medium:
        //            return new TypeDificultyRoom(EnumDificultyRoom.medium, 51, 80);
        //        case EnumDificultyRoom.hard:
        //            return new TypeDificultyRoom(EnumDificultyRoom.hard, 91, 100);
        //    }
        //    return null;
        //}
    }

}
