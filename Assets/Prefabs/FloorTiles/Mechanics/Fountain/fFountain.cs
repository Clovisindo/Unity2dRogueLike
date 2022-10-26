using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fFountain : fFloorMechanic
{
    public override string fName => "fountain";
    public override string SubtypeName { get;}

    protected override void ActivateMechanic()
    {
        //sonido de trampa
        SoundManager.instance.PlaySingle(trapSound);

        ActivateEffect();
    }

    private void ActivateEffect()//ToDo: cambiar esto no esta bien implementado
    {
        GameManager.instance.player.PlayerHealth += 1;
        GameManager.instance.player.UpdatePlayerHealth();
    }
}
