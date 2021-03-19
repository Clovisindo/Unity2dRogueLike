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
    //public Count wallCount = new Count(5, 9);                       //Lower and upper limit for our random number of walls per level.
    //public Count foodCount = new Count(1, 5);                       //Lower and upper limit for our random number of food items per level.
    //public GameObject exit;                                         //Prefab to spawn for exit.
    public GameObject[] floorTiles;                                 //Array of floor prefabs.
    //public GameObject[] wallTiles;                                  //Array of wall prefabs.
    //public GameObject[] foodTiles;                                  //Array of food prefabs.
    //public GameObject[] enemyTiles;                                 //Array of enemy prefabs.
    public GameObject[] outerWallFrontTiles;                             //Array of outer tile prefabs.
    public GameObject outerWallSideLeftTiles;                             //Array of outer tile prefabs.
    public GameObject outerWallSideRightTiles;                             //Array of outer tile prefabs.
    //public GameObject[] outerWallTopTiles;                             //Array of outer tile prefabs.
    public List<GameObject> exitTiles = new List<GameObject>();
    public GameObject rightChangeRoomCollider;
    public GameObject roomDoorLeftRight;
    public GameObject roomDoorUpDown;
    private GameObject roomDoor;


    public GameObject wallCornerTopLeft;                                // prefab corner wall
    public GameObject wallCornerTopRight;                                // prefab corner wall
    public GameObject wallCornerBottomRight;                                // prefab corner wall
    public GameObject wallCornerBottomLeft;                                // prefab corner wall


    private Transform boardHolder;                                  //A variable to store a reference to the transform of our Board object.
    private List<Vector3> gridPositions = new List<Vector3>();	//A list of possible locations to place tiles.
    private Quaternion angleExitCollider;
    private Quaternion angleExitDoor;

    public BoardRoom prefabBoardRoom;
    private BoardRoom generatedBoardRoom;
    public GameObject prefabRoomCollider;
    private GameObject roomCollider;
    private GameObject colliderExit;
    private GameObject colliderEntrance;

    private bool isEntrance = false;                                //mark is a exit tile
    private bool isCreatedEntrance = false;
    private bool isExit = false;                                //mark is a exit tile
    private bool isCreatedExit = false;
    private int index = 1;
    private float movColliderX;
    private float movColliderY;


    //Clears our list gridPositions and prepares it to generate a new board.
    void InitialiseList()
    {
        //Clear our list gridPositions.
        gridPositions.Clear();

        //Loop through x axis (columns).
        for (int x = 1; x < columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    //Sets up the outer walls and floor (background) of the game board.
    /// <summary>
    /// DEPRECATED
    /// </summary>
    //public void BoardSetup()
    //{
    //    //Instantiate Board and set boardHolder to its transform.
    //    boardHolder = new GameObject("Board").transform;

    //    //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
    //    for (int x = -1; x < columns + 1; x++)
    //    {
    //        //Loop along y axis, starting from -1 to place floor or outerwall tiles.
    //        for (int y = -1; y < rows + 1; y++)
    //        {
    //            //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
    //            GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

    //            if (x == -1 && y == -1)// WallCorners
    //            {
    //                toInstantiate = wallCornerBottomLeft;
    //            }
    //            if (x == -1 && y == rows)
    //            {
    //                toInstantiate = wallCornerTopLeft;
    //            }
    //            if (x == columns && y == -1)
    //            {
    //                toInstantiate = wallCornerBottomRight;
    //            }
    //            if (x == columns && y == rows)
    //            {
    //                toInstantiate = wallCornerTopRight;
    //            }
    //            if ((y == -1 || y == rows) && !((x == -1 && y == -1) || (x == -1 && y == rows) || (x == columns && y == -1) || (x == columns && y == rows))) //rows y que no sea esquina de fila
    //            {
    //                toInstantiate = outerWallFrontTiles[Random.Range(0, outerWallFrontTiles.Length)];
    //            }

    //            if ( (x == -1) && !(x == -1 && y == -1)  && !(x == -1 && y == rows))//collumns y que no sea esquina de columna
    //            {
    //                toInstantiate = outerWallSideLeftTiles;
    //            }
    //            if ((x == columns) && !(x == columns && y == -1) && !(x == columns && y == rows))//collumns y que no sea esquina de columna
    //            {
    //                toInstantiate = outerWallSideRightTiles;
    //            }

    //            //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
    //            GameObject instance =
    //                Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

    //            //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
    //            instance.transform.SetParent(boardHolder);
    //        }
    //    }
    //}

    /// <summary>
    /// Sobrecarga para crear una habitacion instancia a una posicion determinada
    /// </summary>
    /// <param name="gridRoomLevelPosition"></param>
   public BoardRoom BoardSetup(Vector3 gridRoomLevelPosition, doorDirection? previousSideRoom, doorDirection? nextSideDoor )
    {
        //Instantiate Board and set boardHolder to its transform.
        //GameObject board = new GameObject("Board");
        isExit = false;
        isEntrance = false;
        isCreatedEntrance = false;
        isCreatedExit = false;
        generatedBoardRoom = null;
        roomCollider = null;
        generatedBoardRoom = Instantiate(prefabBoardRoom, new Vector3(0,0,0), Quaternion.identity);
        generatedBoardRoom.name = "BoardRoom_" + index.ToString();
        index++;
        roomCollider = Instantiate(prefabRoomCollider, new Vector3(0, 0, 0), Quaternion.identity);

        roomCollider.transform.SetParent(generatedBoardRoom.transform);

        generatedBoardRoom.colliderDetector = roomCollider.GetComponent<BoxCollider2D>();

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
                    if ((x == 7 || x == 8) && y == -1 && (nextSideDoor == doorDirection.down || previousSideRoom == doorDirection.down))
                    {
                        if (nextSideDoor == doorDirection.down)
                        {
                            isExit = true;//marcador para identificar salida y cambiar el tag del objeto
                            isCreatedExit = true;
                        }
                        else if (previousSideRoom == doorDirection.down)
                        {
                            isEntrance = true;
                            isCreatedEntrance = true;
                        }
                        movColliderX = 0.5f;
                        movColliderY = -0.3f;
                        angleExitCollider = Quaternion.AngleAxis(90, Vector3.back);
                        angleExitDoor = Quaternion.identity;
                        roomDoor = roomDoorUpDown;
                    }
                    else if ((x == 7 || x == 8) && y == rows && (nextSideDoor == doorDirection.up || previousSideRoom == doorDirection.up))
                    {
                        if (nextSideDoor == doorDirection.up)
                        {
                            isExit = true;//marcador para identificar salida y cambiar el tag del objeto
                            isCreatedExit = true;
                        }
                        else if (previousSideRoom == doorDirection.up)
                        {
                            isEntrance = true;
                            isCreatedEntrance = true;
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
                    if ((y == 3 || y == 4) && (nextSideDoor == doorDirection.left || previousSideRoom == doorDirection.left))// doors leftSide
                    {
                        if (nextSideDoor == doorDirection.left)
                        {
                            isExit = true;//marcador para identificar salida y cambiar el tag del objeto
                            isCreatedExit = true;
                        }
                        else if (previousSideRoom == doorDirection.left)
                        {
                            isEntrance = true;
                            isCreatedEntrance = true;
                        }
                        movColliderX = -0.3f;
                        movColliderY = 0.5f;
                        angleExitCollider = Quaternion.identity;
                        angleExitDoor = Quaternion.AngleAxis(180, Vector3.back); ;
                        roomDoor = roomDoorLeftRight;
                    }
                    else
                    {
                        toInstantiate = outerWallSideLeftTiles;
                    }
                }
                if ((x == columns) && !(x == columns && y == -1) && !(x == columns && y == rows))//collumns y que no sea esquina de columna LADO DERECHO
                {
                    if ((y == 3 || y ==4) && (nextSideDoor == doorDirection.right || previousSideRoom == doorDirection.right) )// doors rightSide
                    {
                        if (nextSideDoor == doorDirection.right)
                        {
                            isExit = true;//marcador para identificar salida y cambiar el tag del objeto
                            isCreatedExit = true;
                        }
                        else if (previousSideRoom == doorDirection.right)
                        {
                            isEntrance = true;
                            isCreatedEntrance = true;
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

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                
                if (isExit)
                {
                    if (isCreatedExit && colliderExit == null)
                    {
                        colliderExit = Instantiate(rightChangeRoomCollider, new Vector3(x + movColliderX, y + movColliderY, 0f), angleExitCollider) as GameObject;
                        colliderExit.tag = "Exit";
                        isCreatedExit = true;
                        colliderExit.transform.SetParent(generatedBoardRoom.transform);
                    }
                    //girar si es de izquierdas o de derechas
                    GameObject exitRoomDoor = Instantiate(roomDoor, new Vector3(x , y , 0f), angleExitDoor) as GameObject;
                    isExit = false;
                    exitRoomDoor.transform.SetParent(generatedBoardRoom.transform);
                }
                if (isEntrance)
                {
                    if (isCreatedEntrance && colliderEntrance == null)
                    {
                        colliderEntrance = Instantiate(rightChangeRoomCollider, new Vector3(x + movColliderX, y + movColliderY, 0f), angleExitCollider) as GameObject;
                        colliderEntrance.tag = "Entrance";
                        isCreatedEntrance = true;
                        colliderEntrance.transform.SetParent(generatedBoardRoom.transform);
                    }
                    GameObject entranceRoomDoor = Instantiate(roomDoor, new Vector3(x, y, 0f), angleExitDoor) as GameObject;
                    isEntrance = false;
                    entranceRoomDoor.transform.SetParent(generatedBoardRoom.transform);
                }

                //if (isExit)
                //{
                //    toInstantiate = roomDoor;
                //    isExit = false;
                //}
                //if (isEntrance)
                //{
                //    toInstantiate = roomDoor;
                //    isEntrance = false;
                //}

                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(generatedBoardRoom.transform);
            }
        }
        colliderEntrance = null;
        colliderExit = null;
        generatedBoardRoom.transform.position = gridRoomLevelPosition;
        generatedBoardRoom.InitialExitDirection = (doorDirection)nextSideDoor;
        if (previousSideRoom != null)
        {
            generatedBoardRoom.InitialEntranceDirection = (doorDirection)previousSideRoom;
        }

        //Asignamos las variables necesarios
        generatedBoardRoom.SetParametersRoom();

        return generatedBoardRoom;
    }

    /// <summary>
    /// DEPRECATED
    /// </summary>
    /// <param name="roomSpawnPosition"></param>
    /// <returns></returns>
    private Vector3 worldToPivot(Vector3 roomSpawnPosition)
    {
        return new Vector3(roomSpawnPosition.x - (roomSpawnPosition.x /2), roomSpawnPosition.y - (roomSpawnPosition.y / 2 ));
    }

    //RandomPosition returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        //Return the randomly selected Vector3 position.
        return randomPosition;
    }


    //LayoutObjectAtRandom accepts an array of game objects to choose from along with a minimum and maximum range for the number of objects to create.
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //Choose a random number of objects to instantiate within the minimum and maximum limits
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Choose a random tile from tileArray and assign it to tileChoice
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene(int level)//DEPRECATED
    {
        //Creates the outer walls and floor.
        //BoardSetup();

        //Reset our list of gridpositions.
        //InitialiseList();

        //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        //LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

        //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
        //LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        //Determine number of enemies based on current level number, based on a logarithmic progression
        //int enemyCount = (int)Mathf.Log(level, 2f);

        //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
        //LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

        //Instantiate the exit tile in the upper right hand corner of our game board
        //Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
