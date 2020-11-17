using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //public float levelStarDelay = 2f;
    //public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public LevelGeneration levelGenerationScript;
    public Player player;
    public Enemy enemy;

    private int level = 1;
    private bool doingSetup;

    public GameObject gameCamera;

    //instantiate prefabs
    public GameObject ini_Player;
    public GameObject ini_Enemy;



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
        boardScript = GetComponent<BoardManager>();
        levelGenerationScript = GetComponent<LevelGeneration>();
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera");

        player = Instantiate(player, ini_Player.transform.position, Quaternion.identity);
        enemy = Instantiate(enemy, ini_Enemy.transform.position, Quaternion.identity);
        //healthManager = GetComponent<HealthManager>();
        InitGame();
        
    }

    void InitGame()
    {
        doingSetup = true;

        //boardScript.SetupScene(level);
        //healthManager.SetupHealth();
    }

        // Update is called once per frame
        void Update()
    {
        //healthManager.health = player.UpdatePlayerHealth();
        //healthManager.UpdateUI();
    }

    public void ChangeLevel()
    {
        //ToDo: gestionar la camara en funcion de la direccion de la habitacion , entrada o salida,etc
        gameCamera.transform.position = new Vector3( gameCamera.transform.position.x +  levelGenerationScript.moveAmountX,
        gameCamera.transform.position.y +  levelGenerationScript.moveAmountY, gameCamera.transform.position.z);
        GameManager.instance.player.UpdatePositionlevel(4);//ToDo: como gestionar el personaje moviendose de habitacion en habitacion
    }

    public void takeDamage( string colliderTag)
    {
        if (enemy.tag == colliderTag && (!enemy.checkIsInmune()))
        {
            enemy.TakeDamage(1);// TODO: weaponDagame
        }
    }
}
