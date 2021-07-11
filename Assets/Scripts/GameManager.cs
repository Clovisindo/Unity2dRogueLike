using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public float levelStarDelay = 2f;
    //public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public LevelGeneration levelGenerationScript;
    public EventRoomController eventRoomController;
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
    private LevelGeneration.doorDirection tempEntrance;
    private LevelGeneration.doorDirection tempExit;

    public BoardRoom[] BoardRooms { get => boardRooms; set => boardRooms = value; }



    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        levelGenerationScript = GetComponent<LevelGeneration>();
        eventRoomController = GetComponent<EventRoomController>();
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        layer_mask_wall = LayerMask.GetMask("ColliderRoomDetector");

        player = Instantiate(player, ini_Player.transform.position, Quaternion.identity);
        enemy = Instantiate(enemy, ini_Enemy.transform.position, Quaternion.identity);
        InitGame();
    }

    /// <summary>
    /// Destruimos al enemigo muerto y comprobamos que no queden mas enemigos
    /// </summary>
    /// <param name="enemy"></param>
    //internal void DestroyEnemy(Enemy enemy)
    //{
    //    currentRoom.enemiesRoom.Remove(enemy.GetComponent<Enemy>());
    //    Destroy(enemy);
    //    if (CheckLastEnemyRoom())
    //    {
    //        currentRoom.OpenDoor();
    //    }
    //}

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

    public void ChangeLevel(bool backwards)
    {
        currentRoom.SetEntranceExitDoor(backwards);

        //pausamos los enemigos de la habitacion
        currentRoom.PauseRoom();

        //gestionar la camara en funcion de la direccion de la habitacion , entrada o salida,etc
        gameCamera.transform.position = ChangeCameraByDoorDirection(currentRoom.exitDoor);

        //desactivar el collider de la habitacion actual
        currentRoom.DisableColliderRoom();
        currentRoom.DisableChangeEventColliderEntranceRoom();
        currentRoom.DisableChangeEventColliderExitRoom();
        //1ºHacemos el RayCast en la direccion hacia la que es la puerta (pendiente modificar el metodo
        UpdateCurrentRoom(GameManager.instance.player, currentRoom.exitDoor, backwards);

        if (currentRoom.RoomComplete)
        {
            currentRoom.EnableChangeEventColliderEntranceRoom();
            currentRoom.EnableChangeEventColliderExitRoom();
        }
      

        //2º con la nueva habitacion actualizada primero, le decimos de donde venimos y cargamos el respawn
        //desactivar el ChangeRoomCollider de la entrada de esta habitacion
        //currentRoom.DisableChangeEventColliderEntranceRoom();
        //actualizamos la posicion del jugador
        GameManager.instance.player.UpdatePositionlevel(currentRoom.GetRespawnPositionPlayer(currentRoom.entranceDoor));//camino en direcion original

        //arrancamos los enemigos en esta habitacion
        currentRoom.ReStartRoom();

    }

    internal void CheckInsideBoundaries(Vector3 nextPosition)//ToDo:
    {
       // hacemos un raycast desde el centro de la habitacion hasta el punto

    }

    public void takeDamage( string colliderTag, Enemy enemy)
    {
        if (enemy.tag == colliderTag && (!enemy.checkIsInmune()))
        {
            enemy.TakeDamage(1);// TODO: weaponDagame
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

        ////localizamos la habitacion adyacente para abrir las puertas adyacentes
        //var adjSecretDoor = GameManager.instance.currentRoom.GetAdjacentSecretDoor(currentDirection);
        //currentRoom.OpenSecretDoor(adjSecretDoor);
        //adjSecretDoor.OpenSecretDoor();

        //Se desactiva el collider de la habitacion actual para localizar la contigua
        GameManager.instance.currentRoom.DisableColliderRoom();
        var adjRoom = GameManager.instance.GetAdjacentRoom(secretDoorobj.transform, currentDirection);
        GameManager.instance.currentRoom.EnableColliderRoom();

        //LevelGeneration.doorDirection adjDoorDirecion = GameManager.instance.GetReversalDoorDirection(currentDirection);
        //var adjDoors = adjRoom.GetDoorsByDirection(adjDoorDirecion);
        //foreach (var adDoor in adjDoors)
        //{
        //    adjRoom.OpenSecretDoor(adDoor.GetComponent<FRoomDoor>());
        //    //adDoor.GetComponent<FRoomDoor>().OpenSecretDoor();
        //}

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
    private void UpdateCurrentRoom( Player _player, LevelGeneration.doorDirection _doorDirection, bool backwards)
    {
        Vector2 rayCastDirection = Vector2.zero;
        switch (_doorDirection)
        {
            case LevelGeneration.doorDirection.down :
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
            currentRoom.SetEntranceExitDoor(backwards);
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
}
