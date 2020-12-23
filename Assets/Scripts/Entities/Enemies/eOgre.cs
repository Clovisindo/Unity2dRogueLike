using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Entities.Enemies
{
    public class eOgre : Enemy
    {
        [SerializeField]
        protected float minAtkRange;
        [SerializeField]
        protected float maxAtkRange;

        private float timeBtwAttacks;
        private float startTimeBtwAttacks = 2f;
        CircleCollider2D attackCollider;

        protected override void Start()
        {
            animator = GetComponent<Animator>();
            target = FindObjectOfType<Player>().transform;
            enemyCurrentHealth = enemyMaxHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);
            //attackCollider = Utilities.FindObjectWithTag(this.transform, "EnemyAttackRange");
            attackCollider = GetComponent<CircleCollider2D>();
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

            //ataque especial si esta cerca
            if (Vector3.Distance(target.position, transform.position) <= maxAtkRange && Vector3.Distance(target.position, transform.position) >= minAtkRange)
            {
                if (timeBtwAttacks <= 0)
                {
                    attackOgre();
                    timeBtwAttacks = startTimeBtwAttacks;
                }
                else
                {
                    timeBtwAttacks -= Time.deltaTime;
                }
                
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

        private void attackOgre()
        {
            //1º desactivamos el collider del ogro
            attackCollider.gameObject.SetActive(true);
            //ataque especial del ogro
            Debug.Log("Entra en area del Ogro.");

            //animacion ataque
            //animator.SetTrigger("AttackOgre");

           
        }

        //colision de ataque especial
        protected void OnTriggerEnter2D(CircleCollider2D other)
        {
            if (other.tag == "Player")
            {
                //restamos vida al jugador
                GameObject enemyColl = other.gameObject;
                GameManager.instance.player.TakeDamage(enemyAttack);
                //ataque especial del ogro
                Debug.Log("Ataque en area del Ogro.");

                //2º desactivamos el collider del ataque especial
                attackCollider.gameObject.SetActive(false);
            }
        }

        void OnDrawGizmos()
        {
            Handles.color = Color.green;
            Handles.DrawWireDisc(this.transform.position, this.transform.forward, maxAtkRange - minAtkRange);
        }

    }
}
