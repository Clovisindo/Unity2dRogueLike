using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eOrcWarrior : Enemy
{
    public float timeBtwCharge;
    public float startTimeBtwCharge;

    public float attackRange;
    [SerializeField] public const float ChargeForce = 3;
    private int layerMaskWall;

    public LayerMask whatIsSolid;
    public bool isCharging = false;
    [SerializeField] private Vector3 directionCharge;

    // Start is called before the first frame update
    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        target = FindObjectOfType<Player>().transform;
        enemyCurrentHealth = enemyMaxHealth;
        healthBar.SetMaxHealth(enemyMaxHealth);
        layerMaskWall = LayerMask.NameToLayer("Wall");
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) >= minRange && !isCharging)
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

        //cd de la carga
        if (timeBtwCharge <= 0 && !isCharging)
        {
            //raycast si hay vision del jugador
            Vector3 directionRay = (GameManager.instance.player.transform.position - transform.position).normalized;
            RaycastHit2D hitVision = Physics2D.Raycast(transform.position + (directionRay * 1), directionRay, attackRange, whatIsSolid);

            //debug
            Debug.DrawRay(transform.position + (directionRay * 1), directionRay * attackRange, Color.green, 0.1f);

            if (hitVision.collider != null)
            {
                if (hitVision.collider.CompareTag("Player"))
                {
                    timeBtwCharge = startTimeBtwCharge;
                    directionCharge = directionRay;
                    isCharging = true;
                }
            }
        }
        else
        {
            timeBtwCharge -= Time.deltaTime;
        }

        if (isCharging)
        {
            this.GetComponent<Rigidbody2D>().AddRelativeForce(directionCharge * ChargeForce);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.gameObject.layer == layerMaskWall && isCharging)
        {
            isCharging = false;
            this.GetComponent<Rigidbody2D>().AddRelativeForce(directionCharge * ChargeForce * -1);
        }
        if (other.gameObject.tag == "Player" && isCharging)
        {
            isCharging = false;
            this.GetComponent<Rigidbody2D>().AddRelativeForce(directionCharge * ChargeForce * -1);
        }
    }
}
