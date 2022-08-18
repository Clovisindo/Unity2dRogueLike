using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.LevelDesign;
using Utilities = Assets.Utilities.Utilities;

public class EventRoomController : MonoBehaviour
{
    public static EventRoomController instance = null;
    [SerializeField] private GameObject[] InitPositionsEnemy;
    private List<Transform> currentInitPositionsEnemy;
    [SerializeField] private GameObject[] InitPositionsPuzzle;

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
        currentInitPositionsEnemy = Utilities.getAllChildsObject<Transform>(InitPositionsEnemy[0].transform);//ToDo: elegir de forma aleatoria
        //SetTypeCurrentRoom(currentRoom.value);

        switch (currentRoom.value.RoomParameters.TypeRoom)
        {
            case Assets.Scripts.EnumTypes.EnumTypeRoom.none:
                SetEmptyRoom();
                break;
            case Assets.Scripts.EnumTypes.EnumTypeRoom.main:
                SpawnEntitiesRoom(currentRoom);
                break;
            case Assets.Scripts.EnumTypes.EnumTypeRoom.secundary:
                SpawnEntitiesRoom(currentRoom);
                break;
            case Assets.Scripts.EnumTypes.EnumTypeRoom.secret:
                SetEmptyRoom();
                break;
        }
    }

    private void SetEmptyRoom()
    {
        //throw new NotImplementedException();
    }

    private void SetSpawnPuzzle()
    {
        //throw new NotImplementedException();
    }

    private void SpawnEntitiesRoom((BoardRoom value, int i) currentRoom)
    {
        int countEnemies = listLevelParameters[currentRoom.i].GetEnemies().Count;
        int countPieces = listLevelParameters[currentRoom.i].GetPuzzlePieces().Count;
        
        if (countEnemies > 0)
        {
            currentInitPositionsEnemy.Clear();
            currentInitPositionsEnemy = listLevelParameters[currentRoom.i].GetTransformEnemyPositions();
            currentRoom.value.InvokeEnemies(currentInitPositionsEnemy, listLevelParameters[currentRoom.i].GetEnemies());
        }
        if (countPieces > 0)
        {
            currentInitPositionsEnemy.Clear();
            currentInitPositionsEnemy = listLevelParameters[currentRoom.i].GetTransformPiecePositions();
            currentRoom.value.InvokeFPieces(currentInitPositionsEnemy, listLevelParameters[currentRoom.i].GetPuzzlePieces());
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
