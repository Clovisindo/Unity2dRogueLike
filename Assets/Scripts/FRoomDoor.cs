using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRoomDoor : MonoBehaviour
{
    [SerializeField] protected AudioClip doorSound;

    private GameObject[] _doors;//cada FRoomDoor contiene 2 elementos puerta para hacer una unica agrupacion 
    public bool isSecretDoor = false;
    public bool isNotDoor = false;
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
    /// <summary>
    /// Desactiva la puerta correspondiente
    /// </summary>
    public void DisableDoor()
    {
        foreach (var _door in _doors)
        {
            _door.SetActive(false);
        }
    }

    //public LevelGeneration.doorDirection GetDoorDirection()
    //{
    //    return _doors[0].doo
    //}

    public bool CheckDoorIsClosed()
    {
        return _doors[0].activeSelf;
    }
}
