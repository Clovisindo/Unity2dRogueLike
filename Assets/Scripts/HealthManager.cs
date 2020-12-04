using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int health;
    public int numOfHearts;
    public static HealthManager instance = null;
    public Canvas UICanvas;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

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
        //UICanvas = GameObject.FindObjectOfType<Canvas>();
        //foreach (var heartImage in hearts)
        //{
        //    Instantiate(heartImage, UICanvas.transform, false);
        //}
        

        Init();

    }
    private void Init()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            // este if define cuando el corazon carga la imagen rellena o no, segun la cantidad de vida
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            // controla el numero de corazones maximos que se muestran( los previamente creados en los objetos)
            //mejora para esto seria instanciarlo desde el iniciador
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void Update()
    {
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            // este if define cuando el corazon carga la imagen rellena o no, segun la cantidad de vida
            if (i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            // controla el numero de corazones maximos que se muestran( los previamente creados en los objetos)
            //mejora para esto seria instanciarlo desde el iniciador
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    //public void SetupHealth()
    //{
    //    Init();
    //}

    public void UpdateUI(int _health)
    {
        health = _health;
        //Init();
    }
}
