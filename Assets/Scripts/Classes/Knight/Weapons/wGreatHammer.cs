using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wGreatHammer : Weapon
{
    public wGreatHammer()
    {
        startTimeBtwAttack = 1.383f;
        damage = 2;
        knockbackDistance = 2;
        knockbackSpeed = 0.5f;
    }

      void Start()
    {
        weapon = GameObject.FindGameObjectWithTag("GreatHammer");
        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponCollider = weapon.GetComponent<BoxCollider2D>();
        weaponAnimator = weapon.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();
    }

    public override void SpecialAttack()
    {
        Debug.Log("ataque especial martillo.");
        weaponAnimator.SetTrigger("SpecialAttack");
    }

    public override void ActiveSpecialParryAtk()
    {
        specialParryAttack = true;
    }
}
