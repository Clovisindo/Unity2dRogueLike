using Assets.Scripts.Entities.Enemies;
using Assets.Scripts.EnumTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SerializableVector = Assets.Utilities.SerializableStructs.SerializableVector3;


namespace Assets.Scripts.LevelDesign
{
    public class LevelParametersManager: MonoBehaviour
    {
        public static LevelParametersManager instance = null;

        private FactoryConfigLevelParameters factoryConfig;
        List<DesignLevelParameters> loadFileLevelDesign;
        private CountRoomsParam countRoomParam;

        int countEasyEnm = 0;
        int countMedEnm = 0;
        int countHardEnm = 0;

        float dificultModifier = 0.0f;

        private string CONFIG_FOLDER ;
        private static readonly string LDPRESETS_CONFIGFILE = "LevelDesignPresets";
        private static readonly string SPAWNPOS_CONFIGFILE = "EntitySpawnPresets";
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
            countRoomParam = new CountRoomsParam();
            //testSaveJson();
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
                SetDificultyModifier(currentLevelDesign);
                
                switch (currentTypeRoom)
                {
                    case EnumTypes.EnumTypeRoom.none:
                        break;
                    case EnumTypes.EnumTypeRoom.main:
                        SetRoomDesign(currentLevelDesign);
                        break;
                    case EnumTypes.EnumTypeRoom.secundary:
                        SetRoomDesign(currentLevelDesign);
                        break;
                    case EnumTypes.EnumTypeRoom.secret:
                        SetRoomDesign(currentLevelDesign);
                        break;
                    default:
                        break;
                }
                RoomsLevelDesign.Add(currentLevelDesign);
            }
            return RoomsLevelDesign;
        }

        private void SetDificultyModifier(DesignLevelParameters roomDesign)
        {
            //sumar a los contadores cuantas habitaciones de dificultad / tipo / tag se han generado
            if (roomDesign == null)
            {
                roomDesign = new DesignLevelParameters();
                roomDesign.dificultyRoom = EnumDificultyRoom.easy;
                roomDesign.typeRoom = EnumTypeRoom.none;
            }

            countRoomParam.SetDificultByCount(roomDesign.dificultyRoom);
            countRoomParam.SetTypeRoomByCount(roomDesign.typeRoom);
            countRoomParam.SetTagRoomByCount(roomDesign.tagRoom);

            dificultModifier = countRoomParam.SetModifierDificulty();
        }

      

        private DesignLevelParameters GetRoomLevelDesign(EnumTypes.EnumTypeRoom typeRoom)
        {
            switch (typeRoom)
            {
                case EnumTypes.EnumTypeRoom.none:
                    return null;
                case EnumTypes.EnumTypeRoom.main:
                    return GetRoomLDByParameters(typeRoom);
                case EnumTypes.EnumTypeRoom.secundary:
                    return GetRoomLDByParameters(typeRoom);
                case EnumTypes.EnumTypeRoom.secret:
                    return GetRoomLDByParameters(typeRoom);
            }
            throw new Exception("Tipo habitacion no soportado");
        }

        private DesignLevelParameters GetRoomLDByParameters(EnumTypes.EnumTypeRoom typeRoom)
        {
            List<DesignLevelParameters> designElements = new List<DesignLevelParameters>();

            var typeDificultyRoom = SetDificulty();
            var typeTagRoom = SetTagRoomByCount(typeRoom, typeDificultyRoom);
            //countEasyEnm++;

            designElements = GetFileLevelDesignByParameters(typeRoom, typeDificultyRoom, typeTagRoom);

            return new DesignLevelParameters(designElements.ElementAt(UnityEngine.Random.Range(0, designElements.Count)));
        }

        private EnumTypes.EnumDificultyRoom SetDificulty()
        {
            float random = countRoomParam.CalculateDificultPlusModifier(dificultModifier);

            return EnumTypes.TypeDificultyRoom.types.Where( d => d.TypeRange.min <= random  
            && d.TypeRange.max >= random).First().EnumDificultyRoom;
        }

        private EnumTypes.EnumTagRoom SetTagRoomByCount(EnumTypes.EnumTypeRoom typeRoom, EnumTypes.EnumDificultyRoom dificultyRoom)
        {
            var listDLParam = DesignLevelParameters.GetDLParamsByFilter(loadFileLevelDesign,typeRoom, dificultyRoom);

            if (listDLParam.Count != 0)
            {
                var nextTagRoom = countRoomParam.CheckIfNextTagRoomByCount();
                if (nextTagRoom != null)
                {
                    return nextTagRoom.Value;
                }
                int random = EnumTypes.TypeTagRoom.GetTagRoomByRandomRange(listDLParam);
                //ToDo: a medida que se repitan habitaciones, añadir modificador al random para que escale mas la dificultad
                return EnumTypes.TypeTagRoom.types.Where(d => d.TypeRange.min <= random && d.TypeRange.max >= random).First().EnumTagRoom;
            }
            else
            {
                LogErrorParams(typeRoom,dificultyRoom);
                return EnumTagRoom.unknown;
            }
        }

        private void LogErrorParams(EnumTypeRoom typeRoom, EnumDificultyRoom dificultyRoom)
        {
            Debug.Log("No hay parametros para el set de habitacion: " + typeRoom.ToString() + " y " + dificultyRoom.ToString());
        }

        private List<DesignLevelParameters> GetFileLevelDesignByParameters(EnumTypes.EnumTypeRoom typeRoom, EnumTypes.EnumDificultyRoom dificultyRoom,
            EnumTypes.EnumTagRoom tagRoom)
        {
            return loadFileLevelDesign.Where(l => l.typeRoom == typeRoom
            && l.dificultyRoom == dificultyRoom && l.tagRoom == tagRoom).ToList();
        }

        private void SetRoomDesign(DesignLevelParameters DLParameters)
        {
            if (DLParameters.enemiesJson != null)
            {
                foreach (var enemyJson in DLParameters.enemiesJson.Select((value, i) => new { i, value }))
                {
                    var enemy = factoryConfig.GetEnemyPrefabByName(enemyJson.value);
                    DLParameters.AddEnemy(enemy);
                }
            }
            if (DLParameters.puzzlesJson != null)
            {
                foreach (var puzzleJson in DLParameters.puzzlesJson.Select((value, i) => new { i, value }))
                {
                    var piece = factoryConfig.GetfPiecePrefabByName(puzzleJson.value);
                    DLParameters.AddPuzzlePiece(piece);
                }
            }
            
        }

        private void testSaveJson()
        {
            DesignLevelParameters t2 = new DesignLevelParameters();
            t2.typeRoom = EnumTypes.EnumTypeRoom.main;
            t2.dificultyRoom = EnumTypes.EnumDificultyRoom.easy;
            t2.tagRoom = EnumTypes.EnumTagRoom.enemies;
            t2.enemiesJson = new string[] { "eGoblin", "eMimic" };
            t2.enemiesJson = new string[] { "blueButton", "potion" };
            t2.arrayEnemiesPos = new List<SerializableVector> { new SerializableVector(1, 2, 3), new SerializableVector(3, 4, 5) };
            t2.arrayPuzzlePiecesPos = new List<SerializableVector> { new SerializableVector(1, 2, 3), new SerializableVector(5, 6, 7) };

            DesignLevelParameters[] t3 = new DesignLevelParameters[] { t2, t2 };

            string json1 = JsonConvert.SerializeObject(t3);
            File.WriteAllText(CONFIG_FOLDER + "save1_" + "." + SAVE_EXTENSION, json1);

            //EntitySpawnPos t2 = new EntitySpawnPos();
            //t2.tagRoom = EnumTagRoom.potionsTreasure;
            //t2.arrayPositionsEnemies = new SerializableVector[] {new SerializableVector(1,2,3), new SerializableVector(3, 4, 5) };
            //t2.arrayPositionsPieces = new SerializableVector[] { new SerializableVector(1, 2, 3), new SerializableVector(5, 6, 7) };
            //t2.countEnemies = 2;
            //t2.countPieces = 2;

            //EntitySpawnPos[] t3 = new EntitySpawnPos[] { t2, t2 };

            //string json1 = JsonConvert.SerializeObject(t3,Formatting.Indented, new JsonSerializerSettings
            //{
            //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});
            //File.WriteAllText(CONFIG_FOLDER + "save1_" + "." + SAVE_EXTENSION, json1);
        }
    }
}
