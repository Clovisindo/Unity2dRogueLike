using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fPotion : fFloorMechanic
{
    public override string name => "potion";
    protected override void ActivateMechanic()
    {
        //sonido de trampa
        SoundManager.instance.PlaySingle(trapSound);

        ActivateEffect();
    }

    private void ActivateEffect()//ToDo: cambiar esto no esta bien implementado
    {
        if (!HealthManager.instance.IsMaxHealth())
        {
            GameManager.instance.player.PlayerHealth += 1;
            GameManager.instance.player.UpdatePlayerHealth();
            Destroy(gameObject);
            Debug.Log("Pocion bebida.");
        }
    }
}
