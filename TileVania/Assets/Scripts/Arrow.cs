using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float velX = 10f;
    float velY = 0f;
    Rigidbody2D rb;
    
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(velX, velY);
        Invoke("DestroySelf", 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Foreground"))
        {
            DestroySelf();
        }
        if (collision.gameObject.name.Equals("Enemy"))
        {
            DestroySelf();
        }
    }


    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
