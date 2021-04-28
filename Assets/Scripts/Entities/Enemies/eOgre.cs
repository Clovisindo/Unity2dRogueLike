using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
        private GameObject attackCollider;
        private CircleCollider2D rangeAttackCollider;

        public AudioClip orcSpecialAtk;
        private bool specialAttacking = false;



        protected override void Awake()
        {
            animator = GetComponent<Animator>();
            target = FindObjectOfType<Player>().transform;
            enemyCurrentHealth = enemyMaxHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);
            attackCollider = Utilities.FindObjectWithTag(this.transform,"EnemyAttackRange");
            rangeAttackCollider = attackCollider.GetComponent<CircleCollider2D>();
            TypeEnemy = EnumTypeEnemies.weak;
            collider = this.GetComponent<BoxCollider2D>();
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

            //ataque especial si esta cerca
            if (Vector3.Distance(target.position, transform.position) <= maxAtkRange && Vector3.Distance(target.position, transform.position) >= minAtkRange)
            {
                if (timeBtwAttacks <= 0 && specialAttacking == false)
                {
                    specialAttacking = true;
                    attackOgre();
                    timeBtwAttacks = startTimeBtwAttacks;
                    //specialAttacking = false;
                }
                else
                {
                    timeBtwAttacks -= Time.deltaTime;
                    if (timeBtwAttacks <= 0)
                    {
                        specialAttacking = false;
                    }
                }
            }
            else if(specialAttacking)
            {
                timeBtwAttacks -= Time.deltaTime;
                if (timeBtwAttacks <= 0)
                {
                    specialAttacking = false;
                }
            }

            //if (passingTime < inmuneTime)
            //{
            //    passingTime += Time.deltaTime;
            //    enemyInmune = true;
            //}
            //else
            //{
            //    enemyInmune = false;
            //}
        }

        private void attackOgre()
        {
            rangeAttackCollider.enabled = true;
            //ataque especial del ogro
            SoundManager.instance.PlaySingle(orcSpecialAtk);

            //animacion ataque
            //animator.SetTrigger("AttackOgre");
        }

        //colision de ataque especial
        protected void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                //restamos vida al jugador
                GameObject enemyColl = other.gameObject;
                GameManager.instance.player.TakeDamage(enemyAttack);
                //ataque especial del ogro
                Debug.Log("Ataque en area del Ogro.");

                //2º desactivamos el collider del ataque especial
                rangeAttackCollider.enabled = false;
            }
        }

        void OnDrawGizmos()
        {
            if (specialAttacking)
            {
#if UNITY_EDITOR
                Handles.color = Color.green;
                Handles.DrawWireDisc(this.transform.position, this.transform.forward, maxAtkRange - minAtkRange);
#endif
            }
        }

    }
}
