using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Utility = Assets.Utilities.Utilities;

namespace Assets.Scripts.Entities.Enemies
{
    public class eOgre : Enemy
    {
        [SerializeField]
        protected float minAtkRange;
        [SerializeField]
        protected float maxAtkRange;


        public override string name => "eOgre";
        private GameObject attackCollider;
        private CircleCollider2D rangeAttackCollider;

        public AudioClip orcSpecialAtk;
        private bool specialAttacking = false;



        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            target = FindObjectOfType<Player>().transform;
            enemyCurrentHealth = enemyMaxHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);
            attackCollider = Utility.FindObjectWithTag(this.transform,"EnemyAttackRange");
            rangeAttackCollider = attackCollider.GetComponent<CircleCollider2D>();
            collider = this.GetComponent<BoxCollider2D>();
            rb = this.GetComponent<Rigidbody2D>();
            startTimeBtwAttacks = 5f;
        }

        protected override void EnemyBehaviour()
        {
            MovementEnemyBehaviour();
            SpecialAttackBehaviour();
        }

        private void SpecialAttackBehaviour()
        {
            if (Vector3.Distance(target.position, transform.position) <= maxAtkRange && Vector3.Distance(target.position, transform.position) >= minAtkRange)
            {
                if (timeBtwAttacks <= 0 && specialAttacking == false)
                {
                    attackOgre();
                }
                else
                {
                    CheckBtwnAttacks();
                }
            }
            else if (specialAttacking)
            {
                CheckBtwnAttacks();
            }
        }

        private void CheckBtwnAttacks()
        {
            timeBtwAttacks -= Time.deltaTime;
            if (timeBtwAttacks <= 0)
            {
                specialAttacking = false;
            }
        }
        private void attackOgre()
        {
            specialAttacking = true;
            //ataque especial del ogro
            SoundManager.instance.PlaySingle(orcSpecialAtk);
            FlashColorEffect(Color.white);
            //animacion ataque
            animator.SetTrigger("AttackOgre");
            timeBtwAttacks = startTimeBtwAttacks;
        }

        protected void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                DisableColliderAttack();
                GameObject enemyColl = other.gameObject;
                GameManager.instance.player.TakeDamage(-enemyAttack);
            }
        }

        protected void EnableColliderAttack()
        {
            rangeAttackCollider.enabled = true;

        }

        protected void DisableColliderAttack()
        {
            rangeAttackCollider.enabled = false;

        }
        void EndAnimationOgre()
        {
            animator.SetTrigger("AttackOgre");
        }

//        void OnDrawGizmos()
//        {
//            if (specialAttacking)
//            {
//#if UNITY_EDITOR
//                Handles.color = Color.green;
//                Handles.DrawWireDisc(this.transform.position, this.transform.forward, maxAtkRange - minAtkRange);
//#endif
//            }
//        }

    }
}

