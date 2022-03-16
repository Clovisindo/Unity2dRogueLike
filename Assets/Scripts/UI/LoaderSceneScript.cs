using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoaderSceneScript
{

	public static void triggerMenu(int trigger)
	{
		switch (trigger)
		{
			case (0):
				SceneManager.LoadScene("gameScene");
				break;
			case (1):
				Application.Quit();
				break;
			case (2):
				SceneManager.LoadScene("MainMenuScene");
				break;
			case (3):
				SceneManager.LoadScene("");
				break;
		}
	}
}

