using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float levelStarDelay = 2f;
    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public Enemy enemy;

    private int level = 1;
    private bool doingSetup;


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
        enemy = /*GameObject.FindGameObjectWithTag("Enemy");*/  // add instanciar enemy
        //healthManager = GetComponent<HealthManager>();
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;

        boardScript.SetupScene(level);
        //healthManager.SetupHealth();
    }

        // Update is called once per frame
        void Update()
    {
        //healthManager.health = player.UpdatePlayerHealth();
        //healthManager.UpdateUI();
    }

    void takeDamage( string colliderTag)
    {
        if (enemy.tag == colliderTag)
        {
            enemy.tak
        }
    }
}
