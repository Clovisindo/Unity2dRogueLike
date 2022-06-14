﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts;
using Random = UnityEngine.Random;
using System.Reflection;
using Assets.Scripts.LevelDesign;

public class EventRoomController : MonoBehaviour
{
    public static EventRoomController instance = null;
    [SerializeField] private GameObject[] InitPositionsEnemy;
    private List<Transform> currentInitPositionsEnemy;
    [SerializeField] private GameObject[] InitPositionsPuzzle;

    [SerializeField]private Enemy[] enemiesPrefab;
    private List<Enemy> enemies = new List<Enemy>();
    [SerializeField]private GameObject[] puzzles;
    public enum TypesRoom { empty, battle, puzzzle };

    public TypesRoom currentTypeRoom = TypesRoom.empty;
    public BoardRoom currentRoom;

    private List<DesignLevelParameters> listLevelParameters;

    //[SerializeField] private int quantityWeakEnemies;
    //[SerializeField] private int quantityMidEnemies;
    //[SerializeField] private int quantityStrongEnemies;
    public Dictionary<string, int> TypeQtyEnemies = new Dictionary<string, int>();


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        //currentRoom = GameManager.instance.currentRoom;
        //InitRoom();//ToDo: DEBUG
    }

    public void InitRoomsDungeonLevel(BoardRoom[] listRooms)
    {
        listLevelParameters = GameManager.instance.levelParametersManager.SetDesignParametersRooms(listRooms);

        foreach (var room in listRooms.Select((value, i) => (value, i)))
        {
            InitRoom(room);
            room.value.PauseRoom();
        }


        // asignar la primera habitacion como inicial
        currentRoom = listRooms[0];
        currentRoom.ReStartRoom();
    }

    /// <summary>
    /// Se inicializa una habitacion
    /// </summary>
    private void InitRoom((BoardRoom value, int i) currentRoom)
    {
        currentInitPositionsEnemy = Utilities.getAllChildsObject<Transform>(InitPositionsEnemy[0].transform);//ToDo: elegir de forma aleatoria
        SetTypeCurrentRoom(currentRoom.value);

        switch (currentTypeRoom)
        {
            case TypesRoom.empty:
                SetEmptyRoom();
                break;
            case TypesRoom.battle:
                //set posiciones enemigos para el nivel
                SpawnEnemiesRoom(currentRoom);
                break;
            case TypesRoom.puzzzle:
                //set posiciones objetos puzzle/diseño
                SetSpawnPuzzle();
                break;
        }
    }
    private void SetTypeCurrentRoom(BoardRoom currentRoom)
    {
        currentTypeRoom = (TypesRoom)currentRoom.RoomParameters.TypeRoom;//ToDo: no es el mismo tipo
    }

    private void SetEmptyRoom()
    {
        //throw new NotImplementedException();
    }

    private void SetSpawnPuzzle()
    {
        //throw new NotImplementedException();
    }

    private void SpawnEnemiesRoom((BoardRoom value, int i) currentRoom)
    {
        //foreach (var enemyParameter in listLevelParameters[currentRoom.i].GetEnemies())
        //{
        //    enemies.Add(enemyParameter);
        //}
        currentRoom.value.InvokeEnemies(currentInitPositionsEnemy, listLevelParameters[currentRoom.i].GetEnemies());
    }

    //pausar la partida
    public void PauseRoom(BoardRoom _boardRoom)
    {
        _boardRoom.PauseRoom();
    }
    //reiniciar la instancia de la habitacion
    public void ReStartRoom(BoardRoom _boardRoom)
    {
        _boardRoom.ReStartRoom();
    }
    
}
