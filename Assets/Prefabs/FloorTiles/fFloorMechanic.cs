using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class fFloorMechanic : MonoBehaviour
{
    protected Animator animator;
    [SerializeField] protected AudioClip trapSound;

    public abstract string name { get; }

    protected const float rechargeTime = 2.0f;
    protected float passingTime = rechargeTime;
    protected bool mechActivated = false;


    // Start is called before the first frame update
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        //controlamos el tiempo que tarda en volver a activarse la trampa
        if (passingTime <= 0)
        {
            passingTime -= Time.deltaTime;
            mechActivated = true;
        }
        else
        {
            mechActivated = false;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !mechActivated)
        {
            ActivateMechanic();
        }
    }

    protected abstract void ActivateMechanic();

    protected virtual void EndAnimation()
    {
        passingTime = rechargeTime;
        animator.SetTrigger("Activate");
    }
}
