using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fFountain : fFloorMechanic
{
    protected override void ActivateMechanic()
    {
        //sonido de trampa
        SoundManager.instance.PlaySingle(trapSound);

        ActivateEffect();
    }

    private void ActivateEffect()//ToDo: cambiar esto no esta bien implementado
    {
        GameManager.instance.player.playerHealth += 1;
        GameManager.instance.player.UpdatePlayerHealth();
    }
}
