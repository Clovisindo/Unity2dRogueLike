using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Enemies
{
    class eGoblin : Enemy
    {
        Vector3 playerPosition;

        float randMoveX;
        float randMoveY;

        bool attackRelease = false;
        private float timeBtwAttacks;
        private float startTimeBtwAttacks = 5.0f;
        const float minRangeNoAtk = 1;
        const float minRangeAtk = 0;

        const float totalTimeFollowing = 5f;
        float passingTimeFollowing = totalTimeFollowing;
       

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            target = FindObjectOfType<Player>().transform;
            enemyCurrentHealth = enemyMaxHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);
            TypeEnemy = EnumTypeEnemies.weak;
            collider = this.GetComponent<BoxCollider2D>();
            rb = this.GetComponent<Rigidbody2D>();
            minRange = minRangeAtk;

           
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

            if (changeFollowingPath)
            {
                if (passingTimeFollowing < totalTimeFollowing)
                {
                    passingTimeFollowing += Time.deltaTime;
                }
                else
                {
                    changeFollowingPath = false;
                }
            }
        }
        protected override void FollowPlayer()
        {
            animator.SetFloat("moveX", (target.position.x - transform.position.x));// esto para devolver a la animacion donde mirar??
            animator.SetFloat("moveY", (target.position.y - transform.position.y));

            playerPosition = target.transform.position;
            if (changeFollowingPath)
            {
                GenerateRandomMove(ref playerPosition);
                transform.position = Vector3.MoveTowards(transform.position, playerPosition *(-1), speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
            }
            //aplicar checkboundaries
            var tempPosition = CheckBoundariesMovement();
            if (tempPosition != Vector3.zero)
            {
                transform.position = tempPosition;
            }
        }

        private void GenerateRandomMove(ref Vector3 playerPosition)
        {
            randMoveX = Random.Range(0f, 0.8f);
            randMoveY = Random.Range(0f, 0.8f);
            playerPosition.x += randMoveX;
            playerPosition.y += randMoveY;
            //check no salirse de los bordes
        }

        //---------------colisiones------------------
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player" && !attackRelease)
            {
                minRange = minRangeNoAtk;
                changeFollowingPath = true;
                passingTimeFollowing = 0f;
                timeBtwAttacks = startTimeBtwAttacks;
                attackRelease = true;
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player" && !attackRelease)
            {
                minRange = minRangeNoAtk;
                changeFollowingPath = true;
                passingTimeFollowing = 0f;
                timeBtwAttacks = startTimeBtwAttacks;
                attackRelease = true;
            }
        }

    }
}
