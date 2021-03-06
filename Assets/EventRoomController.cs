using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts;
using Random = UnityEngine.Random;
using System.Reflection;

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
        DontDestroyOnLoad(gameObject);
        currentRoom = GameManager.instance.currentRoom;

       
        InitRoom();//ToDo: DEBUG
    }

    /// <summary>
    /// Se inicializa una habitacion
    /// </summary>
    public void InitRoom()
    {
        currentRoom = GameManager.instance.currentRoom;
        currentInitPositionsEnemy = Utilities.getAllChildsObject<Transform>(InitPositionsEnemy[0].transform);//ToDo: elegir de forma aleatoria
        SetTypeCurrentRoom();//ToDo:

        switch (currentTypeRoom)
        {
            case TypesRoom.empty:
                SetEmptyRoom();
                break;
            case TypesRoom.battle:
                //set posiciones enemigos para el nivel
                SetSpawnEnemies();
                break;
            case TypesRoom.puzzzle:
                //set posiciones objetos puzzle/diseño
                SetSpawnPuzzle();
                break;
        }
    }
    private void SetTypeCurrentRoom()
    {
        currentTypeRoom = TypesRoom.battle;
    }

    private void SetEmptyRoom()
    {
        throw new NotImplementedException();
    }

    private void SetSpawnPuzzle()
    {
        throw new NotImplementedException();
    }

    private void SetSpawnEnemies()
    {
        //var typesMonster = Assembly.GetAssembly(typeof(EnumTypeEnemies)).GetTypes().Where(currentType => currentType.IsSubclassOf(typeof(EnumTypeEnemies)));

        //asignar cantidad de enemigos
        SetQtyEnemies();

        //recorrer bucle por tipo de enemigo
        foreach (var typeEnemy in Enum.GetNames(typeof(EnumTypeEnemies)))
        {
            if (typeEnemy != "none")
            {
                //el metodo será general pasandole la cantidad de enemigos a generar y el tipo de enemigos
                SpawnTypeEnemies(typeEnemy);
            }
           
        }
        //primero elegir la cantidad de enemigos en la formula y luego buscar el array de posiciones que coincida con esa cantidad y sus variantes
        currentRoom.InvokeEnemies(currentInitPositionsEnemy, enemies);//ToDo: gestionar cantidad de enemigos y el array de posiciones previamente e incluso el tipo de enemigos
    }

    private void SetQtyEnemies()//ToDo: hacer dinamico
    {
        TypeQtyEnemies.Add(EnumTypeEnemies.weak.ToString(), 2);
        TypeQtyEnemies.Add(EnumTypeEnemies.mid.ToString(), 2);
        TypeQtyEnemies.Add(EnumTypeEnemies.strong.ToString(), 1);
    }

    private void SpawnTypeEnemies(string _typeEnemy)
    {
        int _quantityTypeEnem = TypeQtyEnemies[_typeEnemy];//asignamos la cantidad segun el tipo de monstruo

        var listCurrentTypeEnemies = enemiesPrefab.Where(e => e.TypeEnemy.ToString() == _typeEnemy.ToString()).ToList();
        //Invocamos enemigos de tipo determinado de la lista, hasta cumplir la cantidad del nivel
        if (listCurrentTypeEnemies.Count > 0)
        {
            for (int i = 0; i < _quantityTypeEnem; i++)
            {
                enemies.Add(listCurrentTypeEnemies[Random.Range(0, listCurrentTypeEnemies.Count - 1)]);
            }
        }
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
