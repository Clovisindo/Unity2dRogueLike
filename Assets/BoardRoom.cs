using Assets.Scripts.Entities.Enemies;
using Assets.Scripts.EnumTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelGeneration;
using Helper = Assets.Utilities.Helper;

public class BoardRoom : MonoBehaviour
{
    public LevelGeneration.doorDirection entranceDirection;
    public LevelGeneration.doorDirection exitDirection;

    public GameObject respawnUp;
    public GameObject respawnDown;
    public GameObject respawnLeft;
    public GameObject respawnRight;

    private RoomParameters roomParameters;

    public BoxCollider2D colliderDetector;

    //LevelBoundaries
    private Vector2 screenBoundsMax;
    private Vector2 screenBoundsMin;

    private BoxCollider2D[] changeRoomsEventColliderEntrance;
    private BoxCollider2D[] changeRoomsEventColliderExit;
    private BoxCollider2D[] changeRoomsEventColliderNoDoor;
    private BoxCollider2D[] changeRoomsEventColliderSecretDoor;

    private FRoomDoor[] roomDoor;
    private Dictionary<GameObject, doorDirection> DctDoors = new Dictionary<GameObject, doorDirection>();

    public List<Enemy> enemiesRoom;

    public List<fFloorMechanic> fPiecesRoom;

    public LevelGeneration.doorDirection InitialExitDirection { get => exitDirection; set => exitDirection = value; }
    public LevelGeneration.doorDirection InitialEntranceDirection { get => entranceDirection; set => entranceDirection = value; }
    public bool RoomComplete { get; internal set; }
    public Dictionary<GameObject, doorDirection> DctDoors1 { get => DctDoors; set => DctDoors = value; }//Contiene los objetos FRoomDoor
    public RoomParameters RoomParameters { get => roomParameters; set => roomParameters = value; }
    public Vector2 ScreenBoundsMax { get => screenBoundsMax; set => screenBoundsMax = value; }
    public Vector2 ScreenBoundsMin { get => screenBoundsMin; set => screenBoundsMin = value; }

    internal void PauseRoom()
    {
        //implementar algo de puzzles
        foreach (var enemy in enemiesRoom)
        {
            enemy.IsPaused = true;
        }
    }

    internal void ReStartRoom()
    {
        //implementar algo de puzzles
        foreach (var enemy in enemiesRoom)
        {
            enemy.IsPaused = false;
        }

        if (RoomParameters.TypeRoom == EnumTypeRoom.secundary  || RoomParameters.TypeRoom == EnumTypeRoom.secret)
        {
            this.OpenDoor();
        }
    }

    public  LevelGeneration.doorDirection entranceDoor;
    public  LevelGeneration.doorDirection exitDoor;

    


    // Start is called before the first frame update
    void Start()
    {
        RoomComplete = false;
        Collider2D colliderRoomInner = Helper.FindComponentInChildWithTag<Collider2D>(this.gameObject,"RoomColliderInner");

        ScreenBoundsMax = new Vector2(colliderRoomInner.bounds.max.x, colliderRoomInner.bounds.max.y);
        ScreenBoundsMin = new Vector2(colliderRoomInner.bounds.min.x, colliderRoomInner.bounds.min.y);
    }
    /// <summary>
    /// Carga los colliders de la habitacion agrupados por tipo
    /// </summary>
    internal void SetParametersRoom()
    {
        changeRoomsEventColliderEntrance = Helper.FindComponentsInChildsWithTag<BoxCollider2D>(DctDoors1.Keys.ToArray(), "Entrance");
        if (changeRoomsEventColliderEntrance != null){foreach (var eventColliderEntrance in changeRoomsEventColliderEntrance){eventColliderEntrance.enabled = false;}}
        changeRoomsEventColliderExit = Helper.FindComponentsInChildsWithTag<BoxCollider2D>(DctDoors1.Keys.ToArray(), "Exit");
        if (changeRoomsEventColliderExit != null) { foreach (var eventColliderExit in changeRoomsEventColliderExit) { eventColliderExit.enabled = false;}}

        changeRoomsEventColliderNoDoor = Helper.FindComponentsInChildsWithTag<BoxCollider2D>(DctDoors1.Keys.ToArray(), "NoDoor");
        if (changeRoomsEventColliderNoDoor != null) { foreach (var eventColliderNoDoor in changeRoomsEventColliderNoDoor) { eventColliderNoDoor.enabled = false;}}

        changeRoomsEventColliderSecretDoor = Helper.FindComponentsInChildsWithTag<BoxCollider2D>(DctDoors1.Keys.ToArray(), "SecretDoor");
        if (changeRoomsEventColliderSecretDoor != null) { foreach (var eventColliderExit in changeRoomsEventColliderExit) { eventColliderExit.enabled = false;}}

        roomDoor = Helper.FindComponentsInChildWithTag<FRoomDoor>(this.transform.gameObject, "FRoomDoor");
    }
    /// <summary>
    /// Log de la generacion de puertas
    /// </summary>
    public void LogCurrentRoom(RoomParameters roomParameters)
    {
        // tipo de habitacion y nombre
        Debug.Log(" Habitación " + this.gameObject.name + " generada, de tipo : " + roomParameters.TypeRoom + ".");
        //recorrer las puertas por direccion y tipo de puerta
        foreach (var roomDoor in roomParameters.RoomDoors)
        {
            Debug.Log(" Puerta de direccion : "  + roomDoor.Key + " definida como tipo " + roomDoor.Value);
        }
    }
    /// <summary>
    /// Obtenemos la direccion de una puerta de la habitacion
    /// </summary>
    /// <param name="currentDoor"></param>
    /// <returns></returns>
    public doorDirection GetDirectionByDoor( GameObject currentDoor)
    {
        return DctDoors1.Where(d => d.Key == currentDoor).Select(d => d.Value).FirstOrDefault();
    }
    /// <summary>
    /// Obtenemos las puertas de la habitacion para una direccion
    /// </summary>
    /// <param name="currentDirection"></param>
    /// <returns></returns>
    public IEnumerable<GameObject> GetDoorsByDirection(doorDirection currentDirection)
    {
        return DctDoors1.Where(d => d.Value == currentDirection).Select(d => d.Key).ToList();
    }
    /// <summary>
    /// Dada una direccion , localizamos la puerta y su collider y le asignamos el nuevo estado
    /// </summary>
    /// <param name="currentDoorDirection"></param>
    public void UpdateColliderDoor ( doorDirection currentDoorDirection, EnumTypeDoor currentTypeDoor)
    {
        var doorsToUpdate = GetDoorsByDirection(currentDoorDirection);
        foreach (var doorToUpdate in doorsToUpdate)
        {
          var doorCollider = doorToUpdate.GetComponentsInChildren<BoxCollider2D>().Where(t => t.tag != "Door").FirstOrDefault();//"Door" es el objeto padre ignorar
            if (doorCollider != null)//en una de las puertas no hay collider
            {
                doorCollider.tag = GetTagDoorByType(currentTypeDoor);
            }
        }
    }
    /// <summary>
    /// Obtener tag por tipo de puerta
    /// </summary>
    /// <param name="typeDoor"></param>
    /// <returns></returns>
    public string GetTagDoorByType ( EnumTypeDoor typeDoor)
    {
        string tag = null;
        switch (typeDoor)
        {
            case EnumTypeDoor.none:
                tag = "NoDoor";
                break;
            case EnumTypeDoor.entrance:
                tag = "Entrance";
                break;
            case EnumTypeDoor.exit:
                tag = "Exit";
                break;
            case EnumTypeDoor.secret:
                tag = "SecretDoor";
                break;
        }
        return tag;
    }
    /// <summary>
    /// Se actualizan los colliderRoom que hayan cambiado(todos)
    /// Se actualizan las puertas en funcion del tipo definido en los parametros
    /// Si es normal no se hace nada
    /// Si es secreta isSecret = true
    /// Si es no existe(secundaria) active = false
    /// </summary>
    /// <param name="roomDoors"> Diccionario de direccion de puerta y tipo de puerta</param>
    public void UpdateDoorsByParameters(Dictionary<doorDirection, EnumTypeDoor> roomDoors)
    {
        foreach (var roomDoor in roomDoors)
        {
            UpdateColliderDoor(roomDoor.Key, roomDoor.Value);
        }
        SetParametersRoom();

        foreach (var paramDoor in roomDoors)
        {
            if (paramDoor.Value == EnumTypeDoor.secret)
            {
                var roomDoorsUpdt = this.GetDoorsByDirection(paramDoor.Key).ToList();
                foreach (var roomDoorUpdt in roomDoorsUpdt)
                {
                    roomDoorUpdt.GetComponent<FRoomDoor>().IsSecretDoor = true;
                }
                
            }
            if (paramDoor.Value == EnumTypeDoor.none)
            {
                var roomDoorsUpdt = this.GetDoorsByDirection(paramDoor.Key).ToList();
                foreach (var roomDoorUpdt in roomDoorsUpdt)
                {
                    roomDoorUpdt.GetComponent<FRoomDoor>().isNotDoor = true;
                }
            }
        }
    }
    /// <summary>
    ///Abre las puertas de la habitacion por finalizar el evento
    ///Si es una habitacion secreta, abre las puertas secretas
    /// </summary>
    internal void OpenDoor()
    {
        foreach (var door in roomDoor)
        {
            door.OpenDoor();
        }
        if (roomParameters.TypeRoom == EnumTypeRoom.secret)
        {
            var secretDoors = roomDoor.Where(f => f.IsSecretDoor);
            foreach (var secretDoor in secretDoors)
            {
                OpenSecretDoor(secretDoor);
            }
        }
        EnableChangeEventColliderEntranceRoom();
        EnableChangeEventColliderExitRoom();
    }
    /// <summary>
    /// Abre la puerta secreta con FRoomDoor
    /// </summary>
    /// <param name="FroomDoor"></param>
    internal void OpenSecretDoor(FRoomDoor FroomDoor)
    {
        FroomDoor.OpenSecretDoor();

        EnableChangeEventColliderSecretRoom();
    }

    internal void CloseDoor()
    {
        foreach (var door in roomDoor)
        {
            door.CloseDoor();
        }
        if (roomParameters.TypeRoom == EnumTypeRoom.secret)
        {
            var secretDoors = roomDoor.Where(f => f.IsSecretDoor);
            foreach (var secretDoor in secretDoors)
            {
                CloseSecretDoor(secretDoor);
            }
        }
        DisableChangeEventColliderEntranceRoom();
        DisableChangeEventColliderExitRoom();
    }

    internal void CloseSecretDoor(FRoomDoor FroomDoor)
    {
        FroomDoor.CloseSecretDoor();

        DisableChangeEventColliderSecretRoom();
    }
    /// <summary>
    /// Obtenemos la posicion del jugador en la habitacion segun la puerta de entrada
    /// </summary>
    /// <param name="_doorDirection"></param>
    /// <returns></returns>
    public Vector2 GetRespawnPositionPlayer (LevelGeneration.doorDirection _doorDirection)
    {
        Vector2 respawnPosition = Vector2.zero;

        switch (_doorDirection)
        {
            case LevelGeneration.doorDirection.down:
                respawnPosition = respawnDown.transform.position;
                break;
            case LevelGeneration.doorDirection.up:
                respawnPosition = respawnUp.transform.position;
                break;
            case LevelGeneration.doorDirection.left:
                respawnPosition = respawnLeft.transform.position;
                break;
            case LevelGeneration.doorDirection.right:
                respawnPosition = respawnRight.transform.position;
                break;
        }
        return respawnPosition;
    }
    /// <summary>
    /// invoca a los enemigos en las posiciones correspondientes
    /// </summary>
    /// <param name="initPositionsEnemy">array de posiciones de spawn</param>
    /// <param name="enemies">lista de enemigos generada por EventRoomCollide</param>
    internal void InvokeEnemies(List<Transform> initPositionsEnemy, List<Enemy> enemies)
    {
        int i = 0;

        foreach (var enemy in enemies)
        {
            //initPositionsEnemy[i].transform.position = enemy.GetRespawnPosition();
            //emptyObject.transform.SetParent(this.transform);
            Transform positionEnemy = Instantiate(initPositionsEnemy[i], this.transform.position, Quaternion.identity);
            positionEnemy.SetParent(this.transform);
            positionEnemy.transform.position = transform.TransformPoint(initPositionsEnemy[i].transform.position);
            Enemy currentEnemy = Instantiate(enemy, positionEnemy.transform.position, Quaternion.identity);
            enemiesRoom.Add(currentEnemy);
            currentEnemy.transform.SetParent(this.transform);
            i++;
        }
    }

    internal void InvokeFPieces(List<Transform> initPositionsEnemy, List<fFloorMechanic> fPieces)
    {
        int i = 0;

        foreach (var fPiece in fPieces)
        {
            initPositionsEnemy[i].transform.position = fPiece.RespawnPosition;
            //emptyObject.transform.SetParent(this.transform);
            Transform positionEnemy = Instantiate(initPositionsEnemy[i], this.transform.position, Quaternion.identity);
            positionEnemy.SetParent(this.transform);
            positionEnemy.transform.position = transform.TransformPoint(initPositionsEnemy[i].transform.position);
            fFloorMechanic currentFPiece = Instantiate(fPiece, positionEnemy.transform.position, Quaternion.identity);
            fPiecesRoom.Add(currentFPiece);
            currentFPiece.transform.SetParent(this.transform);
            i++;
        }
    }

    public void DisableColliderRoom()
    {
        this.colliderDetector.enabled = false;
    }
    public void EnableColliderRoom()
    {
        this.colliderDetector.enabled = true;
    }

    public void DisableChangeEventColliderEntranceRoom()
    {
        if (changeRoomsEventColliderEntrance != null) { foreach (var eventColliderEntrance in changeRoomsEventColliderEntrance) { eventColliderEntrance.enabled = false; } }
    }

    public void DisableChangeEventColliderExitRoom()
    {
        if (changeRoomsEventColliderExit != null) { foreach (var eventColliderExit in changeRoomsEventColliderExit) { eventColliderExit.enabled = false; } }
    }
    public void DisableChangeEventColliderSecretRoom()
    {
        if (changeRoomsEventColliderSecretDoor != null) { foreach (var eventColliderSecret in changeRoomsEventColliderSecretDoor) { eventColliderSecret.enabled = false; } }
    }

    public void EnableChangeEventColliderEntranceRoom()
    {
        if (changeRoomsEventColliderEntrance != null) { foreach (var eventColliderEntrance in changeRoomsEventColliderEntrance) { eventColliderEntrance.enabled = true; } }
    }

    public void EnableChangeEventColliderExitRoom()
    {
        if (changeRoomsEventColliderExit != null) { foreach (var eventColliderExit in changeRoomsEventColliderExit) { eventColliderExit.enabled = true; } }
    }

    public void EnableChangeEventColliderSecretRoom()
    {
        if (changeRoomsEventColliderSecretDoor != null) { foreach (var eventColliderSecret in changeRoomsEventColliderSecretDoor) { eventColliderSecret.enabled = true; } }
    }


}
