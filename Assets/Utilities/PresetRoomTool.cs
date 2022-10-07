using Assets.Scripts.Entities.Enemies;
using Assets.Scripts.LevelDesign;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Assets.Utilities.SerializableStructs;
using enumTypes = Assets.Scripts.EnumTypes;

public class PresetRoomTool : MonoBehaviour
{
    [SerializeField] enumTypes.EnumTypeRoom typeRoom;
    [SerializeField] enumTypes.EnumDificultyRoom dificultyRoom;
    [SerializeField] enumTypes.EnumTagRoom tagRoom;
    
    [SerializeField] List<Enemy> enemies;
    [SerializeField] List<fFloorMechanic> pieces;
    
    [SerializeField] List<Transform> enemiesPosition;
    [SerializeField] List<Transform> piecePosition;

    private string[] enemiesJson;
    private string[] puzzlesJson;

    private List<SerializableVector3> JSONEnemiesPos;
    private List<SerializableVector3> JSONPuzzlePiecesPos;

    private string CONFIG_FOLDER;
    private const string SAVE_EXTENSION = ".json";


    // Start is called before the first frame update
    void Awake()
    {
        LoadEditorData();
        GenerateJsonByRoom();
    }

    private void LoadEditorData()
    {
        CONFIG_FOLDER = Application.dataPath + "/Levels/";

        if (enemies.Count > 0)
        {
            enemiesJson = ConvertEnemyListToName(enemies);
        }
        if (pieces.Count > 0)
        {
            puzzlesJson = ConvertPieceListToName(pieces);
        }
        if (enemiesPosition.Count > 0)
        {
            JSONEnemiesPos = ConvertVector3ToSerialize(enemiesPosition);
        }
        if (piecePosition.Count > 0)
        {
            JSONPuzzlePiecesPos = ConvertVector3ToSerialize(piecePosition);
        }
    }

    private void GenerateJsonByRoom()
    {
        DesignLevelParameters roomParams = FormatToJson();
        GenerateJSON(roomParams);
    }

    private void GenerateJSON(DesignLevelParameters roomParams)
    {
        string json1 = JsonConvert.SerializeObject(roomParams,Formatting.Indented);
        File.WriteAllText(CONFIG_FOLDER + "room1_" + "." + SAVE_EXTENSION, json1);
    }

    private DesignLevelParameters FormatToJson()
    {
        DesignLevelParameters newParams = new DesignLevelParameters();
        newParams.typeRoom = typeRoom;
        newParams.dificultyRoom = dificultyRoom;
        newParams.tagRoom = tagRoom;
        newParams.enemiesJson = enemiesJson;
        newParams.puzzlesJson = puzzlesJson;
        newParams.arrayEnemiesPos = JSONEnemiesPos;
        newParams.arrayPuzzlePiecesPos = JSONPuzzlePiecesPos;
    
        return newParams;
    }

    private string[] ConvertEnemyListToName(List<Enemy> enemyList)
    {
        List<string> listNames = new List<string>();
        foreach (var enemy in enemyList)
        {
            listNames.Add(enemy.name);
        }
        return listNames.ToArray();
    }

    private string[] ConvertPieceListToName(List<fFloorMechanic> pieceList)
    {
        List<string> listNames = new List<string>();
        foreach (var piece in pieceList)
        {
            listNames.Add(piece.fName);
        }
        return listNames.ToArray();
    }

    private List<SerializableVector3> ConvertVector3ToSerialize(List<Transform> entitiesPosition)
    {
        List<SerializableVector3> sVectors = new List<SerializableVector3>();
        foreach (var entityPos in entitiesPosition)
        {
            sVectors.Add((SerializableVector3)entityPos.position);
        }
        return sVectors;
    }
}
