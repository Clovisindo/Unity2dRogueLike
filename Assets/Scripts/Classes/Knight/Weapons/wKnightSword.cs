using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wKnightSword : Weapon
{

    public wKnightSword()
    {
        startTimeBtwAttack = 1.383f;
        damage = 1;
        knockbackDistance = 1;
        knockbackSpeed = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        weapon = GameObject.FindGameObjectWithTag("KnightSword");
        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponCollider = weapon.GetComponent<BoxCollider2D>();
        weaponAnimator = weapon.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
    }

    public override void SpecialAttack()
    {
        Debug.Log("ataque especial espada de caballero.");
        weaponAnimator.SetTrigger("SpecialAttack");
        base.SpecialAttack();
    }

    //internal override void ActiveSpecialParryAtk()
    //{
    //    specialParryAttack = true;
    //}

    //internal override void DisableSpecialParryAtk()
    //{
    //    specialParryAttack = false;
    //}
}
