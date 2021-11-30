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

    protected int damage = 1;
    protected float knockbackDistance = 1;// fuerza y cantidad de desplazamiento
    protected float knockbackSpeed = 1;//velocidad a la que ocurre el empuje

    public AudioClip weaponSwin;
    [SerializeField] public float timeBtwAttack;
    [SerializeField] public float startTimeBtwAttack;

    public float moveX;
    public float moveY;
    [SerializeField] private Sprite weaponSprite;

    public Sprite WeaponSprite { get => weaponSprite; set => weaponSprite = value; }

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
        if (other.tag == "Enemy" && (!other.gameObject.GetComponent<Enemy>().checkIsInmune()))
        {
            GameObject enemyColl = other.gameObject;
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            GameManager.instance.takeDamage(other.tag, other.gameObject.GetComponent<Enemy>(),damage, knockbackDistance, knockbackSpeed);
            //if (enemyColl.GetComponent<Enemy>().CheckIsDeath())
            //{
            //    GameManager.instance.DestroyEnemy(enemyColl.GetComponent<Enemy>());
                
            //}
            //else
            //{
            //    //other.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            //}

        }
    }

    protected virtual void  ProcessInputs()
    {
        //Cooldown entre ataques para permitir spamear
        if (timeBtwAttack <= 0)
        {
            setDirectionAttack();
            //if (Input.GetKey(KeyCode.Space))
            //{
            //    if (specialParryAttack)
            //    {
            //        SoundManager.instance.PlaySingle(weaponSwin);
            //        isAttacking = true;
            //        weaponAnimator.SetTrigger("Counter");
            //        timeBtwAttack = startTimeBtwAttack;
            //        SpecialAttack();
            //        //specialParryAttack = false;
                   
            //    }
            //    else
            //    {
            //        SoundManager.instance.PlaySingle(weaponSwin);
            //        isAttacking = true;
            //        weaponAnimator.SetTrigger("Attacking");
            //        timeBtwAttack = startTimeBtwAttack;
            //    }
            //}
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
        //resetWeapon();
        
        if (specialParryAttack)
        {
            weaponAnimator.SetTrigger("Counter");
            DisableSpecialParryAtk();
            GameManager.instance.player.DisableParryAttack();
        }
        else
        {
            weaponAnimator.SetTrigger("Attacking");
        }
    }

  

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
        moveX = player.GetComponent<Player>().AttackPosition.x; // playerAnimator.GetFloat("moveX");
        moveY = player.GetComponent<Player>().AttackPosition.y;

        weaponAnimator.SetFloat("moveX", moveX);
        weaponAnimator.SetFloat("moveY", moveY);
    }

    public virtual void setIsAttacking()
    {
        isAttacking = true;
    }

    public virtual void EnableColliderAttack()
    {
        weaponCollider.isTrigger = true;
        weaponCollider.enabled = true;
    }
    public virtual void DisableColliderAttack()
    {
        weaponCollider.isTrigger = false;
        weaponCollider.enabled = false;
    }

    public virtual void SpecialAttack()
    {
        Debug.Log(" ataque especial por defecto.");
        
    }

    public virtual void ActiveSpecialParryAtk()
    {
        specialParryAttack = true;
    }
    public virtual void DisableSpecialParryAtk()
    {
        specialParryAttack = false;
    }
}

 