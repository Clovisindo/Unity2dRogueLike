using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fFloor_trap : fFloorMechanic
{

    protected override  void ActivateMechanic()
    {
        //sonido de trampa
        SoundManager.instance.PlaySingle(trapSound);

        //se activa la animacion
        animator.SetTrigger("Activate");
    }
    /// <summary>
    /// Se llama desde el gestor de animaciones
    /// </summary>
    protected override void EndAnimation()
    {
        passingTime = rechargeTime;
        animator.SetTrigger("Activate");
    }
}
