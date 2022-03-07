using UnityEngine;

namespace Assets.Scripts.Components
{
    public class EquipShieldComponent : MonoBehaviour
    {

        public bool EquipShieldBehaviour( ref float timeBtwEquipShield, float startTimeBtwEquipShield, bool EquipShieldPressed, Weapon currentShield, Weapon currentWeapon)
        {
            //Equipar escudo
            if (timeBtwEquipShield <= 0)
            {
                if (EquipShieldPressed && !currentShield.gameObject.activeSelf && currentWeapon.CheckIsIddleAnim())
                {
                    EquipShieldBlock(currentShield,currentWeapon);
                }
                if (currentShield.gameObject.activeSelf && currentShield.CheckIsIddleAnim() && (!EquipShieldPressed || currentShield.FirstAttack))
                {
                    UnEquipShieldBlock(currentShield,currentWeapon);
                    timeBtwEquipShield = startTimeBtwEquipShield;
                }
            }
            else
            {
                timeBtwEquipShield -= Time.deltaTime;
            }
            return EquipShieldPressed;
        }

        private void EquipShieldBlock(Weapon currentShield, Weapon currentWeapon)
        {
            currentWeapon.gameObject.SetActive(false);
            currentShield.gameObject.SetActive(true);
            currentShield.FirstAttack = false;
            HealthManager.instance.UpdateWeaponFrame(currentShield.WeaponSprite);
        }

        public void UnEquipShieldBlock(Weapon currentShield, Weapon currentWeapon)
        {
            currentShield.gameObject.SetActive(false);
            currentWeapon.gameObject.SetActive(true);
            HealthManager.instance.UpdateWeaponFrame(currentWeapon.WeaponSprite);
        }
    }
}
