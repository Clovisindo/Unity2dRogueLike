using UnityEngine;

public class wKnightShield : Weapon
{
    bool parryBehaviour = false;
    Player playerClass;


    private float timeBtwBlocks;
    private float startTimeBtwBlocks = 1f;
    //private bool specialParryAttack = false;
    bool EquipShieldPressed;


    public wKnightShield()
    {
        isAttacking = false;
        startTimeBtwAttack = 1.383f;
    }

    protected override void Awake()
    {
        weapon = GameObject.FindGameObjectWithTag("KnightShield");
        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponCollider = weapon.GetComponent<BoxCollider2D>();
        weaponAnimator = weapon.GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerClass = player.GetComponent<Player>();
        playerAnimator = player.GetComponent<Animator>();

        //action buttons
        inputAction = new Playerinputactions();
        inputAction.Playercontrols.AttackDirection.performed += ctx => AttackPosition = ctx.ReadValue<Vector2>();
        inputAction.Playercontrols.Attack.performed += ctx => attackWeaponPressed = true;
        //inputAction.Playercontrols.EquipShield.performed += ctx => EquipShieldPressed = true;
        //inputAction.Playercontrols.EquipShield.canceled += ctx => EquipShieldPressed = false;
    }

    protected override void ProcessInputs()
    {
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy" && isAttacking && !parryBehaviour)
        {
            Parry();
        }
    }

    void FixedUpdate()
    {
     //activar bloqueo del escudo
        if (timeBtwBlocks <= 0)
        {
            setDirectionAttack();
            if (attackWeaponPressed)
            {
                setIsAttacking();
                //player.GetComponent<Player>().EquipShieldBlock();
                timeBtwBlocks = startTimeBtwBlocks;
                //EquipShieldPressed = false;
            }
            attackWeaponPressed = false;
        }
        else
        {
            timeBtwBlocks -= Time.deltaTime;
        }
    }

    protected override void setDirectionAttack()
    {
        moveX = attackPosition.x;
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

    //public  void Block()
    //{
    //    Debug.Log("Bloqueas el ataque.");
    //    //weaponAnimator.SetTrigger("SpecialAttack");
    //}

    public void Parry()
    {
        ActiveParryBehaviour();
        Debug.Log("Devuelve el golpe!");
        //weaponAnimator.SetTrigger("SpecialAttack");
        //activamos el ataque especial del jugador cuando hace parry
        playerClass.ActiveSpecialParryAtk();
    }

    public override void setIsAttacking()
    {
        weaponAnimator.SetTrigger("Attacking");
        isAttacking = true;
    }

    public void ActiveParryBehaviour()
    {
        parryBehaviour = true;
    }
    public void DisableParryBehaviour()
    {
        parryBehaviour = false;
    }

    /// <summary>
    /// al acabar la animacion desactiva el escudo
    /// </summary>
    public void UnequipShield()
    {
        //weaponAnimator.SetTrigger("Attacking");
        isAttacking = false;
        //playerClass.UnEquipShieldBlock();
    }

    public override void ActiveSpecialParryAtk()
    {
        specialParryAttack = true;
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
