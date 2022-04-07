using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class LoaderCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;
        private float timeBtwLoadingScenes;
        private float startTImeLoadingScene = 2f;

        private void Update()
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
                StartCoroutine(LoadingCoroutine());
                
            }
        }
        private IEnumerator LoadingCoroutine()
        {
            yield return null;
            timeBtwLoadingScenes = startTImeLoadingScene;
            while (!(timeBtwLoadingScenes <= 0))
            {
                timeBtwLoadingScenes -= Time.deltaTime;
                yield return null;
            }
            LoaderSceneScript.LoaderCallback();
        }

    }
}
