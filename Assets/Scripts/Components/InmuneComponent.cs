using UnityEngine;
namespace Assets.Scripts.Components
{
    public class InmuneComponent : MonoBehaviour
    {


        public bool InmuneBehaviour(ref float  passingTime, float inmuneTime, bool playerInmune)
        {
            if (passingTime < inmuneTime)
            {
                passingTime += Time.deltaTime;
                playerInmune = true;
            }
            else
            {
                playerInmune = false;
            }
            return playerInmune;
        }
    }
}
