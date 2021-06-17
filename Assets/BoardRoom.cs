using Assets.Scripts.EnumTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static LevelGeneration;

public class BoardRoom : MonoBehaviour
{
    public LevelGeneration.doorDirection entranceDirection;
    public LevelGeneration.doorDirection exitDirection;

    public GameObject respawnUp;
    public GameObject respawnDown;
    public GameObject respawnLeft;
    public GameObject respawnRight;

    public BoxCollider2D colliderDetector;

    private BoxCollider2D[] changeRoomsEventColliderEntrance;
    private BoxCollider2D[] changeRoomsEventColliderExit;
    private BoxCollider2D[] changeRoomsEventColliderNoDoor;
    private BoxCollider2D[] changeRoomsEventColliderSecretDoor;
    private BoxCollider2D[] changeRoomsEventColliderSecondaryDoor;

    private FRoomDoor[] roomDoor;
    private Dictionary<GameObject, doorDirection> DctDoors = new Dictionary<GameObject, doorDirection>();

    public List<Enemy> enemiesRoom;

    

    public LevelGeneration.doorDirection InitialExitDirection { get => exitDirection; set => exitDirection = value; }
    public LevelGeneration.doorDirection InitialEntranceDirection { get => entranceDirection; set => entranceDirection = value; }
    public bool RoomComplete { get; internal set; }
    public Dictionary<GameObject, doorDirection> DctDoors1 { get => DctDoors; set => DctDoors = value; }//Contiene los objetos FRoomDoor

    internal void PauseRoom()
    {
        // en bucle repasar todos los enemigos y poner el bool de paused

        //implementar algo de puzzles
        foreach (var enemy in enemiesRoom)
        {
            enemy.IsPaused = true;
        }
    }

    internal void ReStartRoom()
    {
        // en bucle repasar todos los enemigos y quitar el bool de paused

        //implementar algo de puzzles
        foreach (var enemy in enemiesRoom)
        {
            enemy.IsPaused = false;
        }
    }

    public  LevelGeneration.doorDirection entranceDoor;
    public  LevelGeneration.doorDirection exitDoor;

    


    // Start is called before the first frame update
    void Start()
    {
        RoomComplete = false;
    }

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
        //changeRoomsEventColliderSecondaryDoor = Helper.FindComponentsInChildsWithTag<BoxCollider2D>(DctDoors1.Keys.ToArray(), "SecretDoor2");
        //if (changeRoomsEventColliderSecondaryDoor != null) { changeRoomsEventColliderSecondaryDoor.enabled = false; }

        roomDoor = Helper.FindComponentsInChildWithTag<FRoomDoor>(this.transform.gameObject, "FRoomDoor");
        //colliderDetector = GameObject.FindGameObjectWithTag("RoomCollider").GetComponent<BoxCollider2D>();
    }

    public doorDirection GetDirectionByDoor( GameObject currentDoor)
    {
        ////ToDo: arreglar que le pasemos en la llamada el objeto directamente, no las transformaciones del parent, pues a veces venimos desde objetos distintos
        //var test = DctDoors1.Where(d => d.Key == currentDoor).Select(d => d.Value).ToList();

        return DctDoors1.Where(d => d.Key == currentDoor).Select(d => d.Value).FirstOrDefault();
    }

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
          var doorCollider = doorToUpdate.GetComponentsInChildren<BoxCollider2D>().Where(t => t.tag != "Door").FirstOrDefault();
            if (doorCollider != null)//en una de las puertas no hay collider
            {
                doorCollider.tag = GetTagDoorByType(currentTypeDoor);
            }
        }
    }

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

    public FRoomDoor GetAdjacentSecretDoor(doorDirection currentDirection)
    {
        //buscamos la otra puerta todavia activa
        var secretDoor = DctDoors1.Where(d => d.Value == currentDirection && d.Key.GetComponent<FRoomDoor>().IsSecretDoor &&
        d.Key.GetComponent<FRoomDoor>().IsClosed).Select(d => d.Key).FirstOrDefault();

        if (secretDoor != null)
        {
            return secretDoor.GetComponent<FRoomDoor>();
        }
        else
        {
            Debug.Log(" No se encuentra la otra puerta secreta.");
            return null;
        }
    }

    public void OpenAdjSecretDoor(doorDirection currentDirection)
    {
        //buscamos la otra puerta todavia activa
        var secretDoor = DctDoors1.Where(d => d.Value == currentDirection && d.Key.GetComponent<FRoomDoor>().IsSecretDoor &&
        d.Key.GetComponent<FRoomDoor>().IsClosed).Select( d => d.Key).FirstOrDefault();

        if (secretDoor != null)
        {
            secretDoor.GetComponent<FRoomDoor>().OpenSecretDoor();
        }
        else
        {
            Debug.Log(" No se encuentra la otra puerta secreta.");
        }
    }

    internal void OpenDoor()
    {
        foreach (var door in roomDoor)
        {
            door.OpenDoor();
        }
        EnableChangeEventColliderEntranceRoom();
        EnableChangeEventColliderExitRoom();
    }

    internal void CloseDoor()
    {
        foreach (var door in roomDoor)
        {
            door.CloseDoor();
        }
        DisableChangeEventColliderEntranceRoom();
        DisableChangeEventColliderExitRoom();
    }
    /// <summary>
    /// Return clossed == true
    /// </summary>
    /// <returns></returns>
    internal bool CheckDoorStatus()//debug
    {
       return roomDoor[0].CheckDoorIsClosed();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// En cada nueva iteracion con la habitacion actual, actualizamos esta informacion
    /// </summary>
    /// <param name="entrance"></param>
    /// <param name="exit"></param>
    public void SetEntranceExitDoor ( bool backwards)
    {
        if (!backwards)//sentido original
        {
            entranceDoor = entranceDirection;
            exitDoor = exitDirection;
        }
        else //sentido contrario
        {
            entranceDoor = exitDirection;
            exitDoor = entranceDirection;
        }
      
    }

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
    /// 
    /// </summary>
    /// <param name="initPositionsEnemy">array de posiciones de spawn</param>
    /// <param name="enemies">lista de enemigos generada por EventRoomCollide</param>
    internal void InvokeEnemies(List<Transform> initPositionsEnemy, List<Enemy> enemies)
    {
        //Transform[] arrayPositions = initPositionsEnemy.ToArray();
        //GameObject emptyObject = Instantiate(new GameObject(), this.transform.position, Quaternion.identity);
        //recorremos la lista de enemigos e instanciamos y añadimos a la clase 
        int i = 0;

        foreach (var enemy in enemies)
        {
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

    public void EnableChangeEventColliderEntranceRoom()
    {
        if (changeRoomsEventColliderEntrance != null) { foreach (var eventColliderEntrance in changeRoomsEventColliderEntrance) { eventColliderEntrance.enabled = true; } }
    }

    public void EnableChangeEventColliderExitRoom()
    {
        if (changeRoomsEventColliderExit != null) { foreach (var eventColliderExit in changeRoomsEventColliderExit) { eventColliderExit.enabled = true; } }
    }

   
}
