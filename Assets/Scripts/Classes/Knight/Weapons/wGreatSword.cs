using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wGreatSword : Weapon
{
    public wGreatSword()
    {
        startTimeBtwAttack = 0.83f;
    }

    void Start()
    {
        weapon = GameObject.FindGameObjectWithTag("GreatSword");
        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponCollider = weapon.GetComponent<BoxCollider2D>();
        weaponAnimator = weapon.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
    }

    public override void SpecialAttack()
    {
        Debug.Log("ataque especial espadón.");
        weaponAnimator.SetTrigger("SpecialAttack");
    }

    public override void ActiveSpecialParryAtk()
    {
        specialParryAttack = true;
    }
}
