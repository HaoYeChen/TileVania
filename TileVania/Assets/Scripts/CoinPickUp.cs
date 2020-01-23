using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int pointsForCoinPickUp = 100;
    private bool coinPicked = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!coinPicked)
        {
            coinPicked = true;
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickUp);
            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
        
    }
}
