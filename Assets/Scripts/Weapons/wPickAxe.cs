using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class wPickAxe : Weapon
    {
        public bool pickAxeCollision = false;
        private int pickAxeUses = 3;


        public wPickAxe()
        {
            startTimeBtwAttack = 1.383f;
        }

        protected override void Awake()
        {
            weapon = GameObject.FindGameObjectWithTag("PickAxe");
            weaponRenderer = weapon.GetComponent<SpriteRenderer>();
            weaponCollider = weapon.GetComponent<BoxCollider2D>();
            weaponAnimator = weapon.GetComponent<Animator>();

            player = GameObject.FindGameObjectWithTag("Player");
            playerAnimator = player.GetComponent<Animator>();
            HealthManager.instance.UpdateEquipUses(pickAxeUses);
            //action buttons
            inputAction = new Playerinputactions();
            inputAction.Playercontrols.AttackDirection.performed += ctx => AttackPosition = ctx.ReadValue<Vector2>();
            inputAction.Playercontrols.Attack.performed += ctx => attackWeaponPressed = true;
        }

        protected override void ProcessInputs()
        {
            //Cooldown entre ataques para permitir spamear
            if (timeBtwAttack <= 0 && !CheckWeaponAttacking() && CheckIsIddleAnim())
            {
                setDirectionAttack();
                if (attackWeaponPressed && pickAxeUses > 0)
                {
                    SoundManager.instance.PlaySingle(weaponSwin);
                    //isAttacking = true;
                    //resetWeapon();
                    setIsAttacking();
                    weaponAnimator.SetTrigger("Attacking");
                    timeBtwAttack = startTimeBtwAttack;
                    pickAxeUses--;
                    HealthManager.instance.UpdateEquipUses(pickAxeUses);
                }
                attackWeaponPressed = false;
            }
            else
            {
                timeBtwAttack -= Time.deltaTime;
            }
        }

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Door" && (other.gameObject.GetComponentInParent<FRoomDoor>(true)!= null))
            {
                FRoomDoor secretDoorobj = other.gameObject.GetComponentInParent<FRoomDoor>(true);

                if (secretDoorobj.IsSecretDoor && secretDoorobj.IsClosed && !pickAxeCollision)
                {
                    pickAxeCollision = true;
                    GameManager.instance.OpenSecretDoor(secretDoorobj, other);
                }
            }
        }
        public override void SpecialAttack()
        {
            Debug.Log("Picar la pared con el martillo.");
            weaponAnimator.SetTrigger("SpecialAttack");
        }

        public override void ActiveSpecialParryAtk()
        {
            //specialParryAttack = true;
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
}
