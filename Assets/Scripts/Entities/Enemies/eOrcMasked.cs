#if UNITY_EDITOR
using UnityEditor;
#endif
using Assets.Scripts;
using UnityEngine;

public class eOrcMasked : Enemy
{
    Vector3 playerPosition;

    public override string name => "eOrcMasked";


    float randMoveX;
    float randMoveY;
    new const float totalTimeFollowing = 1f;
    private const float totalTimeWalking = 1f;
    private float passingTimeWalking = 0;
    private bool walkingMode = true;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
        enemyCurrentHealth = enemyMaxHealth;
        healthBar.SetMaxHealth(enemyMaxHealth);
        collider = this.GetComponent<BoxCollider2D>();
        rb = this.GetComponent<Rigidbody2D>();

        passingTimeFollowing = 0;
        changeFollowingPath = false;
    }

    // Update is called once per frame
    protected override void EnemyBehaviour()
    {
        MovementEnemyBehaviour();
    }

    protected override void MovementEnemyBehaviour()
    {
        base.MovementEnemyBehaviour();
        WalkingMovement();
        if (changeFollowingPath)
        {
            ChangePathMovement();
        }
    }

    private void WalkingMovement()
    {
        if (walkingMode)
        {
            if (passingTimeWalking < totalTimeWalking)
            {
                passingTimeWalking += Time.deltaTime;
            }
            else
            {
                changeFollowingPath = true;
                walkingMode = false;
                playerPosition = target.transform.position;
                GenerateRandomMove(ref playerPosition);
            }
        }
    }

    private void ChangePathMovement()
    {
        if (passingTimeFollowing < totalTimeFollowing)
        {
            passingTimeFollowing += Time.deltaTime;
        }
        else
        {
            changeFollowingPath = false;
            walkingMode = true;
            passingTimeWalking = 0;
            passingTimeFollowing = 0;
        }
    }

    protected override void FollowPlayer()
    {
        SetAnimatorMovement();

        if (!changeFollowingPath)
        {
            UpdateTargetPosition();
        }
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, speed * Time.deltaTime);
    }

    private void UpdateTargetPosition()
    {
        playerPosition = target.transform.position;
    }

    private void GenerateRandomMove(ref Vector3  playerPosition)
    {
        randMoveX = Random.Range(0f, 3f);
        randMoveY = Random.Range(0f,3f);
        playerPosition.x += randMoveX;
        playerPosition.y += randMoveY;
        CheckBoundariesMovement();
    }

    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        //debug dibujar las fintas y direccion del muñeco
        Handles.color = Color.green;
        Handles.DrawLine(transform.position, playerPosition);
        #endif
    }
}
