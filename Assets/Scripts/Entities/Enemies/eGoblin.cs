using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Enemies
{
    class eGoblin : Enemy
    {

        private Animator animator;
        private Transform target;

        //Override properties
        [SerializeField]
        private new float speed;
        [SerializeField]
        private new float minRange;
        [SerializeField]
        private new float maxRange;

        //Attackw
        private const int goblinAttack = 1;

        //movement
        private new bool isMoving = false;

        protected override void Start()
        {
            animator = GetComponent<Animator>();
            target = FindObjectOfType<Player>().transform;
            enemyAttack = goblinAttack;
            enemyCurrentHealth = enemyMaxHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);

        }

        protected override void FixedUpdate() // patron distinto al goblin y con ataque
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
        protected override void FollowPlayer()
        {
            animator.SetFloat("moveX", (target.position.x - transform.position.x));// esto para devolver a la animacion donde mirar??
            animator.SetFloat("moveY", (target.position.y - transform.position.y));
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }

    }
}
