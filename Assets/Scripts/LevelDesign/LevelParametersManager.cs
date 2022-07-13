using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace Assets.Scripts.LevelDesign
{
    public class LevelParametersManager: MonoBehaviour
    {
        public static LevelParametersManager instance = null;

        private FactoryConfigLevelParameters factoryConfig;
        List<DesignLevelParameters> loadFileLevelDesign;

        int countEasyEnm = 0;
        int countMedEnm = 0;
        int countHardEnm = 0;

        private string CONFIG_FOLDER ;
        private static readonly string LDPRESETS_CONFIGFILE = "LevelDesignPresets";
        private const string SAVE_EXTENSION = ".json";

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
            CONFIG_FOLDER = Application.dataPath + "/Levels/";
        }

        private void LoadFileLevelParameters()
        {
            loadFileLevelDesign = LoadLDPresetsConfigFile();
        }

        private List<DesignLevelParameters> LoadLDPresetsConfigFile()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(CONFIG_FOLDER);
            FileInfo configFile = directoryInfo.GetFiles(LDPRESETS_CONFIGFILE + SAVE_EXTENSION).First();

            string presetsJson = File.ReadAllText(configFile.FullName);
            return JsonConvert.DeserializeObject<List<DesignLevelParameters>>(presetsJson);
        }

        public List<DesignLevelParameters> SetDesignParametersRooms(BoardRoom[] rooms)
        {
            List<DesignLevelParameters> RoomsLevelDesign = new List<DesignLevelParameters>();
            DesignLevelParameters currentLevelDesign;
            LoadFileLevelParameters();

            for (int i = 0; i < rooms.Length; i++)
            {
                var currentTypeRoom = rooms[i].RoomParameters.TypeRoom;
                currentLevelDesign = GetRoomLevelDesign(currentTypeRoom);
                
                switch (currentTypeRoom)
                {
                    case EnumTypes.EnumTypeRoom.none:
                        break;
                    case EnumTypes.EnumTypeRoom.main:
                        SetCombatRoom(currentLevelDesign);
                        break;
                    case EnumTypes.EnumTypeRoom.secundary:
                        SetPuzzleRoom(currentLevelDesign);
                        break;
                    case EnumTypes.EnumTypeRoom.secret:
                        SetPuzzleRoom(currentLevelDesign);//ToDo
                        break;
                    default:
                        break;
                }
                RoomsLevelDesign.Add(currentLevelDesign);
            }
            return RoomsLevelDesign;
        }

        private DesignLevelParameters GetRoomLevelDesign(EnumTypes.EnumTypeRoom typeRoom)
        {
            switch (typeRoom)
            {
                case EnumTypes.EnumTypeRoom.main:
                    return GetRoomLDCombat();
                case EnumTypes.EnumTypeRoom.secundary:
                    return GetRoomLDPuzzle();
                case EnumTypes.EnumTypeRoom.secret:
                    return GetRoomLDPuzzle();//ToDo: especial
            }
            throw new Exception("Tipo habitacion no soportado");
        }

        private DesignLevelParameters GetRoomLDCombat()
        {
            List<DesignLevelParameters> designElements = new List<DesignLevelParameters>();

            designElements = SetFileLevelDesignByDificulty();
            countEasyEnm++;
            return new DesignLevelParameters(designElements.ElementAt(UnityEngine.Random.Range(0, designElements.Count)));
        }

        private List<DesignLevelParameters> SetFileLevelDesignByDificulty()
        {
            List<DesignLevelParameters> designElements;
            switch (countEasyEnm)
            {
                case 0:
                case 1:
                    designElements = GetFileLevelDesignByDificulty(EnumTypes.EnumDificultyRoom.easy);
                    break;
                case 2:
                case 3:
                    designElements = GetFileLevelDesignByDificulty(EnumTypes.EnumDificultyRoom.medium);
                    break;
                default:
                    designElements = GetFileLevelDesignByDificulty(EnumTypes.EnumDificultyRoom.hard);
                    break;
            }
            return designElements;
        }

        private List<DesignLevelParameters> GetFileLevelDesignByDificulty(EnumTypes.EnumDificultyRoom dificultyRoom)
        {
            return loadFileLevelDesign.Where(l => l.typeRoom == EnumTypes.EnumTypeRoom.main
            && l.dificultyRoom == dificultyRoom).ToList();
        }

        private DesignLevelParameters GetRoomLDPuzzle()
        {
            return new DesignLevelParameters(loadFileLevelDesign.Where(l => l.typeRoom == EnumTypes.EnumTypeRoom.secundary).First());//ToDo: reglas
        }

        private void SetCombatRoom(DesignLevelParameters DLParameters)
        {
            foreach (var enemyJson in DLParameters.enemiesJson)
            {
                DLParameters.AddEnemy(factoryConfig.GetEnemyPrefabByName(enemyJson));
            }
        }

        private void SetPuzzleRoom(DesignLevelParameters DLParameters)
        {
            foreach (var puzzleJson in DLParameters.puzzlesJson)
            {
                DLParameters.AddPuzzlePiece(factoryConfig.GetfPiecePrefabByName(puzzleJson));
            }
        }

        private void testSaveJson()
        {
            DesignLevelParameters t2 = new DesignLevelParameters();
            t2.typeRoom = EnumTypes.EnumTypeRoom.main;
            t2.dificultyRoom = EnumTypes.EnumDificultyRoom.easy;
            t2.tagRoom = EnumTypes.EnumTagRoom.tipoA;
            t2.enemiesJson = new string[] { "eGoblin", "eMimic" };
            t2.enemiesJson = new string[] { "blueButton", "potion" };

            DesignLevelParameters[] t3 = new DesignLevelParameters[] { t2, t2 };

            string json1 = JsonConvert.SerializeObject(t3);
            File.WriteAllText(CONFIG_FOLDER + "save1_" + "." + SAVE_EXTENSION, json1);
        }
    }
}
