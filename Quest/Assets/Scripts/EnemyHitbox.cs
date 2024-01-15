using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // Allows enemy sword to hurt player

    public float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if the collision is not with the player, exit
        if (!collision.TryGetComponent<PlayerController>(out PlayerController pc))
        {
            return;
        }

        // hit player for 10 damage
        pc.GetHit(damage);
    }
}
