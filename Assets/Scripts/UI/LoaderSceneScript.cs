using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoaderSceneScript
{

	public enum Scene
    {
		gameScene,
		MainMenuScene,
		VictoryMenuScene,
		GameOverMenuScene,
		LoadingScene,
		QuitGame
	}

	private static Action onLoaderCallback;

	public static void triggerMenu(int trigger)
	{
		switch (trigger)
		{
			case (0):
				LoadScene(Scene.gameScene);
				break;
			case (1):
				Application.Quit();
				break;
			case (2):
				LoadScene(Scene.MainMenuScene);
				break;
			case (3):
				LoadScene(Scene.VictoryMenuScene);
				break;
			case (4):
				LoadScene(Scene.GameOverMenuScene);
				break;
		}
	}

	public static void LoadScene(Scene scene)
    {
        if (scene != Scene.QuitGame)
        {
			onLoaderCallback = () =>
			{
				SceneManager.LoadScene(scene.ToString());
			};

			SceneManager.LoadScene(Scene.LoadingScene.ToString());
			
		}
        else
        {
			Application.Quit();
        }
    }

	public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
			onLoaderCallback();
			onLoaderCallback = null;
        }
    }


}

