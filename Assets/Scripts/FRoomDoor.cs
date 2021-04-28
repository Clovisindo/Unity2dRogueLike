using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRoomDoor : MonoBehaviour
{
    [SerializeField] protected AudioClip doorSound;

    private GameObject[] _doors;
    public bool isSecretDoor = false;
    private bool isClosed = true;
    private BoxCollider2D colliderDoor;

    public bool IsSecretDoor { get => isSecretDoor; set => isSecretDoor = value; }
    public bool IsClosed { get => isClosed; set => isClosed = value; }

    // Start is called before the first frame update
    void Awake()
    {
        _doors = Utilities.GetChildsObject(this.gameObject.transform, "Door");
        if (IsSecretDoor)
        {
            colliderDoor = Utilities.GetChildObject(this.gameObject.transform, "Door").GetComponent<BoxCollider2D>();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void OpenDoor()
    {
        if (!IsSecretDoor)
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
        if (IsSecretDoor)
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
        if (!IsSecretDoor)
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
        if (IsSecretDoor)
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

    public bool CheckDoorIsClosed()
    {
        return _doors[0].activeSelf;
    }
}
