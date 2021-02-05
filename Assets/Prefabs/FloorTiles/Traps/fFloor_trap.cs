using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fFloor_trap : MonoBehaviour
{
    private Animator animator;
    public AudioClip trapSound;

    protected const int trap_damage = 1;

    protected const float rechargeTime = 2.0f;
    protected float passingTime = rechargeTime;
    protected bool trapActivated = false;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        //controlamos el tiempo que tarda en volver a activarse la trampa
        if (passingTime <= 0)
        {
            passingTime -= Time.deltaTime;
            trapActivated = true;
        }
        else
        {
            trapActivated = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !trapActivated)
        {
            ActivateTrap();
        }
    }

    private void ActivateTrap()
    {
        //sonido de trampa
        SoundManager.instance.PlaySingle(trapSound);

        //se activa la animacion
        animator.SetTrigger("Activate");

        //restamos vida al jugador
        GameManager.instance.player.TakeDamage(trap_damage);
    }
    /// <summary>
    /// Se llama desde el gestor de animaciones
    /// </summary>
    void EndAnimation()
    {
        passingTime = rechargeTime;
        animator.SetTrigger("Activate");
    }
}
