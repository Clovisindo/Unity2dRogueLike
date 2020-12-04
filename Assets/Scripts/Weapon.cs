using UnityEngine;

public class Weapon : MonoBehaviour
{
    private GameObject weapon;
    private Animator weaponAnimator;
    private SpriteRenderer weaponRenderer;
    private BoxCollider2D weaponCollider;

    private GameObject player;
    private Animator playerAnimator;

    private bool isAttacking = false;

    public AudioClip weaponSwin;
    public float timeBtwAttack;
    public float startTimeBtwAttack;

    public float moveX;
    public float moveY;

    // Start is called before the first frame update
    void Start()
    {
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        player = GameObject.FindGameObjectWithTag("Player");
        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponCollider = weapon.GetComponent<BoxCollider2D>();
        weaponAnimator = weapon.GetComponent<Animator>();
        playerAnimator = player.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            GameObject enemyColl = other.gameObject;
            GameManager.instance.takeDamage(other.tag);
            if (enemyColl.GetComponent<Enemy>().CheckIsDeath())
            {
                Destroy(enemyColl);
            }

        }
    }

    void ProcessInputs()
    {
        //Cooldown entre ataques para permitir spamear
        if (timeBtwAttack <= 0)
        {
            setDirectionAttack();
            if (Input.GetKey(KeyCode.Space))
            {
                SoundManager.instance.PlaySingle(weaponSwin);
                isAttacking = true;
                resetWeapon();
                weaponAnimator.SetTrigger("Attacking");
                timeBtwAttack = startTimeBtwAttack;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Se llama desde el gestor de animaciones
    /// </summary>
    void EndAnimation()
    {
        isAttacking = false;
        resetWeapon();
        weaponAnimator.SetTrigger("Attacking");

    }

    /// <summary>
    /// Semaforo de activar o desactivar el arma
    /// </summary>
    void resetWeapon()
    {
        if (isAttacking)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = true;
            weaponRenderer.enabled = true;
        }
        else
        {
            weaponCollider.isTrigger = false;
            weaponCollider.enabled = false;
            weaponRenderer.enabled = false;
            weapon.transform.rotation = new Quaternion(0,0,0,0);
        }
    }

    /// <summary>
    /// Carga las variables para el arbol de animaciones de los ataques
    /// </summary>
    void setDirectionAttack()
    {
        moveX = playerAnimator.GetFloat("moveX");
        moveY = playerAnimator.GetFloat("moveY");

        weaponAnimator.SetFloat("moveX", moveX);
        weaponAnimator.SetFloat("moveY", moveY);
    }
}