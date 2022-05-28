using Assets.Scripts;
using Assets.Scripts.Components;
using UnityEngine;

public class eOrcShaman : Enemy
{

    public GameObject enemyShot;
    public Transform shotPoint;

    public float timeBtwShots;
    public float startTimeBtwShots;
    public float attackRange;
    public float webForce;

    //Components
    [SerializeField] ShootCastComponent shootComponent;

    public float offset;
    public LayerMask whatIsSolid;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
        enemyCurrentHealth = enemyMaxHealth;
        healthBar.SetMaxHealth(enemyMaxHealth);
        timeBtwShots = startTimeBtwShots;
        TypeEnemy = EnumTypeEnemies.mid;
        collider = this.GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    protected override void EnemyBehaviour()
    {
        MovementEnemyBehaviour();
      
    }

    protected override void MovementEnemyBehaviour()
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
            shootComponent.ShootBehaviour(ref timeBtwShots,attackRange,whatIsSolid,startTimeBtwShots,
                enemyShot,shotPoint, webForce);
        }
    }

    void OnDrawGizmos()
    {
        //Handles.color = Color.green;
        //Handles.DrawLine(transform.position, playerPosition);
    }
}
