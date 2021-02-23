using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eMimic : Enemy
{
    protected bool mimicActivated = false;
    protected GameObject HPBarobject;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
        enemyCurrentHealth = enemyMaxHealth;
        healthBar.SetMaxHealth(enemyMaxHealth);
        HPBarobject = Utilities.GetChildObject(this.transform, "healthBar");
        TypeEnemy = EnumTypeEnemies.strong;
    }

    protected override void FixedUpdate()
    {
        if (mimicActivated)
        {
            if (Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) >= minRange)
            {
                FollowPlayer();
                isMoving = true;
                animator.SetBool("isMoving", isMoving);
            }
            else
            {
                isMoving = false;
                animator.SetBool("isMoving", isMoving);
                //goRespawn();
            }

            if (passingTime < inmuneTime)
            {
                passingTime += Time.deltaTime;
                enemyInmune = true;
            }
            else
            {
                enemyInmune = false;
            }
        }
    }

    /// <summary>
    /// Cuando se acerce el jugador, se activa el enemigo
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !mimicActivated)
        {
            ActivateMimic();
        }
    }

    private void ActivateMimic ()
    {
        mimicActivated = true;
        animator.SetTrigger("Activate");
        HPBarobject.SetActive(true);
    }
}
