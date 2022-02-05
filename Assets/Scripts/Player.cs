using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private Animator animator;
    private Transform transform;
    private List<Weapon> playerWeapons;
    private Weapon currentWeapon;
    private Weapon currentShield;

    private Vector2 movementDirection;
    private float movementSpeed;
    public float moveX;
    public float moveY;

    //inputactions
    Playerinputactions inputAction;
    bool changeWeaponPressed;
    bool EquipShieldPressed;
    bool attackWeaponPressed;

    //move
    Vector2 movementInput;
    

    public float MOVEMENT_BASE_SPEED = 3.0f;
    public int playerHealth = 3;
    private const float inmuneTime = 2.0f;
    private float passingTime = inmuneTime;
    private bool playerInmune = false;
    public bool playerExitCollision = false;
    public bool playerEntranceCollision = false;

    private float timeBtwChangeWeapon;
    private float startTimeBtwChangeWeapon = 0.5f;

    private float timeBtwEquipShield;
    private float startTimeBtwEquipShield = 5.0f;

    private bool falling = false;
    private bool currentWeaponAttacking = false;

    public bool CurrentWeaponAttacking { get => currentWeaponAttacking; set => currentWeaponAttacking = value; }




    // Start is called before the first frame update
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        playerWeapons = GetWeapons();
        SetCurrentWeapon(EnumWeapons.KnightSword);
        currentWeapon.gameObject.SetActive(true);
        inputAction = new Playerinputactions();
        inputAction.Playercontrols.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        

        //action buttons
        inputAction.Playercontrols.Changeweapon.performed += ctx => changeWeaponPressed = true;
        inputAction.Playercontrols.Changeweapon.canceled += ctx => changeWeaponPressed = false;
        inputAction.Playercontrols.EquipShield.performed += ctx => EquipShieldPressed = true;
        inputAction.Playercontrols.EquipShield.canceled += ctx => EquipShieldPressed = false;
        inputAction.Playercontrols.Attack.performed += ctx => attackWeaponPressed = true;
    }

    private void SetCurrentWeapon(EnumWeapons _enumWeapon)
    {
        //no casteamos el arma en concreto hasta asignarla en currentWeapon
        switch (_enumWeapon)
        {
            case EnumWeapons.GreatHammer:
                currentWeapon = (wGreatHammer)GetWeaponByTag(_enumWeapon);
                break;
            case EnumWeapons.KnightSword:
                currentWeapon = (wKnightSword)GetWeaponByTag(_enumWeapon);
                break;
            default:
                break;
        }
        HealthManager.instance.UpdateWeaponFrame(currentWeapon.WeaponSprite);
        Debug.Log("Arma cambiada a " + _enumWeapon.ToString());
    }
    /// <summary>
    /// se activa la bandera para que el proximo ataque sea el counter del parry
    /// </summary>
    internal void ActiveSpecialParryAtk()
    {
        currentWeapon.ActiveSpecialParryAtk();
    }
    /// <summary>
    /// Desactivar el funcionamiento de parry escudo hasta que hagas otro block
    /// </summary>
    public void DisableParryAttack()
    {
        currentShield.DisableColliderAttack();
        currentShield.GetComponent<wKnightShield>().DisableParryBehaviour();
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
        if (!falling)// no permitir control jugador si esta cayendo
        {
            ProcessInputs();
            animator.SetFloat("movementSpeed", movementSpeed);
            Move();
        }
    }

    void ProcessInputs()
    {
        moveX = movementInput.x;
        moveY = movementInput.y;
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

        //cambiar de arma
        if (timeBtwChangeWeapon <= 0)
        {
            if (changeWeaponPressed && currentWeapon.CheckIsIddleAnim())//!currentWeapon.IsAttacking)
            {
                ChangeWeapon();
                timeBtwChangeWeapon = startTimeBtwChangeWeapon;
                changeWeaponPressed = false;
            }
            if (!changeWeaponPressed)
            {
                changeWeaponPressed = false;
            }
        }
        else
        {
            timeBtwChangeWeapon -= Time.deltaTime;
        }

        //Equipar escudo
        if (timeBtwEquipShield <= 0)
        {
            if (EquipShieldPressed && !currentShield.gameObject.activeSelf && currentWeapon.CheckIsIddleAnim())//!currentWeapon.IsAttacking)
            {
                EquipShieldBlock();
            }
            if (currentShield.gameObject.activeSelf && (!EquipShieldPressed) && (currentShield.CheckIsIddleAnim()))
            {
                UnEquipShieldBlock();
                timeBtwEquipShield = startTimeBtwEquipShield;
            }
        }
        else
        {
            timeBtwEquipShield -= Time.deltaTime;
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
            SetPlayerHealth(- enemyColl.GetComponent<Enemy>().GetAttack());
            passingTime = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && !playerInmune)
        {
            GameObject enemyColl = other.gameObject;
            animator.SetTrigger("Hurt");
            SetPlayerHealth(- enemyColl.GetComponent<Enemy>().GetAttack());
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
        if ((other.tag == "Exit" || other.tag == "Entrance" || other.tag == "SecretDoor" ) && playerExitCollision == false)//collider puerta activada
        {
            playerExitCollision = true;
            GameManager.instance.MovePlayerToRoom(other.transform.parent.gameObject);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit" || other.tag == "Entrance" || other.tag == "SecretDoor")
        {
            playerExitCollision = false;
        }
    }

    private void SetPlayerHealth(int modifyHealth)
    {
        playerHealth += modifyHealth;
        UpdatePlayerHealth();
    }

    public bool PlayerAttackActivate()
    {
        return currentShield.CheckIsIddleAnim();
    }

    public void TakeDamage(int _damage)
    {
        SetPlayerHealth(-_damage);
        UpdatePlayerHealth();
    }
    public void UpdatePlayerHealth()
    {
        HealthManager.instance.UpdateUI(playerHealth);
    }

    public void UpdatePositionlevel(Vector2 respawnPosition)
    {
        transform.position = respawnPosition;
    }

    public void PlayerStartFalling()
    {
        if (!falling)
        {
            falling = true;
            animator.SetTrigger("falling");
        }
    }

    public void EndFallinPlayerAnim()
    {
        if (falling)
        {
            falling = false;
            //reseteamos la posicion inicial del jugador
            this.transform.position = GameManager.instance.ini_Player.transform.position;
            this.TakeDamage(1);
        }
       
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
            if (tWeapon.tag == EnumWeapons.KnightShield.ToString())//ToDo: no esta bien gestionado el tener el escudo en las armas
            {
                currentShield = tWeapon;
            }
        }
        return equipedWeapons;
    }

    private bool CheckIsWeapon(Weapon item)
    {
        var _enumWeapons = Utilities.EnumUtil.GetValues<EnumWeapons>();
        foreach (var _enumWeapon in _enumWeapons)
        {
            if (item.tag == EnumWeapons.KnightShield.ToString())//ToDo: no esta bien gestionado el tener el escudo en las armas
            {
                continue;
            }
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

    public void EquipShieldBlock()
    {
        currentShield.gameObject.SetActive(true);
        currentWeapon.gameObject.SetActive(false);
        HealthManager.instance.UpdateWeaponFrame(currentShield.WeaponSprite);
        currentShield.FirstAttack = false;
    }

    public void UnEquipShieldBlock()
    {
        currentShield.gameObject.SetActive(false);
        currentWeapon.gameObject.SetActive(true);
        HealthManager.instance.UpdateWeaponFrame(currentWeapon.WeaponSprite);
        timeBtwEquipShield = startTimeBtwEquipShield;
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
            case "KnightSword":
                _enumWeapon = EnumWeapons.KnightSword;
                break;
            case "KnightShield":
                _enumWeapon = EnumWeapons.KnightShield;
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

    private void OnEnable()
    {
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Disable();
    }
}
