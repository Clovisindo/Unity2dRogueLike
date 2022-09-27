using Assets.Scripts.Components;
using System.Xml.Linq;
using UnityEngine;
using Enemy = Assets.Scripts.Entities.Enemies.Enemy;

namespace Assets.Scripts.Entities.Enemies
{
    [System.Serializable]
    public abstract class Enemy : MonoBehaviour
    {
        protected Animator animator;
        protected Transform target;

        public abstract string name { get; }

        //Override properties
        [SerializeField]
        protected float speed = 0.5f;
        [SerializeField]
        protected float minRange;
        [SerializeField]
        protected float maxRange;
        [SerializeField]
        private bool finalBoss = false;
        protected Rigidbody2D rb;
        protected BoxCollider2D collider;

        //Components
        [SerializeField] FlashDamageComponent flashDamageComponent;

        //health
        [SerializeField]
        protected int enemyMaxHealth;
        [SerializeField]
        protected int enemyCurrentHealth;
        [SerializeField]
        protected HealthBar healthBar;

        //Attack
        [SerializeField]
        protected int enemyAttack;
        [SerializeField]
        protected AudioClip playerHit;

        //LevelBoundaries
        //protected Vector2 screenBoundsMax;
        //protected Vector2 screenBoundsMin;
        protected float objectWidth;
        protected float objectHeight;


        //movement
        protected bool isMoving = false;
        protected float moveX;
        protected float moveY;

        protected Vector2 positionEndKnockback = default;
        private Vector3 respawnPosition;

        private float distanceKnockback = 0;
        private float kbDistance = 1;
        private float kbSpeed = 1;

        //const stats
        protected const float inmuneTime = 2.0f;
        protected float knockbackResistence = 1;
        protected float passingTime = inmuneTime;
        protected bool enemyInmune = false;
        [SerializeField]
        private bool isPaused = false;
        private bool CheckKknockback = false;
        public bool changeFollowingPath = false;

        protected bool attackRelease = false;
        protected float timeBtwAttacks;
        protected float startTimeBtwAttacks = 5.0f;
        protected const float minRangeNoAtk = 1;
        protected const float minRangeAtk = 0;

        protected const float totalTimeFollowing = 5f;
        protected float passingTimeFollowing = totalTimeFollowing;


        //Enemigos habitacion
        public bool IsPaused { get => isPaused; set => isPaused = value; }


        public Enemy()
        { }

        protected virtual void Awake()
        {
            rb = this.GetComponent<Rigidbody2D>();
            collider = this.GetComponent<BoxCollider2D>();
            respawnPosition = Vector3.zero;

            objectWidth = collider.bounds.size.x / 2;
            objectHeight = collider.bounds.size.y / 2;

        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (!isPaused)
            {

                InmuneBehaviour();
                if (CheckKknockback)
                {
                    CheckKknockback = KnockbackBehaviour(kbDistance * knockbackResistence, kbSpeed * knockbackResistence, GameManager.instance.player.transform);
                }
                else
                {
                    EnemyBehaviour();
                    distanceKnockback = 0;
                }

                if (CheckIsDeath())
                {
                    DestroyEnemy(this);
                }
            }
        }

        internal void DestroyEnemy(Enemy enemy)
        {
            GameManager.instance.currentRoom.enemiesRoom.Remove(enemy.GetComponent<Enemy>());
            if (GameManager.instance.CheckLastEnemyRoom())
            {
                GameManager.instance.currentRoom.OpenDoor();
                GameManager.instance.currentRoom.RoomComplete = true;
            }
            if (finalBoss)
            {
                LevelGeneration.ActivateVictoryScene();
            }
            Destroy(gameObject);
        }

        protected void InmuneBehaviour()
        {
            if (passingTime < inmuneTime)
            {
                passingTime += Time.deltaTime;
                enemyInmune = true;
                collider.enabled = false;
            }
            else
            {
                collider.enabled = true;
                enemyInmune = false;
            }
        }

        /// <summary>
        /// Aplica un empuje al enemigo en un intervalo de tiempo
        /// </summary>
        /// <param name="knockbackDistance"> distancia de desplazamiento</param>
        /// <param name="knockbackSpeed"> velocidad</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected bool KnockbackBehaviour(float knockbackDistance, float knockbackSpeed, Transform obj)
        {
            Vector2 difference = (transform.position - obj.position).normalized * knockbackDistance;

            if (positionEndKnockback == default) { positionEndKnockback = new Vector3(transform.position.x + difference.x, transform.position.y + difference.y); }

            if (distanceKnockback <= knockbackDistance)//moviendo frame a frame
            {
                Vector2 currentPositionKB = Vector2.Lerp(transform.position, new Vector2(transform.position.x + difference.x,
                    transform.position.y + difference.y), knockbackSpeed * 2 * Time.deltaTime);// speed(1) * 2 es lo minimo de velocidad que queremos
                distanceKnockback += Vector3.Distance(transform.position, currentPositionKB);
                transform.position = currentPositionKB;
                return true;
            }
            else
            {
                positionEndKnockback = default;//fin knockback
                return false;
            }
        }

        protected abstract void EnemyBehaviour();

        protected virtual void FollowPlayer()
        {
            SetAnimatorMovement();
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }

        protected virtual void GoRespawn()
        {
            SetAnimatorMovement();
            transform.position = Vector3.MoveTowards(transform.position, respawnPosition, speed * Time.deltaTime);
        }

        protected virtual void MovementEnemyBehaviour()
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
            }
        }

        protected virtual void AttackRangeBehaviour()
        {
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

        protected virtual void ChangePathBehaviour()
        {
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

        protected virtual void ReleaseAfterAtkBehaviour()
        {
            minRange = minRangeNoAtk;
            changeFollowingPath = true;
            passingTimeFollowing = 0f;
            timeBtwAttacks = startTimeBtwAttacks;
            attackRelease = true;
            FlashColorEffect(Color.white);
        }

        /// <summary>
        /// Evita salir de los bordes
        /// </summary>
        protected virtual void CheckBoundariesMovement()
        {
            Vector3 viewPos = transform.position;
            Vector2 screenBoundsMin = GameManager.instance.currentRoom.ScreenBoundsMin;
            Vector2 screenBoundsMax = GameManager.instance.currentRoom.ScreenBoundsMax;
            viewPos.x = Mathf.Clamp(viewPos.x, screenBoundsMin.x + objectWidth, screenBoundsMax.x);
            viewPos.y = Mathf.Clamp(viewPos.y, screenBoundsMin.y + objectHeight, screenBoundsMax.y);

            if (transform.position != viewPos)
            {
                changeFollowingPath = false;
                transform.position = viewPos;
            }
        }
        public void goRespawn()
        {
            //transform.position = Vector3.MoveTowards(transform.position, home)
        }

        public bool CheckIsDeath()
        {
            if (enemyCurrentHealth <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void UpdateRespawnPosition(Vector3 newPos)
        {
            this.respawnPosition = newPos;
        }

        public Vector3 GetRespawnPosition()
        {
            return this.respawnPosition;
        }

        public int GetAttack()
        {
            return enemyAttack;
        }
        public void TakeDamage(int damage, float knockbackDistance, float knockbackSpeed)
        {
            //TODO: animator trigger HURT
            enemyCurrentHealth -= damage;
            healthBar.SetHealth(enemyCurrentHealth);
            SoundManager.instance.PlaySingle(playerHit);
            FlashColorEffect(Color.red);
            passingTime = 0;
            CheckKknockback = true;
            kbDistance = knockbackDistance;
            kbSpeed = knockbackSpeed;
        }

        protected void FlashColorEffect(Color color)
        {
            flashDamageComponent.Flash(color);
        }
        public bool checkIsInmune()
        {
            if (enemyInmune)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void SetAnimatorMovement()
        {
            animator.SetFloat("moveX", (target.position.x - transform.position.x));
            animator.SetFloat("moveY", (target.position.y - transform.position.y));
        }
    }
}