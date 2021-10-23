using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wKnightShield : Weapon
{
    int damage = 0;
    bool parryBehaviour = false;
    Player playerClass;
    

    public wKnightShield()
    {
        isAttacking = false;
        startTimeBtwAttack = 1.383f;
    }

    protected override void Awake()
    {
        weapon = GameObject.FindGameObjectWithTag("KnightShield");
        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponCollider = weapon.GetComponent<BoxCollider2D>();
        weaponAnimator = weapon.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerClass = player.GetComponent<Player>();
        playerAnimator = player.GetComponent<Animator>();
    }

    protected override void ProcessInputs()
    {
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && isAttacking && !parryBehaviour)
        {
            Parry();
        }
    }

    void FixedUpdate()
    {
        setDirectionAttack();
    }

    protected override void setDirectionAttack()
    {
        moveX = playerAnimator.GetFloat("moveX");
        moveY = playerAnimator.GetFloat("moveY");

        weaponAnimator.SetFloat("moveX", moveX);
        weaponAnimator.SetFloat("moveY", moveY);
    }

    //public  void Block()
    //{
    //    Debug.Log("Bloqueas el ataque.");
    //    //weaponAnimator.SetTrigger("SpecialAttack");
    //}

    public  void Parry()
    {
        ActiveParryBehaviour();
        Debug.Log("Devuelve el golpe!");
        //weaponAnimator.SetTrigger("SpecialAttack");
        //activamos el ataque especial del jugador cuando hace parry
        playerClass.ActiveSpecialParryAtk();
    }
    
        public override void setIsAttacking()
    {
        weaponAnimator.SetTrigger("Attacking");
        isAttacking = true;
    }

    public void ActiveParryBehaviour()
    {
        parryBehaviour = true;
    }
    public void DisableParryBehaviour()
    {
        parryBehaviour = false;
    }

    /// <summary>
    /// al acabar la animacion desactiva el escudo
    /// </summary>
    public void UnequipShield()
    {
        weaponAnimator.SetTrigger("Attacking");
        isAttacking = false;
        playerClass.UnEquipShieldBlock();
    }

    public override void ActiveSpecialParryAtk()
    {
        specialParryAttack = true;
    }
}
