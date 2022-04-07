using UnityEngine;
using scene = LoaderSceneScript.Scene;

namespace Assets.Scripts.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        public static bool GameIsPaused = false;
        public GameObject PauseMenuUI;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        private void Pause()
        {
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        public void LoadMenu()
        {
            Time.timeScale = 1f;
            LoaderSceneScript.LoadScene(scene.MainMenuScene);
        }
        public void QuitGame()
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
    }
}
