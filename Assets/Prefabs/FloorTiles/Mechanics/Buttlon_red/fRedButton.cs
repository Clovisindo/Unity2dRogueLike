using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Prefabs.FloorTiles.Mechanics.Buttlon_red
{
    internal class fRedButton : fFloorMechanic
    {
        public override string name => "redButton";

        protected override void ActivateMechanic()
        {
            //sonido de trampa
            SoundManager.instance.PlaySingle(trapSound);

            ActivateEffect();

            //se activa la animacion
            animator.SetTrigger("Activate");
        }

        private void ActivateEffect()
        {
        }

        /// <summary>
        /// Se llama desde el gestor de animaciones
        /// </summary>
        protected override void EndAnimation()
        {
            passingTime = rechargeTime;
            ChangeSwitch();
        }

        private void ChangeSwitch()
        {
            if (animator.GetBool("switch_on"))
            {
                animator.SetBool("switch_on", false);
                animator.SetBool("switch_off", true);
            }
            else
            {
                animator.SetBool("switch_on", true);
                animator.SetBool("switch_off", false);
            }
        }
    }
}
