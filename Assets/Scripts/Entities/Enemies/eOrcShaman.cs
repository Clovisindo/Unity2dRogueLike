using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class eOrcShaman : Enemy
{

    public GameObject enemyShot;
    public Transform shotPoint;

    public float timeBtwShots;
    public float startTimeBtwShots;
    public float attackRange;
    public float webForce;

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
            ShootBehaviour();
        }
    }

    private void ShootBehaviour()
    {
        if (timeBtwShots <= 0)
        {
            RaycastHit2D hitVision = RaycastVisionToPlayer();
            //Debug.DrawRay(transform.position + (directionRay * 1), directionRay * attackRange, Color.green, 0.1f);
            CheckCollisionAndShoot(hitVision);
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    private RaycastHit2D RaycastVisionToPlayer()
    {
        Vector3 directionRay = (GameManager.instance.player.transform.position - transform.position).normalized;
        return Physics2D.Raycast(transform.position + (directionRay * 1), directionRay, attackRange, whatIsSolid);

    }

    private void CheckCollisionAndShoot(RaycastHit2D hitVision)
    {
        if (hitVision.collider != null)
        {
            if (hitVision.collider.CompareTag("Player"))
            {
                timeBtwShots = startTimeBtwShots;
                shotPoint.rotation = CalculateRotationToTarget();
                InstantiateShoot();
            }
        }
    }

    private Quaternion CalculateRotationToTarget()
    {
        Vector3 difference = (transform.position - GameManager.instance.player.transform.position);
        float rotZ = (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg);
       return Quaternion.Euler(0f, 0f, rotZ + 90);
    }

    private void InstantiateShoot()
    {
        GameObject shot = Instantiate(enemyShot, shotPoint.position, shotPoint.rotation);
        shot.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0f, webForce, 0f));
    }

    void OnDrawGizmos()
    {
        //Handles.color = Color.green;
        //Handles.DrawLine(transform.position, playerPosition);
    }
}
