using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MainMenuScript: MonoBehaviour
    {

        public void triggerMenu(int trigger)
        {
            LoaderSceneScript.triggerMenu(trigger);
        }
    }
}
