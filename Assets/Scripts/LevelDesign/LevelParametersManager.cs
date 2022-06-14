using Assets.Scripts.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace Assets.Scripts.LevelDesign
{
    public class LevelParametersManager: MonoBehaviour
    {
        public static LevelParametersManager instance = null;
        int level = 1;


        private Dictionary<string, Type> enemiesByName;
        private FactoryConfigLevelParameters factoryConfig;


        public LevelParametersManager()
        {
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Destroy(gameObject);
            }

            factoryConfig = this.GetComponent<FactoryConfigLevelParameters>();
        }

        private string[] LoadFileLevelParameters()
        {
            //cargar la lista de parametros del fichero
            return new string[] { "eGoblin", "eMimic" };

        }

        public List<DesignLevelParameters> SetDesignParametersRooms(BoardRoom[] rooms)
        {
            List<DesignLevelParameters> currentLevelDesign = new List<DesignLevelParameters>();

            for (int i = 0; i < rooms.Length; i++)
            {
                var fileLevelParameters = LoadFileLevelParameters();
                
                var currentTypeRoom = rooms[i].RoomParameters.TypeRoom;
                switch (currentTypeRoom)
                {
                    case EnumTypes.EnumTypeRoom.none:
                        break;
                    case EnumTypes.EnumTypeRoom.Main:
                        currentLevelDesign.Add(SetCombatRoom(fileLevelParameters));
                        break;
                    case EnumTypes.EnumTypeRoom.Secundary:
                        break;
                    case EnumTypes.EnumTypeRoom.Secret:
                        break;
                    default:
                        break;
                }
            }
            return currentLevelDesign;
        }

        private DesignLevelParameters SetCombatRoom(string[] fileLevelParameters)
        {
            DesignLevelParameters DLParameters = new DesignLevelParameters();

            foreach (var fileLevelParameter in fileLevelParameters)
            {
                DLParameters.AddEnemy(factoryConfig.GetEnemyPrefabByName(fileLevelParameter));
            }

            return DLParameters;
        }
    }
}
