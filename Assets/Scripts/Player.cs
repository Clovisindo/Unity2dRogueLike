using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private Animator animator;

    private Vector2 movementDirection;
    private float movementSpeed;
    public float moveX;
    public float moveY;

    public float MOVEMENT_BASE_SPEED = 3.0f;
    public int playerHealth = 3;
    private const float inmuneTime = 2.0f;
    private float passingTime = inmuneTime;
    private bool playerInmune = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

    private void SetPlayerHealth(int modifyHealth)
    {
        playerHealth += modifyHealth;
    }

    public void UpdatePlayerHealth()
    {
       HealthManager.instance.UpdateUI(playerHealth);
    }



}
