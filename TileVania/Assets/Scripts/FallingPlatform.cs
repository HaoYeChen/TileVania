using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float platformFallingDelay = 0.5f;
    [SerializeField] float platformDestroyDelay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Invoke("DropPlatform", platformFallingDelay);
            //Destroy(gameObject, platformDestroyDelay);
        }
    }

    private void DropPlatform()
    {
        rb.isKinematic = false;
    }
}
