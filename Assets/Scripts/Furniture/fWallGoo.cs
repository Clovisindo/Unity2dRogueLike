using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Furniture
{
    public class fWallGoo : fFloorMechanic
    {
        public override string fName => "wallGoo";


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
}
