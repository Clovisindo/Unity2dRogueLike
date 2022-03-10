using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

	public void triggerMenu(int trigger)
	{
		switch (trigger)
		{
			case (0):
				SceneManager.LoadScene("gameScene");
				break;
			case (1):
				Application.Quit();
				break;
		}
	}
}

