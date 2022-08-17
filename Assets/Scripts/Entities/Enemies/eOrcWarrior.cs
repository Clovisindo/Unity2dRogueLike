using Assets.Scripts;
using Assets.Scripts.Components;
using UnityEngine;

namespace Assets.Scripts.Entities.Enemies
{
    public class eOrcWarrior : Enemy
    {
        public float timeBtwCharge;
        public float startTimeBtwCharge;

        public override string name => "eOrcWarrior";

        //Components
        [SerializeField] ShootCastComponent shootComponent;

        public float attackRange;
        [SerializeField] public const float ChargeForce = 3;
        private int layerMaskWall;

        public LayerMask whatIsSolid;
        public bool isCharging = false;
        [SerializeField] private Vector3 directionCharge;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            target = FindObjectOfType<Player>().transform;
            enemyCurrentHealth = enemyMaxHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);
            layerMaskWall = LayerMask.NameToLayer("Wall");
            collider = this.GetComponent<BoxCollider2D>();
            rb = this.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        protected override void EnemyBehaviour()
        {
            MovementEnemyBehaviour();
            ChargeBehaviour();
        }

        private void ChargeBehaviour()
        {
            if (timeBtwCharge <= 0 && !isCharging)
            {
                var directionAndRayCast = shootComponent.EnemyAimWithDirection(attackRange, whatIsSolid);
                shootComponent.CheckCollisionAndCharge(directionAndRayCast.HitVision, ref timeBtwCharge, startTimeBtwCharge,
                    directionAndRayCast.DirectionCast, ref directionCharge, ref isCharging);
            }
            else
            {
                timeBtwCharge -= Time.deltaTime;
            }

            if (isCharging)
            {
                this.GetComponent<Rigidbody2D>().AddRelativeForce(directionCharge * ChargeForce);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            //Check if the tag of the trigger collided with is Exit.
            if (other.gameObject.layer == layerMaskWall && isCharging)
            {
                isCharging = false;
                this.GetComponent<Rigidbody2D>().AddRelativeForce(directionCharge * ChargeForce * -1);
            }
            if (other.gameObject.tag == "Player" && isCharging)
            {
                isCharging = false;
                this.GetComponent<Rigidbody2D>().AddRelativeForce(directionCharge * ChargeForce * -1);
            }
        }
    }
}
