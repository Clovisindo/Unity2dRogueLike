﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private Animator animator;
    private Transform transform;
    private List<Weapon> playerWeapons;
    private Weapon currentWeapon;

    private Vector2 movementDirection;
    private float movementSpeed;
    public float moveX;
    public float moveY;

    public float MOVEMENT_BASE_SPEED = 3.0f;
    public int playerHealth = 3;
    private const float inmuneTime = 2.0f;
    private float passingTime = inmuneTime;
    private bool playerInmune = false;
    public bool playerExitCollision = false;
    public bool playerEntranceCollision = false;

    private float timeBtwChangeWeapon;
    private float startTimeBtwChangeWeapon = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        playerWeapons = GetWeapons();
        SetCurrentWeapon(EnumWeapons.GreatSword);
        currentWeapon.gameObject.SetActive(true);
    }

    private void SetCurrentWeapon(EnumWeapons _enumWeapon)
    {
        //no casteamos el arma en concreto hasta asignarla en currentWeapon
        switch (_enumWeapon)
        {
            case EnumWeapons.GreatSword:
                currentWeapon = (wGreatSword)GetWeaponByTag(_enumWeapon);
                break;
            case EnumWeapons.GreatHammer:
                currentWeapon = (wGreatHammer)GetWeaponByTag(_enumWeapon);
                break;
            default:
                break;
        }
        Debug.Log("Arma cambiada a " + _enumWeapon.ToString());
    }

    private Weapon GetWeaponByTag(EnumWeapons _enumWeapon)
    {
        foreach (var weapon in playerWeapons)
        {
            if (weapon.tag == _enumWeapon.ToString())
            {
                return weapon.GetComponent<Weapon>();
            }
        }
        return null;
    }

    void FixedUpdate()
    {
        ProcessInputs();
        animator.SetFloat("movementSpeed", movementSpeed);
        Move();
    }

    void ProcessInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(moveX, moveY);

        animator.SetFloat("moveX", moveX);
        animator.SetFloat("moveY", moveY);

        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        movementDirection.Normalize();

        if (passingTime < inmuneTime)
        {
            passingTime += Time.deltaTime;
            playerInmune = true;
        }
        else
        {
            playerInmune = false;
        }

        if (timeBtwChangeWeapon <= 0)
        {
            //change weapon
            if (Input.GetKey(KeyCode.T))
            {
                ChangeWeapon();
                timeBtwChangeWeapon = startTimeBtwChangeWeapon;
            }
        }
        else
        {
            timeBtwChangeWeapon -= Time.deltaTime;
        }
    }

    protected void Move()
    {
        rb2D.velocity = movementDirection * movementSpeed * MOVEMENT_BASE_SPEED;
    }

    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Enemy"  && !playerInmune)
        {
            GameObject enemyColl = other.gameObject;
            animator.SetTrigger("Hurt");
            //restamos vida al jugador
            SetPlayerHealth(- enemyColl.GetComponent<Enemy>().GetAttack());
            //UpdatePlayerHealth();
            passingTime = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && !playerInmune)
        {
            GameObject enemyColl = other.gameObject;
            animator.SetTrigger("Hurt");
            //restamos vida al jugador
            SetPlayerHealth(- enemyColl.GetComponent<Enemy>().GetAttack());
            //UpdatePlayerHealth();
            passingTime = 0;
        }
    }

    private void OnCollisionStay2D(CircleCollider2D other)
    {
        if (other.gameObject.tag == "Enemy" && !playerInmune)
        {
            //ataque especial del ogro
            Debug.Log("Ataque en area del Ogro.");
        }
    }



        private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit" && playerExitCollision == false)
        {
            playerExitCollision = true;
            GameManager.instance.ChangeLevel(false);
            
        }

        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Entrance" && playerExitCollision == false)
        {
            playerExitCollision = true;
            GameManager.instance.ChangeLevel(true);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit" )
        {
            //GameManager.instance.currentRoom.EnableChangeEventColliderExitRoom();
            playerExitCollision = false;
        }

        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Entrance" )
        {
            //GameManager.instance.currentRoom.EnableChangeEventColliderEntranceRoom();
            playerExitCollision = false;
        }
    }

    private void SetPlayerHealth(int modifyHealth)
    {
        playerHealth += modifyHealth;
        UpdatePlayerHealth();
    }

    public void TakeDamage(int _damage)
    {
        SetPlayerHealth(-_damage);
        UpdatePlayerHealth();
    }

    private List<Weapon> GetWeapons()
    {
        List<Weapon> tempWeapons = Utilities.getAllChildsObject<Weapon>(this.transform);
        List<Weapon> equipedWeapons = new List<Weapon>();

        foreach (var tWeapon in tempWeapons)
        {
            if (CheckIsWeapon(tWeapon))
            {
                equipedWeapons.Add(tWeapon);
            }
        }
        return equipedWeapons;
    }

    private bool CheckIsWeapon(Weapon item)
    {
        var _enumWeapons = Utilities.EnumUtil.GetValues<EnumWeapons>();
        foreach (var _enumWeapon in _enumWeapons)
        {
            if (_enumWeapon.ToString() == item.tag)
            {
                return true;
            }
        }
        return false;
    }

    private void ChangeWeapon()
    {
        // current weapon disable
        currentWeapon.gameObject.SetActive(false);
        //Get next weapon
        var nextWeapon = GetNextWeapon(currentWeapon.tag);
        SetCurrentWeapon(GetEnumWeaponByTag(nextWeapon.tag));
        //enable current weapon
        currentWeapon.gameObject.SetActive(true);
    }

    private EnumWeapons GetEnumWeaponByTag(string weaponTag)
    {
        EnumWeapons _enumWeapon;
        switch (weaponTag)
        {
            case "GreatSword":
                _enumWeapon = EnumWeapons.GreatSword;
                break;
            case "GreatHammer":
                _enumWeapon = EnumWeapons.GreatHammer;
                break;
            default:
                _enumWeapon = EnumWeapons.GreatHammer;//ToDo: controlar nulos
                break;
        }
        return _enumWeapon;
    }

    private Weapon GetNextWeapon(string currentWeaponTag)
    {
        for (int i = 0; i < playerWeapons.Count; i++)
        {
            if (playerWeapons[i].tag == currentWeaponTag)
            {
                if ((i + 1) >= playerWeapons.Count)
                {
                    return playerWeapons[0].GetComponent<Weapon>();
                }
                else
                {
                    return playerWeapons[i + 1].GetComponent<Weapon>();//controlar esto para que de la vuelta
                }
               
            }
        }
        return null;
    }

    public void UpdatePlayerHealth()
    {
       HealthManager.instance.UpdateUI(playerHealth);
    }

    public void UpdatePositionlevel(Vector2 respawnPosition)
    {
        transform.position = respawnPosition;
        //playerExitCollision = false;
    }





}
