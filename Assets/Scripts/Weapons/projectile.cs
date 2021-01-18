using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{

    public float speed;
    public float lifeTime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;//blockingLayer

    public GameObject destroyEffect;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroyProjectile), lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);

        //debug
        Debug.DrawRay(transform.position, transform.up, Color.green, 0.1f);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                Debug.Log("Jugador golpeado.");
                hitInfo.collider.GetComponent<Player>().TakeDamage(damage);
                DestroyProjectile();
            }
            //ToDo: que se destruya con los muros tambien
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        //Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Debug.Log("Destruido proyectil.");
        Destroy(gameObject);
    }

}
