using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected Transform target;

    //Override properties
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float minRange;
    [SerializeField]
    protected  float maxRange;

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

    //const stats
    protected const float inmuneTime = 2.0f;
    protected float passingTime = inmuneTime;
    protected bool enemyInmune = false;

    protected abstract void Awake();

    // Update is called once per frame
    protected abstract void FixedUpdate();

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
