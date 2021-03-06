using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRoomDoor : MonoBehaviour
{
    [SerializeField] protected AudioClip doorSound;

    private GameObject[] _doors;

    // Start is called before the first frame update
    void Awake()
    {
        _doors = Utilities.GetChildsObject(this.gameObject.transform, "Door"); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void OpenDoor()
    {
        foreach (var _door in _doors)
        {
            _door.SetActive(false);
        }
       
        //sonido de puerta
        SoundManager.instance.PlaySingle(doorSound);
    }

    public void CloseDoor()
    {
        foreach (var _door in _doors)
        {
            _door.SetActive(true);
        }
        //sonido de puerta
        SoundManager.instance.PlaySingle(doorSound);
    }

    public bool CheckDoorIsClosed()
    {
        return _doors[0].activeSelf;
    }
}
