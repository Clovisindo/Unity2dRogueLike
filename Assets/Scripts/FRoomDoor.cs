using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities = Assets.Utilities.Utilities;

public class FRoomDoor : MonoBehaviour
{
    [SerializeField] protected AudioClip doorSound;

    private GameObject[] _doors;//cada FRoomDoor contiene 2 elementos puerta para hacer una unica agrupacion 
    public bool isSecretDoor = false;
    public bool isNotDoor = false;
    private bool isClosed = true;

    public bool IsSecretDoor { get => isSecretDoor; set => isSecretDoor = value; }
    public bool IsClosed { get => isClosed; set => isClosed = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _doors = Utilities.GetChildsObject(this.gameObject.transform, "Door");
    }

    public void OpenDoor()
    {
        if (!IsSecretDoor && !isNotDoor)
        {
            foreach (var _door in _doors)
            {
                _door.SetActive(false);
            }

            //sonido de puerta
            SoundManager.instance.PlaySingle(doorSound);
        }
       
    }

    public void OpenSecretDoor()
    {
        if (IsSecretDoor && !isNotDoor)
        {
            IsClosed = false;
            foreach (var _door in _doors)
            {
                _door.SetActive(false);
            }

            //sonido de puerta
            SoundManager.instance.PlaySingle(doorSound);
        }

    }

    public void CloseDoor()
    {
        if (!IsSecretDoor && !isNotDoor)
        {
            foreach (var _door in _doors)
            {
                _door.SetActive(true);
            }
            //sonido de puerta
            SoundManager.instance.PlaySingle(doorSound);
        }
    }

    public void CloseSecretDoor()
    {
        if (IsSecretDoor && !isNotDoor)
        {
            IsClosed = true;
            foreach (var _door in _doors)
            {
                _door.SetActive(true);
            }
            //sonido de puerta
            SoundManager.instance.PlaySingle(doorSound);
        }
    }
}
