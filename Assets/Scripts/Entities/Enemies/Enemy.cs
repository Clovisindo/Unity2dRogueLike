using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.EnumTypes;
using System;
using Assets.Scripts.Components;

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
    protected  int enemyAttack;
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
    public bool changeFollowingPath = false;

    //Enemigos habitacion
    public EnumTypeEnemies TypeEnemy { get => typeEnemy; set => typeEnemy = value; }
    public bool IsPaused { get => isPaused; set => isPaused = value; }

    protected virtual void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        collider = this.GetComponent<BoxCollider2D>();

        //screenBoundsMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        //screenBoundsMin = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z));
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

    protected virtual Vector3 CheckBoundariesMovement()
    {
        Vector3 viewPos = transform.position;
        Vector2 screenBoundsMin = GameManager.instance.currentRoom.ScreenBoundsMin;
        Vector2 screenBoundsMax = GameManager.instance.currentRoom.ScreenBoundsMax;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBoundsMin.x + objectWidth , screenBoundsMax.x);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBoundsMin.y + objectHeight  , screenBoundsMax.y );
        
        if (transform.position != viewPos)
        {
            changeFollowingPath = false;
            return viewPos;
        }
        else
        {
            return Vector3.zero;
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
        SoundManager.instance.PlaySingle(playerHit);
        flashDamageComponent.Flash(Color.white);
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
