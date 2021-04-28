using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject healthManager;
    public GameObject soundManager;
    public GameObject eventRoomControllerManager;


    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManager);
        }
        if (HealthManager.instance == null)
        {
            Instantiate(healthManager);
        }
        if (SoundManager.instance == null)
        {
            Instantiate(soundManager);
        }
        if (EventRoomController.instance == null)
        {
            Instantiate(eventRoomControllerManager);
        }

    }
}
