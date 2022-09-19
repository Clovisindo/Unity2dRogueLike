using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.LevelDesign;
using Utilities = Assets.Utilities.Utilities;

public class EventRoomController : MonoBehaviour
{
    public static EventRoomController instance = null;
    [SerializeField] private GameObject[] InitPositionsEnemy;
    private List<Vector3> currentInitPositionsEnemy;
    [SerializeField] private GameObject[] InitPositionsPuzzle;
    private List<Vector3> currentInitPositionsPuzzle;

    public BoardRoom currentRoom;

    private List<DesignLevelParameters> listLevelParameters;

    public Dictionary<string, int> TypeQtyEnemies = new Dictionary<string, int>();


    // Start is called before the first frame update
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
    }

    public void InitRoomsDungeonLevel(BoardRoom[] listRooms)
    {
        listLevelParameters = GameManager.instance.levelParametersManager.SetDesignParametersRooms(listRooms);
        foreach (var room in listRooms.Select((value, i) => (value, i)))
        {
            InitRoom(room);
            room.value.PauseRoom();
        }
        // asignar la primera habitacion como inicial
        currentRoom = listRooms[0];
        currentRoom.ReStartRoom();
    }

    /// <summary>
    /// Se inicializa una habitacion
    /// </summary>
    private void InitRoom((BoardRoom value, int i) currentRoom)
    {
        switch (currentRoom.value.RoomParameters.TypeRoom)
        {
            case Assets.Scripts.EnumTypes.EnumTypeRoom.none:
                SetEmptyRoom(currentRoom);
                break;
            case Assets.Scripts.EnumTypes.EnumTypeRoom.main:
                SpawnEntitiesRoom(currentRoom);
                break;
            case Assets.Scripts.EnumTypes.EnumTypeRoom.secundary:
                SpawnEntitiesRoom(currentRoom);
                break;
            case Assets.Scripts.EnumTypes.EnumTypeRoom.secret:
                SpawnEntitiesRoom(currentRoom);
                break;
        }
    }

    private void SetEmptyRoom((BoardRoom value, int i) currentRoom)
    {
        Debug.Log(" habitacion vacia en " + currentRoom.value.name);
    }


    private void SpawnEntitiesRoom((BoardRoom value, int i) currentRoom)
    {
        int countEnemies = listLevelParameters[currentRoom.i].GetEnemies().Count;
        int countPieces = listLevelParameters[currentRoom.i].GetPuzzlePieces().Count;
        
        if (countEnemies > 0)
        {
            currentInitPositionsEnemy = listLevelParameters[currentRoom.i].GetTransformEnemyPositions();
            currentRoom.value.InvokeEnemies(currentInitPositionsEnemy, listLevelParameters[currentRoom.i].GetEnemies());
            currentInitPositionsEnemy.Clear();
        }
        if (countPieces > 0)
        {
            currentInitPositionsPuzzle = listLevelParameters[currentRoom.i].GetTransformPiecePositions();
            currentRoom.value.InvokeFPieces(currentInitPositionsPuzzle, listLevelParameters[currentRoom.i].GetPuzzlePieces());
            currentInitPositionsPuzzle.Clear();
        }
        LogCurrentRoom(listLevelParameters[currentRoom.i], currentRoom.value);
    }

    public void LogCurrentRoom(DesignLevelParameters roomParameters, BoardRoom currentRoom)
    {
        Debug.Log(" Habitación " + currentRoom.name + " generada, de tipo : " + roomParameters.typeRoom +
            " dificultad : " + roomParameters.dificultyRoom +
            " y clase : " + roomParameters.tagRoom +
            ".");
        if (roomParameters.enemiesJson != null)
        {
            string logEnemies = "Enemigos invocados : ";
            foreach (var roomEnemy in roomParameters.enemiesJson)
            {
                logEnemies += roomEnemy + ",";
            }
            Debug.Log(logEnemies + ".");
        }

        if (roomParameters.puzzlesJson != null)
        {
            string logPieces = "Piezas invocadas : ";
            foreach (var roomPiece in roomParameters.puzzlesJson)
            {
                logPieces += roomPiece + ",";
            }
            Debug.Log(logPieces + ".");
        }

    }

    //pausar la partida
    public void PauseRoom(BoardRoom _boardRoom)
    {
        _boardRoom.PauseRoom();
    }
    //reiniciar la instancia de la habitacion
    public void ReStartRoom(BoardRoom _boardRoom)
    {
        _boardRoom.ReStartRoom();
    }

}
