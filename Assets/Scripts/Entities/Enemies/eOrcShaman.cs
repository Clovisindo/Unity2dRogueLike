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
        animator = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
        enemyCurrentHealth = enemyMaxHealth;
        healthBar.SetMaxHealth(enemyMaxHealth);
        timeBtwShots = startTimeBtwShots;
        TypeEnemy = EnumTypeEnemies.mid;
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

        if (passingTime < inmuneTime)
        {
            passingTime += Time.deltaTime;
            enemyInmune = true;
        }
        else
        {
            enemyInmune = false;
        }

        // shooting enemy
        if (timeBtwShots <= 0)
        {
            //Raycast si hay vision
            Vector3 directionRay = (GameManager.instance.player.transform.position - transform.position).normalized;
            RaycastHit2D hitVision = Physics2D.Raycast(transform.position + (directionRay * 1), directionRay, attackRange, whatIsSolid);

            //debug
            Debug.DrawRay(transform.position + (directionRay * 1), directionRay * attackRange, Color.green, 0.1f);

            if (hitVision.collider != null)
            {
                if (hitVision.collider.CompareTag("Player"))
                {
                    timeBtwShots = startTimeBtwShots;
                    // control para girar el punto de disparo hacia el jugador
                    Vector3 difference =  (transform.position - GameManager.instance.player.transform.position);
                    float rotZ = (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg);
                    shotPoint.rotation = Quaternion.Euler(0f, 0f, rotZ + 90);

                    GameObject shot = Instantiate(enemyShot, shotPoint.position,shotPoint.rotation);
                    shot.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0f, webForce, 0f));
                    
                }
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        ////debug dibujar las fintas y direccion del muñeco
        //Handles.color = Color.green;
        //Handles.DrawLine(transform.position, playerPosition);
    }
}
