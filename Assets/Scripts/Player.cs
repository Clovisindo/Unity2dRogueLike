using Assets.Scripts.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator animator;
    private Transform transform;
    private List<Weapon> playerWeapons;
    private Weapon currentWeapon;
    private Weapon currentShield;
    private Weapon currentPickAxe;

    //Components
    [SerializeField]MoveComponent moveComponent;
    [SerializeField] InmuneComponent inmuneComponent;
    [SerializeField] ChangeWeaponComponent changeWeaponComponent;
    [SerializeField] EquipShieldComponent EquipShieldComponent;
    [SerializeField] ChangeUtilityComponent changeUtilityComponent;
    [SerializeField] FlashDamageComponent flashDamageComponent;

    [SerializeField] protected AudioClip HitSound;
    [SerializeField] protected AudioClip GameOverSound;

    private Vector2 movementDirection;
    private float movementSpeed;
    public float moveX;
    public float moveY;

    //inputactions
    Playerinputactions inputAction;
    bool changeWeaponPressed;
    bool EquipShieldPressed;
    bool EquipPickAxePressed;
    bool attackWeaponPressed;

    //move
    Vector2 movementInput;


    [SerializeField] private float MOVEMENT_BASE_SPEED = 5f;
    [SerializeField] private int playerHealth = 3;
    private const float inmuneTime = 2.0f;
    private float passingTime = inmuneTime;
    private bool playerInmune = false;
    private bool playerExitCollision = false;
    private bool playerEntranceCollision = false;

    private float timeBtwChangeWeapon;
    private float startTimeBtwChangeWeapon = 0.5f;

    private float timeBtwEquipShield;
    private float startTimeBtwEquipShield = 5.0f;

    private float timeBtwChangeUtility;
    private float startTimeBtwChangeUtility = 2f;

    private bool falling = false;
    private bool death = false;
    private bool currentWeaponAttacking = false;

    public bool CurrentWeaponAttacking { get => currentWeaponAttacking; set => currentWeaponAttacking = value; }
    public int PlayerHealth { get => playerHealth; set => playerHealth = value; }




    // Start is called before the first frame update
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        playerWeapons = GetWeapons();
        changeWeaponComponent.SetCurrentWeapon(EnumWeapons.KnightSword,ref currentWeapon, playerWeapons);
        currentWeapon.gameObject.SetActive(true);
        inputAction = new Playerinputactions();
        inputAction.Playercontrols.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();

        //action buttons
        inputAction.Playercontrols.Changeweapon.performed += ctx => changeWeaponPressed = true;
        inputAction.Playercontrols.Changeweapon.canceled += ctx => changeWeaponPressed = false;
        inputAction.Playercontrols.EquipShield.performed += ctx => EquipShieldPressed = true;
        inputAction.Playercontrols.EquipShield.canceled += ctx => EquipShieldPressed = false;
        inputAction.Playercontrols.EquipPickAxe.performed += ctx => EquipPickAxePressed = true;
        inputAction.Playercontrols.EquipPickAxe.canceled += ctx => EquipPickAxePressed = false;
        inputAction.Playercontrols.Attack.performed += ctx => attackWeaponPressed = true;
    }

    //inputs
    void FixedUpdate()
    {
        CheckStatusPlayer();
        if (!falling && !death)// no permitir control jugador si esta cayendo o en animacion de muerte
        {
            ProcessInputs();
            animator.SetFloat("movementSpeed", movementSpeed);
            rb2D.velocity = moveComponent.Move(rb2D,MOVEMENT_BASE_SPEED);
        }
    }

    private void CheckStatusPlayer()
    {
        if (PlayerHealth <= 0 && !death)
        {
            animator.SetTrigger("death");
            SoundManager.instance.PlaySingle(GameOverSound);
            death = true;
        };
    }

    // --------------------inputs-------------
    void ProcessInputs()
    {
        movementInput = moveComponent.MoveBehaviour(movementInput, animator);
        playerInmune = inmuneComponent.InmuneBehaviour(ref passingTime,inmuneTime,  playerInmune);
        changeWeaponComponent.ChangeWeaponBehaviour(ref timeBtwChangeWeapon,startTimeBtwChangeWeapon,ref changeWeaponPressed,ref currentWeapon,playerWeapons);
        EquipShieldPressed = EquipShieldComponent.EquipShieldBehaviour(ref timeBtwEquipShield,startTimeBtwEquipShield,EquipShieldPressed,currentShield,currentWeapon);
        EquipPickAxePressed = changeUtilityComponent.ChangeUtilityBehaviour(ref timeBtwChangeUtility,startTimeBtwChangeUtility,EquipPickAxePressed,currentPickAxe,currentWeapon);
    }

    //-----------------armas-----------------

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
            if (tWeapon.tag == EnumWeapons.PickAxe.ToString())//ToDo: no esta bien gestionado el tener el escudo en las armas
            {
                currentPickAxe = tWeapon;
            }
        }
        return equipedWeapons;
    }

    private bool CheckIsWeapon(Weapon item)
    {
        var _enumWeapons = Utilities.EnumUtil.GetValues<EnumWeapons>();
        foreach (var _enumWeapon in _enumWeapons)
        {
            if (item.tag == EnumWeapons.KnightShield.ToString() || item.tag == EnumWeapons.PickAxe.ToString())//ToDo: no esta bien gestionado el tener el escudo en las armas
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

    //---------------colisiones------------------
    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Enemy"  && !playerInmune && !death)
        {
            GameObject enemyColl = other.gameObject;
            animator.SetTrigger("Hurt");
            TakeDamage(-enemyColl.GetComponent<Enemy>().GetAttack());
            passingTime = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && !playerInmune && !death)
        {
            GameObject enemyColl = other.gameObject;
            animator.SetTrigger("Hurt");
            TakeDamage(-enemyColl.GetComponent<Enemy>().GetAttack());
            passingTime = 0;
        }
    }

    private void OnCollisionStay2D(CircleCollider2D other)
    {
        if (other.gameObject.tag == "Enemy" && !playerInmune && !death)
        {
            //ataque especial del ogro
            Debug.Log("Ataque en area del Ogro.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Exit") || other.CompareTag("Entrance") || other.CompareTag("SecretDoor")) && playerExitCollision == false)//collider puerta activada
        {
            playerExitCollision = true;
            GameManager.instance.MovePlayerToRoom(other.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.CompareTag("Exit") || other.CompareTag("Entrance") || other.CompareTag("SecretDoor"))
        {
            playerExitCollision = false;
        }
    }

    //--------------------vida----------------------
    private void SetPlayerHealth(int modifyHealth)
    {
        playerHealth += modifyHealth;
        UpdatePlayerHealth();
    }

    public void TakeDamage(int _damage)
    {
        SetPlayerHealth(_damage);
        flashDamageComponent.Flash(Color.red);
        SoundManager.instance.PlaySingle(HitSound);
    }
    public void UpdatePlayerHealth()
    {
        HealthManager.instance.UpdateUI(playerHealth);
    }


    //-------------- gameManager---------------
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

    public void EndDeathPlayerAnim()
    {
        if (death)
        {
            GameManager.instance.DefeatEndGame();
        }

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
