using UnityEngine;


namespace Assets.Scripts.Components
{
    public class ChangeUtilityComponent : MonoBehaviour
    {

        public bool ChangeUtilityBehaviour(ref float timeBtwChangeUtility, float startTimeBtwChangeUtility, bool EquipPickAxePressed, Weapon currentPickAxe, Weapon currentWeapon)
        {
            ////herramienta util
            if (timeBtwChangeUtility <= 0)
            {
                //change weapon
                if (EquipPickAxePressed && !currentPickAxe.gameObject.activeSelf && currentWeapon.CheckIsIddleAnim())
                {
                    EquipPickAxe(currentPickAxe,currentWeapon);
                }
                if (currentPickAxe.gameObject.activeSelf && currentPickAxe.CheckIsIddleAnim() && (!EquipPickAxePressed || currentPickAxe.FirstAttack))
                {
                    UnEquipPickAxe(currentPickAxe, currentWeapon);
                    timeBtwChangeUtility = startTimeBtwChangeUtility;
                }
            }
            else
            {
                timeBtwChangeUtility -= Time.deltaTime;
            }
            return EquipPickAxePressed;
        }

        private void EquipPickAxe(Weapon currentPickAxe, Weapon currentWeapon)
        {
            currentWeapon.gameObject.SetActive(false);
            currentPickAxe.gameObject.SetActive(true);
            currentPickAxe.FirstAttack = false;
        }

        private void UnEquipPickAxe(Weapon currentPickAxe, Weapon currentWeapon)
        {
            currentPickAxe.gameObject.SetActive(false);
            currentWeapon.gameObject.SetActive(true);
        }
    }
}
