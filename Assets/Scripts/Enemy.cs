using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private Transform target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float minRange;
    [SerializeField]
    private float maxRange;

    

    //health
    public int enemyMaxHealth;
    public int enemyCurrentHealth;
    public HealthBar healthBar;

    //Attack
    public const int enemyAttack = 1;

    //movement
    public float MOVEMENT_BASE_SPEED = 3.0f;
    private bool isMoving = false;
    private float moveX;
    private float moveY;

    //const stats
    private const float inmuneTime = 2.0f;
    private float passingTime = inmuneTime;
    private bool enemyInmune = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
        enemyCurrentHealth = enemyMaxHealth;
        healthBar.SetMaxHealth(enemyMaxHealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(target.position , transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) >= minRange)
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

    public void FollowPlayer()
    {
        animator.SetFloat("moveX", (target.position.x - transform.position.x));// esto para devolver a la animacion donde mirar??
        animator.SetFloat("moveY", (target.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
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
          return  false;
        }
    }

    public int GetAttack()
    {
        return enemyAttack;
    }
    public void TakeDamage(int damage)
    {
        //TODO: animator trigger HURT
        enemyCurrentHealth -= damage;
        healthBar.SetHealth(enemyCurrentHealth);
        passingTime = 0;
    }
    public bool checkIsInmune()
    {
        if (enemyInmune){
            return true;
        }
        else{
            return false;
        }
    }
    
}
