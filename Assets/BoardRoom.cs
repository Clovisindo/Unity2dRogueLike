using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardRoom : MonoBehaviour
{
    public LevelGeneration.doorDirection entranceDirection;
    public LevelGeneration.doorDirection exitDirection;

    public GameObject respawnUp;
    public GameObject respawnDown;
    public GameObject respawnLeft;
    public GameObject respawnRight;

    public BoxCollider2D colliderDetector;

    private BoxCollider2D changeRoomEventColliderEntrance;
    private BoxCollider2D changeRoomEventColliderExit;

    private FRoomDoor[] roomDoor;

    public List<Enemy> enemiesRoom;

    

    public LevelGeneration.doorDirection InitialExitDirection { get => exitDirection; set => exitDirection = value; }
    public LevelGeneration.doorDirection InitialEntranceDirection { get => entranceDirection; set => entranceDirection = value; }

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
        changeRoomEventColliderEntrance = Helper.FindComponentInChildWithTag<BoxCollider2D>(this.transform.gameObject,"Entrance");
        changeRoomEventColliderExit = Helper.FindComponentInChildWithTag<BoxCollider2D>(this.transform.gameObject, "Exit");
        //roomDoor = Helper.FindComponentInChildWithTag<FRoomDoor>(this.transform.gameObject, "FRoomDoor");
        roomDoor = Helper.FindComponentsInChildWithTag <FRoomDoor>(this.transform.gameObject, "FRoomDoor");// GameObject.FindGameObjectWithTag("FRoomDoor").GetComponent<FRoomDoor>();
        //colliderDetector = GameObject.FindGameObjectWithTag("RoomCollider").GetComponent<BoxCollider2D>();
    }

    internal void OpenDoor()
    {
        foreach (var door in roomDoor)
        {
            door.OpenDoor();
        }
        
    }

    internal void CloseDoor()
    {
        foreach (var door in roomDoor)
        {
            door.CloseDoor();
        }

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
        changeRoomEventColliderEntrance.enabled = false;
    }

    public void DisableChangeEventColliderExitRoom()
    {
        changeRoomEventColliderExit.enabled = false;
    }

    public void EnableChangeEventColliderEntranceRoom()
    {
        changeRoomEventColliderEntrance.enabled = true;
    }

    public void EnableChangeEventColliderExitRoom()
    {
        changeRoomEventColliderExit.enabled = true;
    }

}
