using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wKnightShield : Weapon
{
    int damage = 0;


    public wKnightShield()
    {
        startTimeBtwAttack = 1.383f;
    }

    void Start()
    {
        weapon = GameObject.FindGameObjectWithTag("KnightShield");
        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponCollider = weapon.GetComponent<BoxCollider2D>();
        //weaponAnimator = weapon.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
    }

    public  void Block()
    {
        Debug.Log("Bloqueas el ataque.");
        //weaponAnimator.SetTrigger("SpecialAttack");
    }

    public  void Parry()
    {
        Debug.Log("Devuelve el golpe!");
        //weaponAnimator.SetTrigger("SpecialAttack");
    }
}
