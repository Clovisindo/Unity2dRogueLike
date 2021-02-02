using UnityEditor;
using UnityEngine;

public class eOrcMasked : Enemy
{
    // for following random pattern
    Vector3 playerPosition;

    float randMoveX;
    float randMoveY;
    
    const float totalTimeFollowing = 1f;
    float passingTimeFollowing = totalTimeFollowing;
    bool changeFollowingPath = false;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
        enemyCurrentHealth = enemyMaxHealth;
        healthBar.SetMaxHealth(enemyMaxHealth);
    }

    // Update is called once per frame
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

        //Fintar a los lados ToDo:
        if (passingTimeFollowing < totalTimeFollowing)
        {
            passingTimeFollowing += Time.deltaTime;
            changeFollowingPath = false;
        }
        else
        {
            changeFollowingPath = true;
        }
    }

    protected override void FollowPlayer()
    {
        animator.SetFloat("moveX", (target.position.x - transform.position.x));// esto para devolver a la animacion donde mirar??
        animator.SetFloat("moveY", (target.position.y - transform.position.y));

        

        if (changeFollowingPath)
        {
            playerPosition = target.transform.position;
            GenerateRandomMove( ref playerPosition);
        }
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
    }

    private void GenerateRandomMove(ref Vector3  playerPosition)
    {
        randMoveX = Random.Range(0f, 0.8f);
        randMoveY = Random.Range(0f, 0.8f);
        playerPosition.x += randMoveX;
        playerPosition.y += randMoveY;
        changeFollowingPath = false;
        passingTimeFollowing = 0f;
        //check no salirse de los bordes
    }

    void OnDrawGizmos()
    {
        //debug dibujar las fintas y direccion del muñeco
        Handles.color = Color.green;
        Handles.DrawLine(transform.position, playerPosition);
    }

    private void CheckNextPositionBoundary( Vector3 _nextPosition)
    {
        GameManager.instance.CheckInsideBoundaries(_nextPosition);//ToDo:
    }
}
