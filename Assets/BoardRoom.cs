using System.Collections;
using System.Collections.Generic;
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

    public LevelGeneration.doorDirection InitialExitDirection { get => exitDirection; set => exitDirection = value; }
    public LevelGeneration.doorDirection InitialEntranceDirection { get => entranceDirection; set => entranceDirection = value; }

    public  LevelGeneration.doorDirection entranceDoor;
    public  LevelGeneration.doorDirection exitDoor;


    // Start is called before the first frame update
    void Start()
    {
        changeRoomEventColliderEntrance = Helper.FindComponentInChildWithTag<BoxCollider2D>(this.transform.gameObject,"Entrance");
        changeRoomEventColliderExit = Helper.FindComponentInChildWithTag<BoxCollider2D>(this.transform.gameObject, "Exit");
        //colliderDetector = GameObject.FindGameObjectWithTag("RoomCollider").GetComponent<BoxCollider2D>();

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
