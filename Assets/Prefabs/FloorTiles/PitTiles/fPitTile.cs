using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fPitTile : fFloorMechanic
{
    public override string fName => "pitTile";
    public override string SubtypeName { get;}

    protected override void ActivateMechanic()
    {
        //sonido de trampa
        SoundManager.instance.PlaySingle(trapSound);

        //se activa la invocacion de pocion
        ActivateEffect();
    }

    private void ActivateEffect()
    {
        GameManager.instance.player.PlayerStartFalling();

        Debug.Log("El jugador se cae.");
    }
}
