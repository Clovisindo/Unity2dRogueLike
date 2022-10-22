using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Furniture
{
    public class fBanner : fFloorMechanic
    {
        public override string fName => "fBanner";
        [SerializeField]
        public string bannerName;

        protected override void Awake()
        {
        }

        protected override void ActivateMechanic()
        {
        }

        protected override void OnCollisionEnter2D(Collision2D other)
        {
        }

        public string GetBannerNameType()
        {
            return bannerName;
        }
    }
}
