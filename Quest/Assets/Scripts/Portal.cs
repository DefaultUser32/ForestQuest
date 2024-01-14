using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // handles the portal at the end of the level

    private PlayerController pc;
    private Coroutine awaitExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the portal has already been collided with, return
        if (awaitExit != null) return;

        // if the collision target is the player, exit the level
        if (collision.TryGetComponent<PlayerController>(out PlayerController _pc))
        {
            pc = _pc;
            awaitExit = StartCoroutine(AwaitExit());
        }
    }
    private IEnumerator AwaitExit() // start the fade out animation, and 0.5 seconds later end the level
    {
        FindObjectOfType<PlayerUIHandler>().fadeOutImage.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        pc.gameStateHandler.CompleteLevel();

    }
}
