using UnityEngine;
using EnumScenes = LoaderSceneScript.Scene;

namespace Assets.Scripts.UI
{
    public class MainMenuScript: MonoBehaviour
    {

        public void triggerMenu(int trigger)
        {
            LoaderSceneScript.triggerMenu(trigger);
        }

        public void triggerScene(EnumScenes scene)
        {
            LoaderSceneScript.LoadScene(scene);
        }
    }
}
