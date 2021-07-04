using Assets.Scripts.EnumTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Esta clase es el recuadro que va moviendose y generando cada fase al empezar
/// Ademas gestiona el movimiento de la camara al cambiar de nivel o mas bien, mantiene actualizado ese dato
/// </summary>
public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;// respawn positions habitaciones x16
    public List<BoardRoom> rooms = new List<BoardRoom>();//lista BoardRoom ya creados
    
    //private Dictionary<Vector3, bool> ListRoomsCreated = new Dictionary<Vector3, bool>();
    private Dictionary<Vector3, RoomParameters> ListRoomsCreated = new Dictionary<Vector3, RoomParameters>();//list parametros y posiciones habitaciones creadas y por crear

    private int previousRoomDirection;
    private int nextRoomDirection;
    public float moveAmountX;// 18 para el tamaño de las habitaciones estandar
    public float moveAmountY;//

    private float timeBtwRoom;
    public float startTimeBTwRoom = 0.25f;
    private int maxNumberRooms = 16;
    private int currentNumberRooms = 1;
    
    public enum doorDirection { up,down,left,right};
    doorDirection prevDirectionDoor = doorDirection.right;
    doorDirection nextDirectionDoor = doorDirection.right;

    //boundaries map generator
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    private bool stopGeneration = false;
    private bool addRoomsGameManager = false;
    private bool generateRoomTurn = false;

    void Awake()
    {
        int randStartingPos = UnityEngine.Random.Range(0, 3);
        InitDictionaryRooms();
        transform.position = startingPositions[0].position;//ToDo Random
        previousRoomDirection = 3;
        nextRoomDirection = 1;

        //set tipo habitacion
        ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.Main;
        ListRoomsCreated[transform.position].RoomGenerated = true;
        ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(nextRoomDirection), EnumTypeDoor.entrance);
        

        BoardRoom initialBoardRoom = GameManager.instance.boardScript.BoardSetup(transform.position, null, nextDirectionDoor,false);// TODO: arreglar que no inicie en estatico la primera habitacion
        //ListRoomsCreated.Add(transform.position, currentRoomParam);
        rooms.Add(initialBoardRoom);
        UpdateCurrentRoomParameters(ListRoomsCreated);
        GameManager.instance.SetCurrentRoomBoard(initialBoardRoom);

       
    }

    /// <summary>
    /// Crear diccionario con las posiciones y parametros sin iniciar
    /// </summary>
    private void InitDictionaryRooms()
    {
        foreach (var startPosRoom in startingPositions)
        {
            RoomParameters currentRoomParam = new RoomParameters(EnumTypeRoom.none, false);
            ListRoomsCreated.Add(startPosRoom.position, currentRoomParam);
        }
    }
    /// <summary>
    /// Primera vuelta genera el camino principal en Move()
    /// Segunda vuelta crea las habitaciones adiconales InitOptionalRooms()
    /// Finalmente activa los eventos de todas las habitaciones generadas
    /// </summary>
    void Update()
    {
        if (timeBtwRoom <= 0 && stopGeneration == false)
        {
            generateRoomTurn = false;
            Move();
            timeBtwRoom = startTimeBTwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
           
        }
        if (stopGeneration && !addRoomsGameManager)
        {
            InitOptinalRooms();
            GameManager.instance.AddRooms(rooms.ToArray());
            addRoomsGameManager = true;
            //arrancamos el iniciar todas las habitaciones
            GameManager.instance.eventRoomController.InitRoomsDungeonLevel(GameManager.instance.BoardRooms);
        }
    }

    private void InitOptinalRooms()
    {
        foreach (var room in ListRoomsCreated)
        {
            if (room.Value.RoomGenerated == false && room.Value.TypeRoom != EnumTypeRoom.none)
            {
                rooms.Add(GameManager.instance.boardScript.BoardSetup(room.Key, null, null,false));
                transform.position = room.Key;
                room.Value.RoomGenerated = true;
                UpdateCurrentRoomParameters(ListRoomsCreated);
            }
        }
    }

    private void Move()
    {
        int numberTries = 0;
       

        if ((nextRoomDirection == 1 || nextRoomDirection == 2) && generateRoomTurn == false) //move RIGHT
        {
            Vector2 newPos = new Vector2(transform.position.x + moveAmountX, transform.position.y);
            if (transform.position.x < maxX && ListRoomsCreated[newPos].RoomGenerated == false)
            {
                transform.position = newPos;
                bool created = ListRoomsCreated[transform.position].RoomGenerated;

                if (!created)
                {
                    previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection); ;//la inversa al nextDireccion del paso anterior
                    do
                    {
                        if (numberTries > 0)
                        {
                            if (nextRoomDirection == 5)//validar si al bajar la siguiente habitacion, es fin de generacion o no
                            {
                                stopGeneration = true;
                                break;
                            }
                        }
                        nextRoomDirection = UnityEngine.Random.Range(1, 6);// al moverse a la derecha, que no sea posible girar a la izquierda de nuevo
                        if (nextRoomDirection == 3)
                        {
                            nextRoomDirection = 2;
                        }
                        else if (nextRoomDirection == 4)
                        {
                            nextRoomDirection = 5;
                        }
                        else if (numberTries > 0)//  si ya hubo intentos, forzar bajar
                        {
                            nextRoomDirection = 5;
                        }
                        numberTries++;
                    } while (!CheckNextRoomValid(transform, nextRoomDirection,false));

                    //1º creamos los RoomParameters
                    ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.Main;
                    ListRoomsCreated[transform.position].RoomGenerated = true;
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(previousRoomDirection), EnumTypeDoor.entrance);
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(nextRoomDirection), EnumTypeDoor.entrance);
                    //2º se crea la habitacion
                    rooms.Add(GameManager.instance.boardScript.BoardSetup(transform.position,GetDoorDirectionRandom(previousRoomDirection) , GetDoorDirectionRandom(nextRoomDirection),false));

                    //ListRoomsCreated.Add(transform.position,currentRoomParam);
                    
                    //previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection);
                    generateRoomTurn = true;
                }
            }
            else//spawnear habitacion hacia abajo
            {
                nextRoomDirection = 5;
                previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection);
                generateRoomTurn = true;
            }
        }
        else if ((nextRoomDirection == 3 || nextRoomDirection == 4) && generateRoomTurn == false)//move LEFT
        {
            Vector2 newPos = new Vector2(transform.position.x - moveAmountX, transform.position.y);
            if (transform.position.x > minX && ListRoomsCreated[newPos].RoomGenerated == false)
            {
                transform.position = newPos;
                bool created = ListRoomsCreated[transform.position].RoomGenerated;

                if (!created)
                {
                    previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection); ;//la inversa al nextDireccion del paso anterior
                    do
                    {
                        if (numberTries > 0)// solo puede ir izquierda o abajo, si hay intentos, es que izquierda no es posible, forzamos abajo
                        {
                            if (nextRoomDirection == 5)//validar si al bajar la siguiente habitacion, es fin de generacion o no
                            {
                                stopGeneration = true;
                                break;
                            }
                            else
                            {
                                nextRoomDirection = 5;
                            }
                        }
                        else
                        {
                            nextRoomDirection = UnityEngine.Random.Range(3, 6); //dont move right after left
                            numberTries++;
                        }
                    } while (!CheckNextRoomValid(transform, nextRoomDirection,false));

                    //1º creamos los RoomParameters
                    ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.Main;
                    ListRoomsCreated[transform.position].RoomGenerated = true;
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(previousRoomDirection), EnumTypeDoor.entrance);
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(nextRoomDirection), EnumTypeDoor.entrance);
                    //2º se crea la habitacion
                    rooms.Add(GameManager.instance.boardScript.BoardSetup(transform.position, GetDoorDirectionRandom(previousRoomDirection), GetDoorDirectionRandom(nextRoomDirection),false));

                    //ListRoomsCreated.Add(transform.position, currentRoomParam);

                    //previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection); 
                    generateRoomTurn = true;
                }
            }
            else
            {
                nextRoomDirection = 5;
                previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection);
                generateRoomTurn = true;
            }
        }
        else if (nextRoomDirection == 5 && generateRoomTurn == false)//move DOWN
        {
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmountY);
            if (transform.position.y > maxY && ListRoomsCreated[newPos].RoomGenerated == false)
            {
                transform.position = newPos;
                bool created = ListRoomsCreated[transform.position].RoomGenerated;

                if (!created)
                {
                    previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection); ;//la inversa al nextDireccion del paso anterior
                    do
                    {
                        nextRoomDirection = UnityEngine.Random.Range(1, 6);//despues de bajar que vaya donde quiera
                        numberTries++;
                    } while (!CheckNextRoomValid(transform, nextRoomDirection,false));

                    //1º creamos los RoomParameters
                    //RoomParameters currentRoomParam = new RoomParameters(EnumTypeRoom.Main, true);
                    ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.Main;
                    ListRoomsCreated[transform.position].RoomGenerated = true;
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(previousRoomDirection), EnumTypeDoor.entrance);
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(nextRoomDirection), EnumTypeDoor.entrance);
                    //2º se crea la habitacion
                    rooms.Add(GameManager.instance.boardScript.BoardSetup(transform.position, GetDoorDirectionRandom(previousRoomDirection), GetDoorDirectionRandom(nextRoomDirection),false));

                    //ListRoomsCreated.Add(transform.position, currentRoomParam);

                    //previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection);
                    generateRoomTurn = true;
                }
            }
            else
            {
                stopGeneration = true;
            }
        }
        
        var nextSecDoorDirec =  CheckNextSecundaryRoomValid(GetDoorDirectionRandom(nextRoomDirection), GetDoorDirectionRandom((int)GetReversalDoorDirection(previousRoomDirection)));//ToDo:comprobar si este prevDoorDirec esta bien
        if (nextSecDoorDirec != null)
        {
            var newSecRoom = CreateSecondaryRoom((doorDirection)nextSecDoorDirec);

            if (ListRoomsCreated[newSecRoom.Item1].TypeRoom == EnumTypeRoom.none)//no se ha seteado
            {
                ListRoomsCreated[newSecRoom.Item1] = newSecRoom.Item2;
                ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)nextSecDoorDirec, EnumTypeDoor.entrance);//entrada secundaria en habitacion actual
            }
        }

        UpdateCurrentRoomParameters(ListRoomsCreated);

        //prevDirectionDoor = (doorDirection)GetDoorDirectionRandom(previousRoomDirection);
        //nextDirectionDoor = (doorDirection)GetDoorDirectionRandom(nextRoomDirection);
    }
    /// <summary>
    /// Lee los parametros de la habitacion y los aplica en los objetos ya generados
    /// 1º las puertas asociadas al BoardRoom->FRoomDoor
    /// </summary>
    private void UpdateCurrentRoomParameters(Dictionary<Vector3, RoomParameters> listRoomsCreated)
    {
        var currentRoomParameters = listRoomsCreated[transform.position];
        var currentRoom = rooms[rooms.Count - 1];

        currentRoom.UpdateDoorsByParameters(currentRoomParameters.RoomDoors);
        currentRoom.LogCurrentRoom(currentRoomParameters);
        
    }
  

    /// <summary>
    /// Dada la habitacion actual generamos la nueva habitacion secundaria
    /// Se elige el tipo de habitacion
    /// Se configuran las puertas de entrada y resto de parametros
    /// </summary>
    /// <returns>
    /// Vector3,RoomParameters: transform y parametros de la nueva habitacion
    /// </returns>
    private (Vector3,RoomParameters) CreateSecondaryRoom(doorDirection  nextSecDirectionDoor)
    {
        BoardRoom currentRoom = rooms[rooms.Count - 1];
        var invnextSecIntDirectionDoor = GetReversalDoorDirection((int)GetIntByDoorDirection(nextSecDirectionDoor));
        var invNextDirectionDoor = (doorDirection)GetDoorDirection((int)invnextSecIntDirectionDoor);

        Vector3 nextSecRoomPosition = GetNextPositionRoom(currentRoom.transform, nextSecDirectionDoor);

        //aqui decidimos si generamos secundaria o secret
        //ToDo: metodo para ir creando las habitaciones de forma aleatoria, pero asegurando un numero minimo 
        RoomParameters currentRoomParam = new RoomParameters(EnumTypeRoom.Secundary, false);
        currentRoomParam.SetDoorTypeByDirection(invNextDirectionDoor, EnumTypeDoor.entrance);

        return (nextSecRoomPosition, currentRoomParam);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="nextDirectionDoor"></param>
    /// <returns></returns>
    private Vector3 GetNextPositionRoom(Transform transform, doorDirection nextDirectionDoor)
    {
        Vector2 newPos = new Vector3();
        if (nextDirectionDoor == doorDirection.right)
        {
            if (transform.position.x < maxX)
            {
                newPos = new Vector2(transform.position.x + moveAmountX, transform.position.y);
            }
        }
        else if (nextDirectionDoor == doorDirection.left)
        {
            if (transform.position.x > minX)
            {
                newPos = new Vector2(transform.position.x - moveAmountX, transform.position.y);
            }
        }
        else if (nextDirectionDoor == doorDirection.down)
        {
            if (transform.position.y > maxY)
            {
                newPos = new Vector2(transform.position.x, transform.position.y - moveAmountY);
            }
        }
        return newPos;
    }

    /// <summary>
    /// Dada la habitacion actual, comprobamos los bordes y devolvemos la direccion donde crear la habitacion secundaria
    /// </summary>
    /// <param name="mainDirection"></param>
    private doorDirection? CheckNextSecundaryRoomValid(doorDirection? nextMainDirection, doorDirection? prevMainDirection)
    {
        BoardRoom currentRoom = rooms[rooms.Count - 1];

        var IEDirectionDoors = Utilities.EnumUtil.GetValues<doorDirection>();
        List<doorDirection> activeDirections = CheckRoomBoundaries( currentRoom, IEDirectionDoors);
        if (nextMainDirection != null){activeDirections.Remove((doorDirection)nextMainDirection);}
        if (prevMainDirection != null){activeDirections.Remove((doorDirection)prevMainDirection);}

        if (activeDirections.Count > 0)
        {
            return activeDirections[UnityEngine.Random.Range(0, activeDirections.Count)];
        }
        else
        {
            return null;
        }
       
    }
    /// <summary>
    /// Comprobamos las posibles direcciones para crear una habitacion
    /// </summary>
    /// <param name="currentRoom"></param>
    private List<LevelGeneration.doorDirection> CheckRoomBoundaries(BoardRoom currentRoom, IEnumerable<LevelGeneration.doorDirection> IEDirectionDoors)
    {
        List<LevelGeneration.doorDirection> activeDirections = new List<doorDirection>();
        foreach (var direcDoor in IEDirectionDoors)
        {
            if(CheckNextRoomValid(currentRoom.transform, direcDoor, true))
            {
                activeDirections.Add(direcDoor);
            }
        }
        return activeDirections;
    }

    private bool CheckNextRoomValid( Transform originalPositionRoom, int nextDirecionRoom, bool checkSecondary)
    {
        Vector2 nextPositionRoom;
        if (nextDirecionRoom == 1 || nextDirecionRoom == 2) //move RIGHT
        {
            nextPositionRoom = new Vector2(transform.position.x + moveAmountX, transform.position.y);
            if (nextPositionRoom.x <= maxX) { return true; }
        }
        else if (nextDirecionRoom == 3 || nextDirecionRoom == 4)//move LEFT
        {
            nextPositionRoom = new Vector2(transform.position.x - moveAmountX, transform.position.y);
            if (nextPositionRoom.x >= minX) { return true; }
        }
        else if (nextDirecionRoom == 5)//move DOWN
        {
            nextPositionRoom = new Vector2(transform.position.x, transform.position.y - moveAmountY);
            if (nextPositionRoom.y >= maxY) { return true; }
            else if (!checkSecondary)
            {
                stopGeneration = true;
            }
        }
       
        return false;
    }

    private bool CheckNextRoomValid(Transform originalPositionRoom, LevelGeneration.doorDirection nextDirecionRoom, bool checkSecondary)
    {
        Vector2 nextPositionRoom;
        if (nextDirecionRoom == LevelGeneration.doorDirection.right) //move RIGHT
        {
            nextPositionRoom = new Vector2(transform.position.x + moveAmountX, transform.position.y);
            if (nextPositionRoom.x <= maxX) { return true; }
        }
        else if (nextDirecionRoom == LevelGeneration.doorDirection.left)//move LEFT
        {
            nextPositionRoom = new Vector2(transform.position.x - moveAmountX, transform.position.y);
            if (nextPositionRoom.x >= minX) { return true; }
        }
        else if (nextDirecionRoom == LevelGeneration.doorDirection.down)//move DOWN
        {
            nextPositionRoom = new Vector2(transform.position.x, transform.position.y - moveAmountY);
            if (nextPositionRoom.y >= maxY) { return true; }
            else if (!checkSecondary)
            {
                stopGeneration = true;
            }
            else if (nextDirecionRoom == LevelGeneration.doorDirection.up)//move UP
            {
                nextPositionRoom = new Vector2(transform.position.x, transform.position.y + moveAmountY);
                if (nextPositionRoom.y <= minY) { return true; }
            }
        }
        return false;
    }

    private doorDirection? GetDoorDirectionRandom(int randomNumber)
    {
        if (randomNumber == 1 || randomNumber == 2)
        {
            return doorDirection.right;
        }
        else if (randomNumber == 3 || randomNumber == 4)
        {
            return doorDirection.left;
        }
        else if (randomNumber == 5 )
        {
            return doorDirection.down;
        }
        else if(randomNumber == 6)
        {
            return doorDirection.up;
        }
        return null;
    }

    private doorDirection? GetDoorDirection(int numberDoorDirecion)
    {
        if (numberDoorDirecion == 1 || numberDoorDirecion == 2)
        {
            return doorDirection.right;
        }
        else if (numberDoorDirecion == 3 || numberDoorDirecion == 4)
        {
            return doorDirection.left;
        }
        else if (numberDoorDirecion == 5)
        {
            return doorDirection.down;
        }
        else if (numberDoorDirecion == 6)
        {
            return doorDirection.up;
        }
        return null;
    }

    private int? GetIntByDoorDirection(doorDirection numberDoorDirecion)
    {
        if (numberDoorDirecion == doorDirection.right)
        {
            return 1 ;
        }
        else if (numberDoorDirecion == doorDirection.left)
        {
            return 3;
        }
        else if (numberDoorDirecion == doorDirection.down)
        {
            return 5;
        }
        else if (numberDoorDirecion == doorDirection.up)
        {
            return 6;
        }
        return null;
    }

    private int? GetReversalDoorDirection(int randomNumber)
    {
        if (randomNumber == 1 || randomNumber == 2)
        {
            return 3;
        }
        else if (randomNumber == 3 || randomNumber == 4)
        {
            return 1;
        }
        else if (randomNumber == 5)
        {
            return 6;//up
        }
        else if (randomNumber == 6)
        {
            return 5;
        }

        return null;
    }

    //public doorDirection GetNextDoorDirection()
    //{
    //    return prevDirectionDoor;//en este momento la direcion que nos interesa esta guarda en PREV
    //}
}
