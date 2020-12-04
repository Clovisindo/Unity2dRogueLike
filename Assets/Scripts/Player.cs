using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private Animator animator;
    private Transform transform;

    private Vector2 movementDirection;
    private float movementSpeed;
    public float moveX;
    public float moveY;

    public float MOVEMENT_BASE_SPEED = 3.0f;
    public int playerHealth = 3;
    private const float inmuneTime = 2.0f;
    private float passingTime = inmuneTime;
    private bool playerInmune = false;
    public bool playerExitCollision = false;
    public bool playerEntranceCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        ProcessInputs();
        animator.SetFloat("movementSpeed", movementSpeed);
        Move();
    }

    void ProcessInputs()
    {
       moveX = Input.GetAxisRaw("Horizontal");
       moveY = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(moveX, moveY);

        animator.SetFloat("moveX", moveX);
        animator.SetFloat("moveY", moveY);

        movementSpeed = Mathf.Clamp(movementDirection.magnitude, 0.0f, 1.0f);
        movementDirection.Normalize();

        if (passingTime < inmuneTime)
        {
            passingTime += Time.deltaTime;
            playerInmune = true;
        }
        else
        {
            playerInmune = false;
        }
        
    }

    protected void Move()
    {
        rb2D.velocity = movementDirection * movementSpeed * MOVEMENT_BASE_SPEED;
    }

    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "Enemy"  && !playerInmune)
        {
            GameObject enemyColl = other.gameObject;
            animator.SetTrigger("Hurt");
            //restamos vida al jugador
            SetPlayerHealth(- enemyColl.GetComponent<Enemy>().GetAttack());
            UpdatePlayerHealth();
            passingTime = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && !playerInmune)
        {
            GameObject enemyColl = other.gameObject;
            animator.SetTrigger("Hurt");
            //restamos vida al jugador
            SetPlayerHealth(- enemyColl.GetComponent<Enemy>().GetAttack());
            UpdatePlayerHealth();
            passingTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit" && playerExitCollision == false)
        {
            playerExitCollision = true;
            GameManager.instance.ChangeLevel(false);
            
        }

        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Entrance" && playerExitCollision == false)
        {
            playerExitCollision = true;
            GameManager.instance.ChangeLevel(true);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit" )
        {
            //GameManager.instance.currentRoom.EnableChangeEventColliderExitRoom();
            playerExitCollision = false;
        }

        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Entrance" )
        {
            //GameManager.instance.currentRoom.EnableChangeEventColliderEntranceRoom();
            playerExitCollision = false;
        }
    }

        private void SetPlayerHealth(int modifyHealth)
    {
        playerHealth += modifyHealth;
    }

    public void UpdatePlayerHealth()
    {
       HealthManager.instance.UpdateUI(playerHealth);
    }

    public void UpdatePositionlevel(Vector2 respawnPosition)
    {
        transform.position = respawnPosition;
        //playerExitCollision = false;
    }



}
