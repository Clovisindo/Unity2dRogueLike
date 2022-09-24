using Assets.Scripts.EnumTypes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class ShootCastComponent : MonoBehaviour
    {
        private bool shoot = false;

        public bool ShootBehaviour(ref float timeBtwShots, float attackRange, LayerMask whatIsSolid, float startTimeBtwShots, GameObject enemyShot,
            Transform shotPoint, float webForce)
        {
            if (timeBtwShots <= 0)
            {
                var hitVision = EnemyAim(attackRange, whatIsSolid);
               shoot = CheckCollisionAndShoot(hitVision,ref timeBtwShots, startTimeBtwShots, enemyShot, shotPoint, webForce);
            }
            else
            {
                shoot = false;
                timeBtwShots -= Time.deltaTime;
            }
            return shoot;
        }

        public RaycastHit2D EnemyAim(float attackRange, LayerMask whatIsSolid)
        {
           return RaycastVisionToPlayer(attackRange, whatIsSolid);
        }

        public DirectionChargeParameters EnemyAimWithDirection(float attackRange, LayerMask whatIsSolid)
        {
            return RaycastVisionToPlayerWithDirection(attackRange, whatIsSolid);
        }

        private RaycastHit2D RaycastVisionToPlayer(float attackRange, LayerMask whatIsSolid)
        {
            Vector3 directionRay = (GameManager.instance.player.transform.position - transform.position).normalized;
            //debug
            Debug.DrawRay(transform.position + (directionRay * 1), directionRay * attackRange, Color.green, 0.1f);

            return Physics2D.Raycast(transform.position + (directionRay * 1), directionRay, attackRange, whatIsSolid);

        }

        private DirectionChargeParameters RaycastVisionToPlayerWithDirection(float attackRange, LayerMask whatIsSolid)
        {
            Vector3 directionRay = (GameManager.instance.player.transform.position - transform.position).normalized;
            var rayCast = Physics2D.Raycast(transform.position + (directionRay * 1), directionRay, attackRange, whatIsSolid);
            //debug
            Debug.DrawRay(transform.position + (directionRay * 1), directionRay * attackRange, Color.green, 0.1f);

            return new DirectionChargeParameters(rayCast, directionRay);
        }

        private bool CheckCollisionAndShoot(RaycastHit2D hitVision, ref float timeBtwShots, float startTimeBtwShots, GameObject enemyShot,
            Transform shotPoint, float webForce)
        {
            if (hitVision.collider != null)
            {
                if (hitVision.collider.CompareTag("Player"))
                {
                    timeBtwShots = startTimeBtwShots;
                    shotPoint.rotation = CalculateRotationToTarget();
                    InstantiateShoot(enemyShot,shotPoint,webForce);
                    return true;
                }
            }
            return false;
        }

        public void CheckCollisionAndCharge(RaycastHit2D hitVision,ref float timeBtwCharge, float startTimeBtwCharge, Vector3 directionRay,
           ref Vector3 directionCharge, ref bool isCharging)
        {
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

        private Quaternion CalculateRotationToTarget()
        {
            Vector3 difference = (transform.position - GameManager.instance.player.transform.position);
            float rotZ = (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg);
            return Quaternion.Euler(0f, 0f, rotZ + 90);
        }

        private void InstantiateShoot(GameObject enemyShot, Transform shotPoint, float webForce)
        {
            GameObject shot = Instantiate(enemyShot, shotPoint.position, shotPoint.rotation);
            shot.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(0f, webForce, 0f));
        }

    }
}