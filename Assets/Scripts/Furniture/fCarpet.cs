using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fCarpet : fFloorMechanic
{
    public override string fName => "fCarpet";
    [SerializeField]
    public string carpetName;
    public override string SubtypeName => carpetName;

    protected override void Awake()
    {
        //SubtypeName = carpetName;
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
