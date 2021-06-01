﻿using Assets.Scripts.EnumTypes;
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
            if (room.Value.RoomGenerated == false)
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
            if (transform.position.x < maxX)
            {
                Vector2 newPos = new Vector2(transform.position.x + moveAmountX, transform.position.y);
                transform.position = newPos;
                bool created = ListRoomsCreated[transform.position].RoomGenerated;


                if (!created)
                {
                    do
                    {
                        nextRoomDirection = UnityEngine.Random.Range(1, 6);// al moverse a la derecha, que no sea posible girar a la izquierda de nuevo
                        if (nextRoomDirection == 3)
                        {
                            nextRoomDirection = 2;
                        }
                        else if (nextRoomDirection == 4)
                        {
                            nextRoomDirection = 5;
                        }
                        numberTries++;
                    } while (!CheckNextRoomValid(transform, nextRoomDirection,false) && (numberTries < 5));

                    //1º creamos los RoomParameters
                    ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.Main;
                    ListRoomsCreated[transform.position].RoomGenerated = true;
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(previousRoomDirection), EnumTypeDoor.entrance);
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(nextRoomDirection), EnumTypeDoor.entrance);
                    //2º se crea la habitacion
                    rooms.Add(GameManager.instance.boardScript.BoardSetup(transform.position,GetDoorDirectionRandom(previousRoomDirection) , GetDoorDirectionRandom(nextRoomDirection),false));

                    //ListRoomsCreated.Add(transform.position,currentRoomParam);
                    
                    previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection);
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
            if (transform.position.x > minX)
            {
                Vector2 newPos = new Vector2(transform.position.x - moveAmountX, transform.position.y);
                transform.position = newPos;
                bool created = ListRoomsCreated[transform.position].RoomGenerated;

                if (!created)
                {
                    do
                    {
                        nextRoomDirection = UnityEngine.Random.Range(3, 6); //dont move right after left
                        numberTries++;
                    } while (!CheckNextRoomValid(transform, nextRoomDirection,false) && (numberTries < 5));

                    //1º creamos los RoomParameters
                    ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.Main;
                    ListRoomsCreated[transform.position].RoomGenerated = true;
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(previousRoomDirection), EnumTypeDoor.entrance);
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(nextRoomDirection), EnumTypeDoor.entrance);
                    //2º se crea la habitacion
                    rooms.Add(GameManager.instance.boardScript.BoardSetup(transform.position, GetDoorDirectionRandom(previousRoomDirection), GetDoorDirectionRandom(nextRoomDirection),false));

                    //ListRoomsCreated.Add(transform.position, currentRoomParam);

                    previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection); 
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
            if (transform.position.y > maxY)
            {
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmountY);
                transform.position = newPos;
                bool created = ListRoomsCreated[transform.position].RoomGenerated;

                if (!created)
                {
                    do
                    {
                        nextRoomDirection = UnityEngine.Random.Range(1, 6);//despues de bajar que vaya donde quiera
                        numberTries++;
                    } while (!CheckNextRoomValid(transform, nextRoomDirection,false) && (numberTries < 5));

                    //1º creamos los RoomParameters
                    //RoomParameters currentRoomParam = new RoomParameters(EnumTypeRoom.Main, true);
                    ListRoomsCreated[transform.position].TypeRoom = EnumTypeRoom.Main;
                    ListRoomsCreated[transform.position].RoomGenerated = true;
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(previousRoomDirection), EnumTypeDoor.entrance);
                    ListRoomsCreated[transform.position].SetDoorTypeByDirection((doorDirection)GetDoorDirection(nextRoomDirection), EnumTypeDoor.entrance);
                    //2º se crea la habitacion
                    rooms.Add(GameManager.instance.boardScript.BoardSetup(transform.position, GetDoorDirectionRandom(previousRoomDirection), GetDoorDirectionRandom(nextRoomDirection),false));

                    //ListRoomsCreated.Add(transform.position, currentRoomParam);

                    previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection);
                    generateRoomTurn = true;
                }
            }
            else
            {
                stopGeneration = true;
            }
        }
        
        var nextSecDoorDirec =  CheckNextSecundaryRoomValid(nextDirectionDoor, GetDoorDirection(previousRoomDirection));//ToDo:comprobar si este prevDoorDirec esta bien
        if (nextSecDoorDirec != null)
        {
            var newSecRoom = CreateSecondaryRoom((doorDirection)nextSecDoorDirec);
            if ((ListRoomsCreated[newSecRoom.Item1].RoomGenerated == true && ListRoomsCreated[newSecRoom.Item1].TypeRoom != EnumTypeRoom.Main)&&
                (ListRoomsCreated[newSecRoom.Item1].RoomGenerated == false && ListRoomsCreated[newSecRoom.Item1].TypeRoom != EnumTypeRoom.Secundary))
            {
                ListRoomsCreated[newSecRoom.Item1] = newSecRoom.Item2;
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
        
    }

    /// <summary>
    /// Dada la habitacion actual generamos la nueva habitacion secundaria
    /// Se elige el tipo de habitacion
    /// Se configuran las puertas de entrada y resto de parametros
    /// </summary>
    /// <returns>
    /// Vector3,RoomParameters: transform y parametros de la nueva habitacion
    /// </returns>
    private (Vector3,RoomParameters) CreateSecondaryRoom(doorDirection nextSecDirectionDoor)
    {
        BoardRoom currentRoom = rooms[rooms.Count - 1];

        Vector3 nextSecRoomPosition = GetNextPositionRoom(currentRoom.transform, nextSecDirectionDoor);

        //aqui decidimos si generamos secundaria o secret
        //ToDo: metodo para ir creando las habitaciones de forma aleatoria, pero asegurando un numero minimo 
        RoomParameters currentRoomParam = new RoomParameters(EnumTypeRoom.Secundary, false);
        currentRoomParam.SetDoorTypeByDirection(nextSecDirectionDoor, EnumTypeDoor.entrance);

        return (nextSecRoomPosition, currentRoomParam);
    }
    ///
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
                if (nextPositionRoom.y <= maxY) { return true; }
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

        return null;
    }

    //public doorDirection GetNextDoorDirection()
    //{
    //    return prevDirectionDoor;//en este momento la direcion que nos interesa esta guarda en PREV
    //}
}
