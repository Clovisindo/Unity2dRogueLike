using Assets.Scripts.Entities.Enemies;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected GameObject weapon;
    protected Animator weaponAnimator;
    protected SpriteRenderer weaponRenderer;
    protected BoxCollider2D weaponCollider;

    protected GameObject player;
    protected Animator playerAnimator;

    private bool isAttacking = false;
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

    //inputactions
    protected Playerinputactions inputAction;
    protected bool attackWeaponPressed;
    //attack action
    protected Vector2 attackPosition;
    private bool firstAttack = false;

    public Vector2 AttackPosition { get => attackPosition; set => attackPosition = value; }
    public Sprite WeaponSprite { get => weaponSprite; set => weaponSprite = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool FirstAttack { get => firstAttack; set => firstAttack = value; }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerAnimator = player.GetComponent<Animator>();

        //action buttons
        inputAction = new Playerinputactions();
        inputAction.Playercontrols.AttackDirection.performed += ctx => AttackPosition = ctx.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProcessInputs();
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && (!other.gameObject.GetComponent<Enemy>().checkIsInmune()))
        {
            GameObject enemyColl = other.gameObject;
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            GameManager.instance.takeDamage(other.tag, other.gameObject.GetComponent<Enemy>(), damage, knockbackDistance, knockbackSpeed);
        }
    }

    protected virtual void ProcessInputs()
    {
        //Cooldown entre ataques para permitir spamear
        if (timeBtwAttack <= 0 && !CheckWeaponAttacking())
        {
            setDirectionAttack();
            if (attackWeaponPressed)
            {
                if (specialParryAttack)
                {
                    SoundManager.instance.PlaySingle(weaponSwin);
                    IsAttacking = true;
                    GameManager.instance.player.CurrentWeaponAttacking = true;
                    weaponAnimator.SetTrigger("Counter");
                    timeBtwAttack = startTimeBtwAttack;
                    SpecialAttack();
                }
                else
                {
                    SoundManager.instance.PlaySingle(weaponSwin);
                    IsAttacking = true;
                    GameManager.instance.player.CurrentWeaponAttacking = true;
                    weaponAnimator.SetTrigger("Attacking");
                    timeBtwAttack = startTimeBtwAttack;
                }
                attackWeaponPressed = false;
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
        IsAttacking = false;
        GameManager.instance.player.CurrentWeaponAttacking = false;
        firstAttack = true;

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

    public virtual bool CheckIsIddleAnim()
    {
        return weaponAnimator.GetCurrentAnimatorStateInfo(0).IsTag("idleAnim") 
            && !(weaponAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
    }

    /// <summary>
    /// Carga las variables para el arbol de animaciones de los ataques
    /// </summary>
    protected virtual void setDirectionAttack()
    {
        moveX = attackPosition.x; // playerAnimator.GetFloat("moveX");
        moveY = attackPosition.y;

        weaponAnimator.SetFloat("moveX", moveX);
        weaponAnimator.SetFloat("moveY", moveY);
        if (moveX != 0 || moveY != 0)
        {
            attackWeaponPressed = true;
        }
        else
        {
            attackWeaponPressed = false;
        }

    }

    public virtual void setIsAttacking()
    {
        IsAttacking = true;
        GameManager.instance.player.CurrentWeaponAttacking = true;
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

    public virtual bool CheckWeaponAttacking()
    {
        return GameManager.instance.player.CurrentWeaponAttacking;
    }

    private void OnEnable()
    {
        inputAction.Enable();
    }
    private void OnDisable()
    {
        inputAction.Disable();
    }
}

