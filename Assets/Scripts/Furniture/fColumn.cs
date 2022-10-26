using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fColumn : fFloorMechanic
{

    public override string fName => "column";
    public override string SubtypeName { get;}

    protected override void Awake()
    {
        
    }

    protected override void ActivateMechanic()
    {
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
    }
}
