using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Enemies
{
    class eGoblin : Enemy
    {

        protected override void Awake()
        {
            animator = GetComponent<Animator>();
            target = FindObjectOfType<Player>().transform;
            enemyCurrentHealth = enemyMaxHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);
        }

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
}
