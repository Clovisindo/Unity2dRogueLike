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
    public GameObject[] rooms;//

    private Dictionary<Vector3, bool> ListRoomsCreated = new Dictionary<Vector3, bool>();

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
    private bool generateRoomTurn = false;

    void Awake()
    {
        int randStartingPos = Random.Range(0, 3);
        transform.position = startingPositions[0].position;//ToDo Random
        GameManager.instance.boardScript.BoardSetup(transform.position, null, nextDirectionDoor);// TODO: arreglar que no inicie en estatico la primera habitacion

        previousRoomDirection = 3;
        nextRoomDirection = 1;

        //creamos la habitacion en cada respawn
        //foreach (var roomStartPosition in startingPositions)
        //{
        //    GameManager.instance.boardScript.BoardSetup(roomStartPosition.position, doorDirection.right);//
        //}

    }

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

                if (!ListRoomsCreated.ContainsKey(transform.position))
                {
                    
                    do
                    {
                        nextRoomDirection = Random.Range(1, 6);// al moverse a la derecha, que no sea posible girar a la izquierda de nuevo
                        if (nextRoomDirection == 3)
                        {
                            nextRoomDirection = 2;
                        }
                        else if (nextRoomDirection == 4)
                        {
                            nextRoomDirection = 5;
                        }
                        numberTries++;
                    } while (!CheckNextRoomValid(transform, nextRoomDirection) && (numberTries < 5));

                    GameManager.instance.boardScript.BoardSetup(transform.position,GetDoorDirectionRandom(previousRoomDirection) , GetDoorDirectionRandom(nextRoomDirection));
                    ListRoomsCreated.Add(transform.position, true);
                    previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection);
                    generateRoomTurn = true;
                }
            }
            else//spawnear habitacion hacia abajo
            {
                nextRoomDirection = 5;
                previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection);
                generateRoomTurn = true;
                //if (transform.position.y > maxY)
                //{
                //    direction = 5;

                //    Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmountY);
                //    transform.position = newPos;

                //    if (!ListRoomsCreated.ContainsKey(transform.position))
                //    {
                //        GameManager.instance.boardScript.BoardSetup(transform.position, GetDoorDirectionRandom(direction));
                //        ListRoomsCreated.Add(transform.position, true);
                //    }

                //    direction = Random.Range(1, 6);
                //}
                //else
                //{
                //    stopGeneration = true;
                //}
            }
            
        }
        else if ((nextRoomDirection == 3 || nextRoomDirection == 4) && generateRoomTurn == false)//move LEFT
        {
            if (transform.position.x > minX)
            {
                Vector2 newPos = new Vector2(transform.position.x - moveAmountX, transform.position.y);
                transform.position = newPos;

                if (!ListRoomsCreated.ContainsKey(transform.position))
                {
                    do
                    {
                        nextRoomDirection = Random.Range(3, 6); //dont move right after left
                        numberTries++;
                    } while (!CheckNextRoomValid(transform, nextRoomDirection) && (numberTries < 5));

                    GameManager.instance.boardScript.BoardSetup(transform.position, GetDoorDirectionRandom(previousRoomDirection), GetDoorDirectionRandom(nextRoomDirection));
                    ListRoomsCreated.Add(transform.position, true);
                    previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection); 
                    generateRoomTurn = true;
                }
            }
            else
            {
                nextRoomDirection = 5;
                previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection);
                generateRoomTurn = true;
                //if (transform.position.y > maxY)
                //{
                //    direction = 5;
                //    Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmountY);
                //    transform.position = newPos;

                //    if (!ListRoomsCreated.ContainsKey(transform.position))
                //    {
                //        GameManager.instance.boardScript.BoardSetup(transform.position, GetDoorDirectionRandom(direction));
                //        ListRoomsCreated.Add(transform.position, true);
                //    }

                //    direction = Random.Range(1, 6);
                //}
                //else
                //{
                //    stopGeneration = true;
                //}

            }
        }
        else if (nextRoomDirection == 5 && generateRoomTurn == false)//move DOWN
        {
            if (transform.position.y > maxY)
            {
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmountY);
                transform.position = newPos;

                if (!ListRoomsCreated.ContainsKey(transform.position))
                {
                    do
                    {
                        nextRoomDirection = Random.Range(1, 6);//despues de bajar que vaya donde quiera
                        numberTries++;
                    } while (!CheckNextRoomValid(transform, nextRoomDirection) && (numberTries < 5));

                    GameManager.instance.boardScript.BoardSetup(transform.position, GetDoorDirectionRandom(previousRoomDirection), GetDoorDirectionRandom(nextRoomDirection));
                    ListRoomsCreated.Add(transform.position, true);
                    previousRoomDirection = (int)GetReversalDoorDirection(nextRoomDirection);
                    generateRoomTurn = true;
                }
            }
            else
            {
                stopGeneration = true;
            }
        }
    }

    private void UpdatemoveDirectionDoorCamera( doorDirection direction)
    {
        //switch (direction == doorDirection.left)
        //{
        //    move
        //    default:
        //        break;
        //}
    }

    private bool CheckNextRoomValid( Transform originalPositionRoom, int nextDirecionRoom)
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
            else
            {
                stopGeneration = true;
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
}
