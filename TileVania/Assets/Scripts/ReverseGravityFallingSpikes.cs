using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseGravityFallingSpikes : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float hitDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
            Invoke("DropSpike", hitDelay);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            Debug.Log("Hit!");
        }
    }

    private void DropSpike()
    {
        rb.isKinematic = false;
        rb.gravityScale *= -1;
    }
}
