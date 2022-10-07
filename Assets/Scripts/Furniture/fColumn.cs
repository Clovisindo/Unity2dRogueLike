using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fColumn : fFloorMechanic
{

    public override string fName => "column";

    // Start is called before the first frame update
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
