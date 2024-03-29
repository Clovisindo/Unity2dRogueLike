﻿using Assets.Scripts.Entities.Enemies;
using Assets.Scripts.LevelDesign;
using UnityEngine;
using EnumScene = LoaderSceneScript.Scene;

public class GameManager : MonoBehaviour
{
    //public float levelStarDelay = 2f;
    //public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public LevelGeneration levelGenerationScript;
    public EventRoomController eventRoomController;
    public LevelParametersManager levelParametersManager;
    public Player player;
    public Enemy enemy;
    private BoardRoom[] boardRooms;

    private int level = 1;
    private bool doingSetup;

    public GameObject gameCamera;

    //instantiate prefabs
    public GameObject ini_Player;
    public GameObject ini_Enemy;

    public BoardRoom currentRoom;

    int layer_mask_wall;

    public BoardRoom[] BoardRooms { get => boardRooms; set => boardRooms = value; }



    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 30;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        boardScript = GetComponent<BoardManager>();
        levelGenerationScript = GetComponent<LevelGeneration>();
        eventRoomController = GetComponent<EventRoomController>();
        levelParametersManager = GetComponent<LevelParametersManager>();
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        layer_mask_wall = LayerMask.GetMask("ColliderRoomDetector");

        player = Instantiate(player, ini_Player.transform.position, Quaternion.identity);
        enemy = Instantiate(enemy, ini_Enemy.transform.position, Quaternion.identity);
        InitGame();
    }

    public void AddRooms(BoardRoom[] arrayRooms)
    {
        BoardRooms = arrayRooms;
    }

    public bool CheckLastEnemyRoom()
    {
        if (currentRoom.enemiesRoom.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void InitGame()
    {
        doingSetup = true;
    }

        // Update is called once per frame
        void Update()
    {
    }
    /// <summary>
    /// Tras contactar con un evento de cambio de habitacion, se le indica la direccion de la puerta para mover la camara
    /// </summary>
    public void ChangeLevel(LevelGeneration.doorDirection currentDirection)
    {
        currentRoom.PauseRoom();
        gameCamera.transform.position = ChangeCameraByDoorDirection(currentDirection);

        currentRoom.DisableColliderRoom();
        currentRoom.DisableChangeEventColliderEntranceRoom();
        currentRoom.DisableChangeEventColliderExitRoom();

        UpdateCurrentRoom(GameManager.instance.player, currentDirection);


        if (currentRoom.RoomComplete)
        {
            //ToDo:enable colliders todas las NO secret rooms
            currentRoom.EnableChangeEventColliderEntranceRoom();
            currentRoom.EnableChangeEventColliderExitRoom();
        }
        GameManager.instance.player.UpdatePositionlevel(currentRoom.GetRespawnPositionPlayer( GetReversalDoorDirection(currentDirection)));//camino en direcion original
        //arrancamos los enemigos en esta habitacion
        currentRoom.ReStartRoom();
    }
    /// <summary>
    /// Cambia al jugador de habitacion
    /// </summary>
    /// <param name="doorActivated">puerta activada</param>
    public void MovePlayerToRoom(GameObject doorActivated)
    {
        GameManager.instance.ChangeLevel(currentRoom.GetDirectionByDoor(doorActivated));
    }

    public void takeDamage( string colliderTag, Enemy enemy, int weaponDamage, float weaponKBDistance, float weaponKBSpeed)
    {
        if (enemy.tag == colliderTag && (!enemy.checkIsInmune()))
        {
            enemy.TakeDamage(weaponDamage, weaponKBDistance,weaponKBSpeed);
        }
    }

    public void SetCurrentRoomBoard( BoardRoom board)
    {
        currentRoom = board;
    }

    public LevelGeneration.doorDirection GetReversalDoorDirection(LevelGeneration.doorDirection doorDirection)
    {
        switch (doorDirection)
        {
            case LevelGeneration.doorDirection.down:
                doorDirection = LevelGeneration.doorDirection.up;
                break;
            case LevelGeneration.doorDirection.up:
                doorDirection = LevelGeneration.doorDirection.down;
                break;
            case LevelGeneration.doorDirection.left:
                doorDirection = LevelGeneration.doorDirection.right;
                break;
            case LevelGeneration.doorDirection.right:
                doorDirection = LevelGeneration.doorDirection.left;
                break;
        }
        return doorDirection;
    }

    /// <summary>
    /// Al descubrir una puerta secreta, se busca todas las puertas secretas adyacentes y se abren a la vez
    /// </summary>
    /// <param name="secretDoorobj"></param>
    /// <param name="doorCollider"></param>
    public void OpenSecretDoor(FRoomDoor secretDoorobj, Collider2D doorCollider)
    {
        //abrir la puerta con la colision y su adyacente
        var currentDirection = GameManager.instance.currentRoom.GetDirectionByDoor(doorCollider.transform.parent.gameObject);
        var currentRoomDoors = currentRoom.GetDoorsByDirection(currentDirection);
        foreach (var currentRoomDoor in currentRoomDoors)
        {
            currentRoom.OpenSecretDoor(currentRoomDoor.GetComponent<FRoomDoor>());
        }
        //Se desactiva el collider de la habitacion actual para localizar la contigua
        GameManager.instance.currentRoom.DisableColliderRoom();
        var adjRoom = GameManager.instance.GetAdjacentRoom(secretDoorobj.transform, currentDirection);
        GameManager.instance.currentRoom.EnableColliderRoom();

        Debug.Log("Se ha abierto una puerta secreta.");
    }
    /// <summary>
    /// devolvemos la habicacion contigua para trabajar con sus parametros por adelantado
    /// </summary>
    /// <param name="currentDoor"></param>
    /// <param name="currentDoorDirection"></param>
    public BoardRoom GetAdjacentRoom( Transform currentDoor, LevelGeneration.doorDirection currentDoorDirection)
    {
        Vector2 rayCastDirection = Vector2.zero;
        switch (currentDoorDirection)
        {
            case LevelGeneration.doorDirection.down:
                rayCastDirection = Vector2.down;
                break;
            case LevelGeneration.doorDirection.up:
                rayCastDirection = Vector2.up;
                break;
            case LevelGeneration.doorDirection.left:
                rayCastDirection = Vector2.left;
                break;
            case LevelGeneration.doorDirection.right:
                rayCastDirection = Vector2.right;
                break;
        }
        RaycastHit2D hit = Physics2D.Raycast(currentDoor.position, rayCastDirection, 4f, layer_mask_wall);
        if (hit.collider.tag == "RoomCollider")
        {

            //1º devolver que habitacion es
            GameObject roomCollider = hit.collider.gameObject;
            return roomCollider.transform.parent.gameObject.GetComponent<BoardRoom>();
        }
            return null;
    }


    private void UpdateCurrentRoom(Player _player, LevelGeneration.doorDirection _doorDirection)
    {
        Vector2 rayCastDirection = Vector2.zero;
        switch (_doorDirection)
        {
            case LevelGeneration.doorDirection.down:
                rayCastDirection = Vector2.down;
                break;
            case LevelGeneration.doorDirection.up:
                rayCastDirection = Vector2.up;
                break;
            case LevelGeneration.doorDirection.left:
                rayCastDirection = Vector2.left;
                break;
            case LevelGeneration.doorDirection.right:
                rayCastDirection = Vector2.right;
                break;

        }
        RaycastHit2D hit = Physics2D.Raycast(_player.transform.position, rayCastDirection, 4f, layer_mask_wall);
        if (hit.collider.tag == "RoomCollider")
        {
            //Debug.DrawLine(transform.position, hit.collider.transform.position, Color.green, Time.deltaTime, true);
            //1º devolver que habitacion es
            GameObject roomCollider = hit.collider.gameObject;
            //2º update setCurrentBoard
            currentRoom.EnableColliderRoom();
            currentRoom = roomCollider.transform.parent.gameObject.GetComponent<BoardRoom>();
        }
    }
    private Vector3 ChangeCameraByDoorDirection ( LevelGeneration.doorDirection _doorDirection)
    {
        Vector3 newPositionCamera = new Vector3();
        switch (_doorDirection)
        {
            case LevelGeneration.doorDirection.down :
                newPositionCamera = new Vector3(gameCamera.transform.position.x, gameCamera.transform.position.y - levelGenerationScript.moveAmountY, gameCamera.transform.position.z);
                break;
            case LevelGeneration.doorDirection.up:
                newPositionCamera = new Vector3(gameCamera.transform.position.x, gameCamera.transform.position.y + levelGenerationScript.moveAmountY, gameCamera.transform.position.z);
                break;
            case LevelGeneration.doorDirection.left:
                newPositionCamera = new Vector3(gameCamera.transform.position.x - levelGenerationScript.moveAmountX, gameCamera.transform.position.y, gameCamera.transform.position.z);
                break;
            case LevelGeneration.doorDirection.right:
                newPositionCamera = new Vector3(gameCamera.transform.position.x + levelGenerationScript.moveAmountX, gameCamera.transform.position.y, gameCamera.transform.position.z);
                break;
        }

        return newPositionCamera;
    }

    public void DefeatEndGame()
    {
        LoadGameOverScene();
    }
    public void VictoryEndGame()
    {
            LoadVictoryScene();
    }
    private void LoadReMenuScene()
    {
        LoaderSceneScript.LoadScene(EnumScene.MainMenuScene);
    }
    private void LoadVictoryScene()
    {
        LoaderSceneScript.LoadScene(EnumScene.VictoryMenuScene);
    }

    private void LoadGameOverScene()
    {
        LoaderSceneScript.LoadScene(EnumScene.GameOverMenuScene);
    }
}
