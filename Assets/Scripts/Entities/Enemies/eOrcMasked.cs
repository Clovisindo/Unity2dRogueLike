using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class eOrcMasked : Enemy
{
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
        enemyCurrentHealth = enemyMaxHealth;
        healthBar.SetMaxHealth(enemyMaxHealth);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
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

        //Fintar a los lados ToDo:

    }

    void OnDrawGizmos()
    {
      //debug dibujar las fintas y direccion del muñeco
    }
}
