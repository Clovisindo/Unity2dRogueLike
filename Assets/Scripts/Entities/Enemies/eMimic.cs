﻿using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities = Assets.Utilities.Utilities;


namespace Assets.Scripts.Entities.Enemies
{
    public class eMimic : Enemy
    {
        protected bool mimicActivated = false;
        public override string name => "eMimic";
        protected GameObject HPBarobject;


        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            target = FindObjectOfType<Player>().transform;
            enemyCurrentHealth = enemyMaxHealth;
            healthBar.SetMaxHealth(enemyMaxHealth);
            HPBarobject = Utilities.Utilities.GetChildObject(this.transform, "healthBar");
            collider = this.GetComponent<BoxCollider2D>();
            rb = this.GetComponent<Rigidbody2D>();
        }

        protected override void EnemyBehaviour()
        {
            MovementEnemyBehaviour();
        }

        protected override void MovementEnemyBehaviour()
        {
            if (mimicActivated)
            {
                if (Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) >= minRange)
                {
                    FollowPlayer();
                    isMoving = true;
                    animator.SetBool("isMoving", isMoving);
                }
                else
                {
                    GoRespawn();
                    CheckResetMimic();
                }
            }
        }

        private void CheckResetMimic()
        {
            if (transform.position == this.GetRespawnPosition())
            {
                DeactivateMimic();
            }
        }

        /// <summary>
        /// Cuando se acerce el jugador, se activa el enemigo
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Player" && !mimicActivated)
            {
                ActivateMimic();
            }
        }

        private void ActivateMimic()
        {
            mimicActivated = true;
            animator.SetTrigger("Activate");
            HPBarobject.SetActive(true);
        }
        private void DeactivateMimic()
        {
            mimicActivated = false;
            animator.SetTrigger("Activate");
            HPBarobject.SetActive(false);
            isMoving = false;
            animator.SetBool("isMoving", isMoving);
        }
    }

}