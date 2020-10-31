using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public Transform[] startingPositions;
    public GameObject[] rooms;

    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBTwRoom = 0.25f;
    
     void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        //Instantiate(rooms[0], transform.position, Quaternion.identity);//integrar aqui nuestro boardManage e irle pasando la posicion
        GameManager.instance.boardScript.BoardSetup(transform.position);

        direction = Random.Range(1, 6);
    }

     void Update()
    {
        if (timeBtwRoom <= 0)
        {
            Move();
            timeBtwRoom = startTimeBTwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (direction == 1 || direction == 2) //move RIGHT
        {
            Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
            transform.position = newPos;
        }
        else if (direction == 3 || direction == 4)//move LEFT
        {
            Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
            transform.position = newPos;
        }
        else if (direction == 5)//move DOWN
        {
            Vector2 newPos = new Vector2(transform.position.x , transform.position.y - moveAmount);
            transform.position = newPos;
        }

        Instantiate(rooms[0], transform.position, Quaternion.identity);
        direction = Random.Range(1, 6);
    }


}
