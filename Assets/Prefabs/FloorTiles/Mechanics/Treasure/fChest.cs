using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fChest : fFloorMechanic
{
    [SerializeField] private GameObject potionPrefab;
    private GameObject potionInstance;

    protected override void ActivateMechanic()
    {
        //sonido de trampa
        SoundManager.instance.PlaySingle(trapSound);

        //se activa la invocacion de pocion
        ActivateEffect();
        //se activa la animacion
        animator.SetTrigger("Activate");
    }

    private void ActivateEffect()
    {
        Vector2 potionPosition;
        potionPosition = new Vector2(this.transform.position.x + 1, this.transform.position.y);
        potionInstance = Instantiate(potionPrefab, potionPosition, Quaternion.identity);
    }

    /// <summary>
    /// Se llama desde el gestor de animaciones
    /// </summary>
    protected override void EndAnimation()
    {
        passingTime = rechargeTime;
        animator.SetTrigger("Activate");
    }
}
