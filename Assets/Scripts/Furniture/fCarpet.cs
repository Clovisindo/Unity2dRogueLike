using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fCarpet : fFloorMechanic
{
    public override string fName => "carpet";
    [SerializeField]
    public string carpetName;

    protected override void Awake()
    {
    }

    protected override void ActivateMechanic()
    {
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
    }

    public string GetCarpetNameType()
    {
        return carpetName;
    }
}
