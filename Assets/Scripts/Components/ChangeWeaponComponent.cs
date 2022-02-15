using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class ChangeWeaponComponent : MonoBehaviour
    {

        public void ChangeWeaponBehaviour(ref float timeBtwChangeWeapon, float startTimeBtwChangeWeapon, ref bool changeWeaponPressed ,ref Weapon currentWeapon, List<Weapon> playerWeapons)
        {
            //cambiar de arma
            if (timeBtwChangeWeapon <= 0)
            {
                if (changeWeaponPressed && currentWeapon.CheckIsIddleAnim())
                {
                    ChangeWeapon(ref currentWeapon, playerWeapons);
                    timeBtwChangeWeapon = startTimeBtwChangeWeapon;
                    changeWeaponPressed = false;
                }
                if (!changeWeaponPressed)
                {
                    changeWeaponPressed = false;
                }
            }
            else
            {
                timeBtwChangeWeapon -= Time.deltaTime;
            }
        }

        private void ChangeWeapon(ref Weapon currentWeapon, List<Weapon> playerWeapons)
        {
            // current weapon disable
            currentWeapon.gameObject.SetActive(false);
            //Get next weapon
            var nextWeapon = GetNextWeapon(currentWeapon.tag, playerWeapons);
            SetCurrentWeapon(GetEnumWeaponByTag(nextWeapon.tag),ref currentWeapon, playerWeapons);
            //enable current weapon
            currentWeapon.gameObject.SetActive(true);
        }

        private Weapon GetNextWeapon(string currentWeaponTag, List<Weapon> playerWeapons)
        {
            for (int i = 0; i < playerWeapons.Count; i++)
            {
                if (playerWeapons[i].tag == currentWeaponTag)
                {
                    if ((i + 1) >= playerWeapons.Count)
                    {
                        return playerWeapons[0].GetComponent<Weapon>();
                    }
                    else
                    {
                        return playerWeapons[i + 1].GetComponent<Weapon>();//controlar esto para que de la vuelta
                    }

                }
            }
            return null;
        }

        public void SetCurrentWeapon(EnumWeapons _enumWeapon,ref Weapon currentWeapon, List<Weapon> playerWeapons)
        {
            //no casteamos el arma en concreto hasta asignarla en currentWeapon
            switch (_enumWeapon)
            {
                case EnumWeapons.GreatHammer:
                    currentWeapon = (wGreatHammer)GetWeaponByTag(_enumWeapon, playerWeapons);
                    break;
                case EnumWeapons.KnightSword:
                    currentWeapon = (wKnightSword)GetWeaponByTag(_enumWeapon, playerWeapons);
                    break;
                default:
                    break;
            }
            HealthManager.instance.UpdateWeaponFrame(currentWeapon.WeaponSprite);
            Debug.Log("Arma cambiada a " + _enumWeapon.ToString());
        }

        private Weapon GetWeaponByTag(EnumWeapons _enumWeapon, List<Weapon> playerWeapons)
        {
            foreach (var weapon in playerWeapons)
            {
                if (weapon.tag == _enumWeapon.ToString())
                {
                    return weapon.GetComponent<Weapon>();
                }
            }
            return null;
        }

        private EnumWeapons GetEnumWeaponByTag(string weaponTag)
        {
            EnumWeapons _enumWeapon;
            switch (weaponTag)
            {
                case "GreatSword":
                    _enumWeapon = EnumWeapons.GreatSword;
                    break;
                case "GreatHammer":
                    _enumWeapon = EnumWeapons.GreatHammer;
                    break;
                case "KnightSword":
                    _enumWeapon = EnumWeapons.KnightSword;
                    break;
                case "KnightShield":
                    _enumWeapon = EnumWeapons.KnightShield;
                    break;
                case "PickAxe":
                    _enumWeapon = EnumWeapons.PickAxe;
                    break;
                default:
                    _enumWeapon = EnumWeapons.GreatHammer;//ToDo: controlar nulos
                    break;
            }
            return _enumWeapon;
        }

    }
}
