using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected GameObject weapon;
    protected Animator weaponAnimator;
    protected SpriteRenderer weaponRenderer;
    protected BoxCollider2D weaponCollider;

    protected GameObject player;
    protected Animator playerAnimator;

    protected bool isAttacking = false;
    protected bool specialParryAttack = false;

    public AudioClip weaponSwin;
    [SerializeField] public float timeBtwAttack;
    [SerializeField] public float startTimeBtwAttack;

    public float moveX;
    public float moveY;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        //weapon = GameObject.FindGameObjectWithTag("Weapon");
        player = GameObject.FindGameObjectWithTag("Player");
        //weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        //weaponCollider = weapon.GetComponent<BoxCollider2D>();
        //weaponAnimator = weapon.GetComponent<Animator>();
        playerAnimator = player.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            GameObject enemyColl = other.gameObject;
            GameManager.instance.takeDamage(other.tag, other.gameObject.GetComponent<Enemy>());
            if (enemyColl.GetComponent<Enemy>().CheckIsDeath())
            {
                Destroy(enemyColl);
            }

        }
    }

    protected virtual void  ProcessInputs()
    {
        //Cooldown entre ataques para permitir spamear
        if (timeBtwAttack <= 0)
        {
            setDirectionAttack();
            if (Input.GetKey(KeyCode.Space))
            {
                if (specialParryAttack)
                {
                    SoundManager.instance.PlaySingle(weaponSwin);
                    isAttacking = true;
                    resetWeapon();
                    weaponAnimator.SetTrigger("Attacking");
                    timeBtwAttack = startTimeBtwAttack;
                    //Debug.Log("ataque arma!");
                    SpecialAttack();
                    specialParryAttack = false;
                }
                else
                {
                    SoundManager.instance.PlaySingle(weaponSwin);
                    isAttacking = true;
                    resetWeapon();
                    weaponAnimator.SetTrigger("Attacking");
                    timeBtwAttack = startTimeBtwAttack;
                    Debug.Log("ataque arma!");
                }
               
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

    internal abstract void ActiveSpecialParryAtk();

    /// <summary>
    /// Semaforo de activar o desactivar el arma
    /// </summary>
    protected void resetWeapon()
    {
        if (isAttacking)
        {
            weaponCollider.isTrigger = true;
            weaponCollider.enabled = true;
            //weaponRenderer.enabled = true;
        }
        else
        {
            weaponCollider.isTrigger = false;
            weaponCollider.enabled = false;
            //weaponRenderer.enabled = false;
            //weapon.transform.rotation = new Quaternion(0,0,0,0);//ToDo: creo que esto quedó como parche de algo mal generado en las animaciones
        }
    }

    /// <summary>
    /// Carga las variables para el arbol de animaciones de los ataques
    /// </summary>
    protected virtual void setDirectionAttack()
    {
        moveX = playerAnimator.GetFloat("moveX");
        moveY = playerAnimator.GetFloat("moveY");

        weaponAnimator.SetFloat("moveX", moveX);
        weaponAnimator.SetFloat("moveY", moveY);
    }

    public virtual void setIsAttacking()
    {
        isAttacking = true;
    }

    public virtual void SpecialAttack()
    {
        Debug.Log(" ataque especial por defecto.");
    }
}

 