using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class eGoblin : Enemy
    {
        Vector3 playerPosition;
        public override string name => "eGoblin";
        float randMoveX;
        float randMoveY;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            target = FindObjectOfType<Player>().transform;
            enemyCurrentHealth = enemyMaxHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);
            collider = this.GetComponent<BoxCollider2D>();
            rb = this.GetComponent<Rigidbody2D>();
            minRange = minRangeAtk;

           
        }

        protected override void EnemyBehaviour()
        {
            MovementEnemyBehaviour();
            AttackRangeBehaviour();
            ChangePathBehaviour();
        }



        protected override void FollowPlayer()
        {
            SetAnimatorMovement();
            playerPosition = target.transform.position;

            if (changeFollowingPath) { GenerateRandomMove(ref playerPosition); }

            transform.position = Vector3.MoveTowards(transform.position, changeFollowingPath ? playerPosition * (-1) : playerPosition,
                speed * Time.deltaTime);
        }

        private void GenerateRandomMove(ref Vector3 playerPosition)
        {
            playerPosition.x += Random.Range(0f, 0.8f);
            playerPosition.y += Random.Range(0f, 0.8f);
            CheckBoundariesMovement();
        }

        //---------------colisiones------------------
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player" && !attackRelease)
            {
                base.ReleaseAfterAtkBehaviour();
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player" && !attackRelease)
            {
                base.ReleaseAfterAtkBehaviour();
            }
        }
    }
