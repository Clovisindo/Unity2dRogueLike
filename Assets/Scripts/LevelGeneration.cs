using Assets.Scripts.EnumTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Esta clase es el recuadro que va moviendose y generando cada fase al empezar
/// Ademas gestiona el movimiento de la camara al cambiar de nivel o mas bien, mantiene actualizado ese dato
/// </summary>
public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;// respawn positions habitaciones x16
    public List<BoardRoom> rooms = new List<BoardRoom>();//lista BoardRoom ya creados
    private Dictionary<Vector3, RoomParameters> ListRoomsCreated = new Dictionary<Vector3, RoomParameters>();//list parametros y posiciones habitaciones creadas y por crear
    private List<LevelParameters> currentLevelParameters = new List<LevelParameters>();
    EnumTypeRoom currentTypeRoom = EnumTypeRoom.none;

    private int previousRoomDirection;
    private int nextRoomDirection;
    public float moveAmountX;// 18 para el tamaño de las habitaciones estandar
    public float moveAmountY;//

    private float timeBtwRoom;
    public float startTimeBTwRoom = 0.25f;
    
    public enum doorDirection { up,down,left,right};
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

        //ToDo:Carga dinamica
        currentLevelParameters.Add(new LevelParameters(EnumTypeRoom.secundary, false));
        currentLevelParameters.Add(new LevelParameters(EnumTypeRoom.secret, false));
        currentLevelParameters.Add(new LevelParameters(EnumTypeRoom.secundary, false));


        //set tipo habitacion
        ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.main;
        ListRoomsCreated[transform.position].RoomGenerated = true;
        ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirectionByInt(nextRoomDirection), EnumTypeDoor.entrance);


        BoardRoom initialBoardRoom = GameManager.instance.boardScript.BoardSetup(transform.position, null, nextDirectionDoor);// TODO: arreglar que no inicie en estatico la primera habitacion
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

    internal static void ActivateVictoryScene()
    {
        GameManager.instance.VictoryEndGame();
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
    /// <summary>
    /// Instancia las habitaciones de caminos secundarios
    /// </summary>
    private void InitOptinalRooms()
    {
        foreach (var room in ListRoomsCreated)
        {
            if (room.Value.RoomGenerated == false && room.Value.TypeRoom != EnumTypeRoom.none)
            {
                rooms.Add(GameManager.instance.boardScript.BoardSetup(room.Key, null, null));
                transform.position = room.Key;
                room.Value.RoomGenerated = true;
                UpdateCurrentRoomParameters(ListRoomsCreated);
            }
        }
    }
    /// <summary>
    /// Instancia las habitaciones del camino principal
    /// </summary>
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
                    ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.main;
                    ListRoomsCreated[transform.position].RoomGenerated = true;
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirectionByInt(previousRoomDirection), EnumTypeDoor.entrance);
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirectionByInt(nextRoomDirection), EnumTypeDoor.entrance);
                    //2º se crea la habitacion
                    rooms.Add(GameManager.instance.boardScript.BoardSetup(transform.position,GetDoorDirectionByInt(previousRoomDirection) , GetDoorDirectionByInt(nextRoomDirection)));

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
                    ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.main;
                    ListRoomsCreated[transform.position].RoomGenerated = true;
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirectionByInt(previousRoomDirection), EnumTypeDoor.entrance);
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirectionByInt(nextRoomDirection), EnumTypeDoor.entrance);
                    //2º se crea la habitacion
                    rooms.Add(GameManager.instance.boardScript.BoardSetup(transform.position, GetDoorDirectionByInt(previousRoomDirection), GetDoorDirectionByInt(nextRoomDirection)));

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
                    ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.main;
                    ListRoomsCreated[transform.position].RoomGenerated = true;
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirectionByInt(previousRoomDirection), EnumTypeDoor.entrance);
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirectionByInt(nextRoomDirection), EnumTypeDoor.entrance);
                    //2º se crea la habitacion
                    rooms.Add(GameManager.instance.boardScript.BoardSetup(transform.position, GetDoorDirectionByInt(previousRoomDirection), GetDoorDirectionByInt(nextRoomDirection)));

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
        
        var nextSecDoorDirec =  CheckNextSecundaryRoomValid(GetDoorDirectionByInt(nextRoomDirection), GetDoorDirectionByInt((int)previousRoomDirection));//ToDo:comprobar si este prevDoorDirec esta bien
        if (nextSecDoorDirec != null)
        {
            CreateNewNextRoom((doorDirection)nextSecDoorDirec);
        }
      
        UpdateCurrentRoomParameters(ListRoomsCreated);
    }

    /// <summary>
    /// Comprobamos que se alternen los tipos de habitacion
    /// Consulta del tipo disponible y asignacion de parametros para instanciar
    /// </summary>
    /// <param name="nextSecDoorDirec"></param>
    private void CreateNewNextRoom(doorDirection nextSecDoorDirec)
    {
        var nextTypeRoom = currentLevelParameters.Where(tr =>  tr.RoomGenerated == false).FirstOrDefault();
        if (nextTypeRoom != null)
        {
            InstantiateNewNextSRoom((doorDirection)nextSecDoorDirec, nextTypeRoom.TypeRoom);
        }
    }
    /// <summary>
    /// Se crea la nueva instancia de habitacion adicional a camino principal, segun el tipo asignado
    /// </summary>
    /// <param name="nextSecDoorDirec"></param>
    private void InstantiateNewNextSRoom(doorDirection nextSecDoorDirec, EnumTypeRoom nextTypeRoom)
    {
        (Vector3, RoomParameters) newSecRoom;

        if (nextTypeRoom == EnumTypeRoom.secundary)
        {
            newSecRoom = CreateSecondaryRoom((doorDirection)nextSecDoorDirec);
            AddNewSecRoomCreated(newSecRoom, (doorDirection)nextSecDoorDirec, nextTypeRoom);

        }
        else if (nextTypeRoom == EnumTypeRoom.secret)
        {
            newSecRoom = CreateSecretRoom((doorDirection)nextSecDoorDirec);
            AddNewSecRoomCreated(newSecRoom, (doorDirection)nextSecDoorDirec, nextTypeRoom);
        }
    }
    /// <summary>
    /// Añadimos a la lista de habitaciones a crear la nueva instancia parametrizada
    /// </summary>
    /// <param name="newSecRoom"></param>
    /// <param name="nextSecDoorDirec"></param>
    private void AddNewSecRoomCreated((Vector3, RoomParameters) newSecRoom, doorDirection nextSecDoorDirec, EnumTypeRoom nextTypeRoom)
    {
        if (ListRoomsCreated[newSecRoom.Item1].TypeRoom == EnumTypeRoom.none)//no se ha seteado
        {
            ListRoomsCreated[newSecRoom.Item1] = newSecRoom.Item2;
            currentTypeRoom = nextTypeRoom;
            ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)nextSecDoorDirec, GetTypeDoorByTypeRoom(currentTypeRoom));//entrada secundaria en habitacion actual
            currentLevelParameters.Where(tr => tr.TypeRoom == currentTypeRoom && tr.RoomGenerated == false).FirstOrDefault().RoomGenerated = true;
        }
    }
    /// <summary>
    /// Obtiene el tipo de puerta segun tipo de habitacion
    /// </summary>
    /// <param name="typeRoom"></param>
    /// <returns></returns>
    private EnumTypeDoor GetTypeDoorByTypeRoom(EnumTypeRoom typeRoom)
    {
        EnumTypeDoor rTypeDoor = EnumTypeDoor.none;
        switch (typeRoom)
        {
            case EnumTypeRoom.main:
                rTypeDoor = EnumTypeDoor.entrance;
                break;
            case EnumTypeRoom.secundary:
                rTypeDoor = EnumTypeDoor.entrance;
                break;
            case EnumTypeRoom.secret:
                rTypeDoor = EnumTypeDoor.secret;
                break;
        }
        return rTypeDoor;
    }

    /// <summary>
    /// Lee los parametros de la habitacion y los aplica en los objetos ya generados
    /// 1º las puertas asociadas al BoardRoom->FRoomDoor
    /// </summary>
    private void UpdateCurrentRoomParameters(Dictionary<Vector3, RoomParameters> listRoomsCreated)
    {
        var currentRoomParameters = listRoomsCreated[transform.position];
        var currentRoom = rooms[rooms.Count - 1];

        currentRoom.RoomParameters = currentRoomParameters;
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
        var invNextDirectionDoor = (doorDirection)GetDoorDirectionByInt((int)invnextSecIntDirectionDoor);

        Vector3 nextSecRoomPosition = GetNextPositionRoom(currentRoom.transform, nextSecDirectionDoor);

        RoomParameters currentRoomParam = new RoomParameters(EnumTypeRoom.secundary, false);
        currentRoomParam.SetDoorTypeByDirection(invNextDirectionDoor, EnumTypeDoor.entrance);

        return (nextSecRoomPosition, currentRoomParam);
    }

    private (Vector3, RoomParameters) CreateSecretRoom(doorDirection nextSecretDirectionDoor)
    {
        BoardRoom currentRoom = rooms[rooms.Count - 1];
        var invnextSecretIntDirectionDoor = GetReversalDoorDirection((int)GetIntByDoorDirection(nextSecretDirectionDoor));
        var invNextDirectionDoor = (doorDirection)GetDoorDirectionByInt((int)invnextSecretIntDirectionDoor);

        Vector3 nextSecretRoomPosition = GetNextPositionRoom(currentRoom.transform, nextSecretDirectionDoor);

        RoomParameters currentRoomParam = new RoomParameters(EnumTypeRoom.secret, false);
        currentRoomParam.SetDoorTypeByDirection(invNextDirectionDoor, EnumTypeDoor.secret);

        return (nextSecretRoomPosition, currentRoomParam);
    }
    /// <summary>
    /// Calcula la posicion de la siguiente habitacion segun la direccion de la puerta
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
    /// Comprueba que la siguiente direccion es aplicable a una habitacion valida
    /// </summary>
    /// <param name="nextActiveDirection"></param>
    /// <returns></returns>
    private bool CheckNextRoomValid(doorDirection nextActiveDirection)
    {
        BoardRoom currentRoom = rooms[rooms.Count - 1];

        return CheckNextActiveDirection(nextActiveDirection);
      
    }

    /// <summary>
    /// Dada la habitacion actual, comprobamos los bordes y devolvemos la direccion donde crear la habitacion secundaria
    /// </summary>
    /// <param name="mainDirection"></param>
    private doorDirection? CheckNextSecundaryRoomValid(doorDirection? nextMainDirection, doorDirection? prevMainDirection)
    {
        if (currentLevelParameters.Where(p => p.RoomGenerated == false).Count() != 0)
        {
            BoardRoom currentRoom = rooms[rooms.Count - 1];

            var IEDirectionDoors = Utilities.EnumUtil.GetValues<doorDirection>();
            List<doorDirection> activeDirections = CheckRoomBoundaries(currentRoom, IEDirectionDoors);
            if (nextMainDirection != null) { activeDirections.Remove((doorDirection)nextMainDirection); }
            if (prevMainDirection != null) { activeDirections.Remove((doorDirection)prevMainDirection); }

            if (activeDirections.Count > 0)
            {
                bool validRoom = false;
                doorDirection nextActiveDirection;
                do
                {
                    nextActiveDirection = activeDirections[UnityEngine.Random.Range(0, activeDirections.Count)];
                    validRoom = CheckNextActiveDirection(nextActiveDirection);
                    if (!validRoom)
                    {
                        activeDirections.Remove((doorDirection)nextActiveDirection);//si no es valida la borramos de la lista
                        if (activeDirections.Count() == 0)
                        {
                            return null;//si no hay opciones, salimos
                        }
                    }
                } while (!validRoom);

                return nextActiveDirection;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// Comprueba si la habitacion en la siguiente direccion ya ha sido creada
    /// </summary>
    /// <param name="nextActiveDirection"></param>
    /// <returns></returns>
    private bool CheckNextActiveDirection(doorDirection nextActiveDirection)
    {
        BoardRoom currentRoom = rooms[rooms.Count - 1];
        Vector3 nextActivePosition = GetNextActivePosition(currentRoom.transform, nextActiveDirection);

        if (ListRoomsCreated[nextActivePosition].TypeRoom == EnumTypeRoom.none){return true;}
        else{return false;}
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
            if (nextPositionRoom.x <= maxX)
            { 
                if(CheckNextRoomValid((doorDirection)GetDoorDirectionByInt(nextDirecionRoom))){ return true; }
                else{return false;}
            }
        }
        else if (nextDirecionRoom == 3 || nextDirecionRoom == 4)//move LEFT
        {
            nextPositionRoom = new Vector2(transform.position.x - moveAmountX, transform.position.y);
            if (nextPositionRoom.x >= minX) {
                if (CheckNextRoomValid((doorDirection)GetDoorDirectionByInt(nextDirecionRoom))) { return true; }
                else { return false; }
            }
        }
        else if (nextDirecionRoom == 5)//move DOWN
        {
            nextPositionRoom = new Vector2(transform.position.x, transform.position.y - moveAmountY);
            if (nextPositionRoom.y >= maxY) {
                if (CheckNextRoomValid((doorDirection)GetDoorDirectionByInt(nextDirecionRoom))) { return true; }
                else { return false; }
            }
            else if (!checkSecondary)
            {
                stopGeneration = true;
            }
        }
       
        return false;
    }
    /// <summary>
    /// A partir de una posicion, devuelve la nextActivePosition aplicando la direcion de salida
    /// </summary>
    /// <param name="originalPositionRoom"></param>
    /// <param name="nextDirecionRoom"></param>
    /// <returns></returns>
    private Vector3 GetNextActivePosition(Transform originalPositionRoom, doorDirection nextDirecionRoom)
    {
        Vector2 nextActivePositionRoom = new Vector3();
        if (nextDirecionRoom == LevelGeneration.doorDirection.right)
        {
            nextActivePositionRoom = new Vector2(transform.position.x + moveAmountX, transform.position.y);
        }
        else if (nextDirecionRoom == LevelGeneration.doorDirection.left)
        {
            nextActivePositionRoom = new Vector2(transform.position.x - moveAmountX, transform.position.y);
        }
        else if (nextDirecionRoom == LevelGeneration.doorDirection.down)
        {
            nextActivePositionRoom = new Vector2(transform.position.x, transform.position.y - moveAmountY);
        }
        else if (nextDirecionRoom == LevelGeneration.doorDirection.up)
        {
            nextActivePositionRoom = new Vector2(transform.position.x, transform.position.y + moveAmountY);
        }

        return nextActivePositionRoom;
    }
    /// <summary>
    /// Comprueba que la siguiente habitacion no se sale de los bordes de la fase
    /// </summary>
    /// <param name="originalPositionRoom"></param>
    /// <param name="nextDirecionRoom"></param>
    /// <param name="checkSecondary"></param>
    /// <returns></returns>
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
    /// <summary>
    /// Dado un int de direccion, devuelve DoorDirecion
    /// </summary>
    /// <param name="randomNumber"></param>
    /// <returns></returns>
    private doorDirection? GetDoorDirectionByInt(int randomNumber)
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
   /// <summary>
   /// Obtenemos el int de direccion de un DoorDirection
   /// </summary>
   /// <param name="numberDoorDirecion"></param>
   /// <returns></returns>
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
    /// <summary>
    /// Obtenemos el int inverso de direccion
    /// </summary>
    /// <param name="randomNumber"></param>
    /// <returns></returns>
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
}
