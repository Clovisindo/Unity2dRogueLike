using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Enemies
{
    class eGoblin : Enemy
    {
        bool attackRelease = false;
        private float timeBtwAttacks;
        private float startTimeBtwAttacks = 5.0f;
        const float minRangeNoAtk = 3;
        const float minRangeAtk = 0;

        protected override void Awake()
        {
            animator = GetComponent<Animator>();
            target = FindObjectOfType<Player>().transform;
            enemyCurrentHealth = enemyMaxHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);
            TypeEnemy = EnumTypeEnemies.weak;
            collider = this.GetComponent<BoxCollider2D>();
            rb = this.GetComponent<Rigidbody2D>();
            

        }

        protected override void EnemyBehaviour()
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

            if (timeBtwAttacks <= 0)
            {
                attackRelease = false;
                minRange = minRangeAtk;
            }
            else
            {
                timeBtwAttacks -= Time.deltaTime;
            }

        }

        //---------------colisiones------------------
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player" && !attackRelease)
            {
                minRange = minRangeNoAtk;
                timeBtwAttacks = startTimeBtwAttacks;
                attackRelease = true;
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player" && !attackRelease)
            {
                minRange = minRangeNoAtk;
                timeBtwAttacks = startTimeBtwAttacks;
                attackRelease = true;
            }
        }

    }
}
