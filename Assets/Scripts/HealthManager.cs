﻿using System;
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
    [SerializeField]private Image weaponFrame;
    [SerializeField] private Image equipFrame;
    [SerializeField] private Text equipFrameText;
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
        weaponFrame = GameObject.FindGameObjectWithTag("weaponFrame").GetComponent<Image>();
        equipFrame = GameObject.FindGameObjectWithTag("EquipFrame").GetComponent<Image>();//ToDo: implementar mas equipo
        equipFrameText = GameObject.FindGameObjectWithTag("EquipFrameText").GetComponent<Text>();
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


    public void UpdateWeaponFrame( Sprite newSpriteFrame)
    {
        weaponFrame.sprite = newSpriteFrame;
    }
    public void UpdateEquipUses(int equipmentUses)
    {
        equipFrameText.text = (equipmentUses).ToString();
    }
    public void UpdateUI(int _health)
    {
        health = _health;
        //Init();
    }

    public bool IsMaxHealth()
    {
        if (health ==numOfHearts)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
