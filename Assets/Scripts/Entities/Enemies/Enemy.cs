using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.EnumTypes;
using System;

public abstract class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected Transform target;

    //Override properties
    [SerializeField]
    protected float speed = 0.5f;
    [SerializeField]
    protected float minRange;
    [SerializeField]
    protected  float maxRange;
    [SerializeField]
    private EnumTypeEnemies typeEnemy;
    protected Rigidbody2D rb;
    protected BoxCollider2D collider;


    //health
    [SerializeField]
    protected int enemyMaxHealth;
    [SerializeField]
    protected int enemyCurrentHealth;
    [SerializeField]
    protected HealthBar healthBar;

    //Attack
    [SerializeField]
    protected  int enemyAttack;

    //movement
    protected bool isMoving = false;
    protected float moveX;
    protected float moveY;

    protected Vector2 positionEndKnockback = default;

    private float distanceKnockback = 0;
    private float kbDistance = 1;
    private float kbSpeed = 1;

    //const stats
    protected const float inmuneTime = 2.0f;
    protected float knockbackResistence = 1;
    protected float passingTime = inmuneTime;
    protected bool enemyInmune = false;
    private bool isPaused = false;
    private bool CheckKknockback = false;

    //Enemigos habitacion
    public EnumTypeEnemies TypeEnemy { get => typeEnemy; set => typeEnemy = value; }
    public bool IsPaused { get => isPaused; set => isPaused = value; }

    protected virtual void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        collider = this.GetComponent<BoxCollider2D>();
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
       
        if (positionEndKnockback == default){positionEndKnockback = new Vector3(transform.position.x + difference.x, transform.position.y + difference.y);}

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
    public void TakeDamage(int damage, float knockbackDistance, float knockbackSpeed)
    {
        //TODO: animator trigger HURT
        enemyCurrentHealth -= damage;
        healthBar.SetHealth(enemyCurrentHealth);
        passingTime = 0;
        CheckKknockback = true;
        kbDistance = knockbackDistance;
        kbSpeed = knockbackSpeed;
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
