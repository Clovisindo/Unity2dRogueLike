using UnityEngine;
using System;
using System.Collections.Generic; 		//Allows us to use Lists.
using Random = UnityEngine.Random;
using static LevelGeneration;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 16;                                         //Number of columns in our game board.
    public int rows = 8;                                            //Number of rows in our game board.
    public GameObject[] floorTiles;                                 //Array of floor prefabs.
    public GameObject[] outerWallFrontTiles;                             //Array of outer tile prefabs.
    public GameObject outerWallSideLeftTiles;                             //Array of outer tile prefabs.
    public GameObject outerWallSideRightTiles;                             //Array of outer tile prefabs.
    public List<GameObject> exitTiles = new List<GameObject>();
    public GameObject rightChangeRoomCollider;
    public GameObject roomDoorLeftRight;
    public GameObject roomDoorUpDown;

    private GameObject roomDoor;
    private doorDirection? currentDirectionDoor = null;

    public GameObject wallCornerTopLeft;                                // prefab corner wall
    public GameObject wallCornerTopRight;                                // prefab corner wall
    public GameObject wallCornerBottomRight;                                // prefab corner wall
    public GameObject wallCornerBottomLeft;                                // prefab corner wall

    private Quaternion angleExitCollider;
    private Quaternion angleExitDoor;

    public BoardRoom prefabBoardRoom;
    public GameObject prefabRoomCollider;
    public GameObject prefabRoomColliderInner;

    private BoardRoom generatedBoardRoom;
    private GameObject roomCollider;
    //private GameObject roomColliderInner;
    private GameObject colliderExit;
    private GameObject colliderEntrance;
    private GameObject colliderExtraDoor;

    private bool isEntrance = false;                                //mark is a exit tile
    private bool isCreatedEntrance = false;
    private bool isExit = false;                                //mark is a exit tile
    private bool isCreatedExit = false;
    private bool isExtraDoor = false;
    private bool isCreatedExtraDoor = false;

    private int index = 1;
    private float movColliderX;
    private float movColliderY;

    /// <summary>
    /// Sobrecarga para crear una habitacion instancia a una posicion determinada
    /// </summary>
    /// <param name="currentRoomPosition"> posicion inicial de instancia </param>
    /// <param name="previousSideRoom"> direccion puerta "entrada" </param>
    /// <param name="nextSideDoor"> direccion puerta "salida" </param>
    /// <returns></returns>
    public BoardRoom BoardSetup(Vector3 currentRoomPosition, doorDirection? previousSideRoom, doorDirection? nextSideDoor)
    {
        //Instantiate Board and set boardHolder to its transform.
        //GameObject board = new GameObject("Board");
        isExit = false;
        isEntrance = false;
        isCreatedEntrance = false;
        isCreatedExit = false;
        generatedBoardRoom = null;
        roomCollider = null;
        //roomColliderInner = null;
        generatedBoardRoom = Instantiate(prefabBoardRoom, new Vector3(0, 0, 0), Quaternion.identity);
        generatedBoardRoom.name = "BoardRoom_" + index.ToString();
        index++;
        roomCollider = Instantiate(prefabRoomCollider, new Vector3(0, 0, 0), Quaternion.identity);
        //roomColliderInner = Instantiate(prefabRoomColliderInner, new Vector3(0, 0, 0), Quaternion.identity);

        roomCollider.transform.SetParent(generatedBoardRoom.transform);
        //roomColliderInner.transform.SetParent(generatedBoardRoom.transform);

        generatedBoardRoom.colliderDetector = roomCollider.GetComponent<BoxCollider2D>();

        //agrupar gerarquia tiles
        GameObject tileSetRoom = new GameObject("tileSetRoom");

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = -1; x < columns + 1; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = -1; y < rows + 1; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if (x == -1 && y == -1)// WallCorners
                {
                    toInstantiate = wallCornerBottomLeft;
                }
                if (x == -1 && y == rows)
                {
                    toInstantiate = wallCornerTopLeft;
                }
                if (x == columns && y == -1)
                {
                    toInstantiate = wallCornerBottomRight;
                }
                if (x == columns && y == rows)
                {
                    toInstantiate = wallCornerTopRight;
                }
                if ((y == -1 || y == rows) && !((x == -1 && y == -1) || (x == -1 && y == rows) || (x == columns && y == -1) || (x == columns && y == rows))) //rows y que no sea esquina de fila
                {
                    if ((x == 7 || x == 8) && y == -1)
                    {
                        if (nextSideDoor == doorDirection.down)
                        {
                            isExit = true;//marcador para identificar salida y cambiar el tag del objeto
                            isCreatedExit = true;
                            currentDirectionDoor = nextSideDoor;
                        }
                        else if (previousSideRoom == doorDirection.down)
                        {
                            isEntrance = true;
                            isCreatedEntrance = true;
                            currentDirectionDoor = previousSideRoom;
                        }
                        else
                        {
                            isExtraDoor = true;
                            isCreatedExtraDoor = true;
                            currentDirectionDoor = doorDirection.down;
                        }

                        movColliderX = 0.5f;
                        movColliderY = -0.3f;
                        angleExitCollider = Quaternion.AngleAxis(90, Vector3.back);
                        angleExitDoor = Quaternion.identity;
                        roomDoor = roomDoorUpDown;
                    }
                    else if ((x == 7 || x == 8) && y == rows)
                    {
                        if (nextSideDoor == doorDirection.up)
                        {
                            isExit = true;//marcador para identificar salida y cambiar el tag del objeto
                            isCreatedExit = true;
                            currentDirectionDoor = nextSideDoor;
                        }
                        else if (previousSideRoom == doorDirection.up)
                        {
                            isEntrance = true;
                            isCreatedEntrance = true;
                            currentDirectionDoor = previousSideRoom;
                        }
                        else
                        {
                            isExtraDoor = true;
                            isCreatedExtraDoor = true;
                            currentDirectionDoor = doorDirection.up;
                        }
                        movColliderX = 0.5f;
                        movColliderY = 0.3f;
                        angleExitCollider = Quaternion.AngleAxis(90, Vector3.back);
                        angleExitDoor = Quaternion.identity;
                        roomDoor = roomDoorUpDown;
                    }
                    else
                    {
                        toInstantiate = outerWallFrontTiles[Random.Range(0, outerWallFrontTiles.Length)];
                    }
                }
                if ((x == -1) && !(x == -1 && y == -1) && !(x == -1 && y == rows))//collumns y que no sea esquina de columna LADO IZQUIERDO
                {
                    if (y == 3 || y == 4) // doors leftSide
                    {
                        if (nextSideDoor == doorDirection.left)
                        {
                            isExit = true;//marcador para identificar salida y cambiar el tag del objeto
                            isCreatedExit = true;
                            currentDirectionDoor = nextSideDoor;
                        }
                        else if (previousSideRoom == doorDirection.left)
                        {
                            isEntrance = true;
                            isCreatedEntrance = true;
                            currentDirectionDoor = previousSideRoom;
                        }
                        else
                        {
                            isExtraDoor = true;
                            isCreatedExtraDoor = true;
                            currentDirectionDoor = doorDirection.left;
                        }
                        movColliderX = -0.3f;
                        movColliderY = 0.5f;
                        angleExitCollider = Quaternion.identity;
                        angleExitDoor = Quaternion.AngleAxis(180, Vector3.back);
                        roomDoor = roomDoorLeftRight;
                    }
                    else
                    {
                        toInstantiate = outerWallSideLeftTiles;
                    }
                }
                if ((x == columns) && !(x == columns && y == -1) && !(x == columns && y == rows))//collumns y que no sea esquina de columna LADO DERECHO
                {
                    if (y == 3 || y == 4) // doors rightSide
                    {
                        if (nextSideDoor == doorDirection.right)
                        {
                            isExit = true;//marcador para identificar salida y cambiar el tag del objeto
                            isCreatedExit = true;
                            currentDirectionDoor = nextSideDoor;
                        }
                        else if (previousSideRoom == doorDirection.right)
                        {
                            isEntrance = true;
                            isCreatedEntrance = true;
                            currentDirectionDoor = previousSideRoom;
                        }
                        else
                        {
                            isExtraDoor = true;
                            isCreatedExtraDoor = true;
                            currentDirectionDoor = doorDirection.right;
                        }
                        movColliderX = 0.3f;
                        movColliderY = 0.5f;
                        angleExitCollider = Quaternion.identity;
                        angleExitDoor = Quaternion.identity;
                        roomDoor = roomDoorLeftRight;
                    }
                    else
                    {
                        toInstantiate = outerWallSideRightTiles;
                    }
                }
                //instanciar la puerta correspondiente
                if (isExit)
                {
                    GameObject exitRoomDoor = Instantiate(roomDoor, new Vector3(x, y, 0f), angleExitDoor) as GameObject;
                    if (isCreatedExit && colliderExit == null)
                    {
                        colliderExit = Instantiate(rightChangeRoomCollider, new Vector3(x + movColliderX, y + movColliderY, 0f), angleExitCollider) as GameObject;
                        colliderExit.tag = "Exit";
                        isCreatedExit = true;
                        colliderExit.transform.SetParent(exitRoomDoor.transform);
                    }
                    isExit = false;
                    exitRoomDoor.transform.SetParent(generatedBoardRoom.transform);
                    generatedBoardRoom.DctDoors1.Add(exitRoomDoor, currentDirectionDoor.Value);
                }
                if (isEntrance)
                {
                    GameObject entranceRoomDoor = Instantiate(roomDoor, new Vector3(x, y, 0f), angleExitDoor) as GameObject;
                    if (isCreatedEntrance && colliderEntrance == null)
                    {
                        colliderEntrance = Instantiate(rightChangeRoomCollider, new Vector3(x + movColliderX, y + movColliderY, 0f), angleExitCollider) as GameObject;
                        colliderEntrance.tag = "Entrance";
                        isCreatedEntrance = true;
                        colliderEntrance.transform.SetParent(entranceRoomDoor.transform);
                    }
                    isEntrance = false;
                    entranceRoomDoor.transform.SetParent(generatedBoardRoom.transform);
                    generatedBoardRoom.DctDoors1.Add(entranceRoomDoor, currentDirectionDoor.Value);
                }
                if (isExtraDoor)
                {
                    GameObject entranceRoomDoor = Instantiate(roomDoor, new Vector3(x, y, 0f), angleExitDoor) as GameObject;
                    if (isCreatedExtraDoor && colliderExtraDoor == null)
                    {
                        colliderExtraDoor = Instantiate(rightChangeRoomCollider, new Vector3(x + movColliderX, y + movColliderY, 0f), angleExitCollider) as GameObject;
                        colliderExtraDoor.tag = "NoDoor";
                        isCreatedExtraDoor = true;
                        colliderExtraDoor.transform.SetParent(entranceRoomDoor.transform);
                    }
                    isExtraDoor = false;
                    entranceRoomDoor.transform.SetParent(generatedBoardRoom.transform);
                    generatedBoardRoom.DctDoors1.Add(entranceRoomDoor, currentDirectionDoor.Value);
                    if (y == 4 || x == 8 && colliderExtraDoor != null)
                    {
                        colliderExtraDoor = null;
                    }
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(tileSetRoom.transform);
            }
        }

        tileSetRoom.transform.SetParent(generatedBoardRoom.transform);

        colliderEntrance = null;
        colliderExit = null;
        generatedBoardRoom.transform.position = currentRoomPosition;
        if (nextSideDoor != null)
        {
            generatedBoardRoom.InitialExitDirection = (doorDirection)nextSideDoor;
        }
        if (previousSideRoom != null)
        {
            generatedBoardRoom.InitialEntranceDirection = (doorDirection)previousSideRoom;
        }
        //Asignamos las variables necesarios
        generatedBoardRoom.SetParametersRoom();

        return generatedBoardRoom;
    }

}
