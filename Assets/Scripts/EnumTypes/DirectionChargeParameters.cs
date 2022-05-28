using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.EnumTypes
{
    public class DirectionChargeParameters 
    {
        RaycastHit2D hitVision;
        Vector3 directionCast;

        public RaycastHit2D HitVision { get => hitVision; set => hitVision = value; }
        public Vector3 DirectionCast { get => directionCast; set => directionCast = value; }

        public DirectionChargeParameters(RaycastHit2D hitVision, Vector3 directionCast)
        {
            HitVision = hitVision;
            DirectionCast = directionCast;
        }
    }
}