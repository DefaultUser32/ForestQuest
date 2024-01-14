using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // Handles logic of coins
    public void DestroySelf() // used to delete the coin at the end of the animation (via animation triggers)
    {
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if touching the player, give them a coin and start the removal animation
        if (collision.TryGetComponent(out PlayerController pc))
        {
            pc.GetCoin();
            GetComponent<Animator>().SetTrigger("Pop");
        }
    }
}
