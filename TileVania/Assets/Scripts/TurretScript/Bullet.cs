using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger !=true)
        {
            if (collision.gameObject.name.Equals("Player"))
            {
                Debug.Log("Bullet Hits The Player!");
                Destroy(gameObject);
            }
            if (collision.gameObject.name.Equals("Foreground"))
            {
                Debug.Log("Bullet Hits The Foreground!");
                Destroy(gameObject);
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            
        }
    }
}
