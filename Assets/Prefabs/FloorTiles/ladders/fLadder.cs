﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fLadder : fFloorMechanic
{
    public override string fName => "ladder";
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
        Debug.Log("Bajar al siguiente piso.");
    }
}
