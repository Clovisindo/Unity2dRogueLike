using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class wPickAxe : Weapon
    {
        public bool pickAxeCollision = false;

        public wPickAxe()
        {
            startTimeBtwAttack = 1.383f;
        }

        void Start()
        {
            weapon = GameObject.FindGameObjectWithTag("PickAxe");
            weaponRenderer = weapon.GetComponent<SpriteRenderer>();
            weaponCollider = weapon.GetComponent<BoxCollider2D>();
            weaponAnimator = weapon.GetComponent<Animator>();

            player = GameObject.FindGameObjectWithTag("Player");
            playerAnimator = player.GetComponent<Animator>();
        }

        protected override void ProcessInputs()
        {
            //Cooldown entre ataques para permitir spamear
            if (timeBtwAttack <= 0)
            {
                setDirectionAttack();
                if (Input.GetKey(KeyCode.C))
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
                        //Debug.Log("ataque arma!");
                    }
                }
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

        internal override void ActiveSpecialParryAtk()
        {
            specialParryAttack = true;
        }




    }
}
