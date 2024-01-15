using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // handles projectile logic
    
    public float speed; // projectile speed
    public float lifeTime; // amount of time before the projectile despawns
    public float damage; // amount of damage done by projectile

    // internal variables
    [HideInInspector] public Vector3 dir; // move direction of projectile.  Made public to allow seting by other scripts, but hidden in inspector
    private SpriteRenderer sr; 
    private float spawnTime; // time at which the projectile was spawned
    private bool isDead = false;
    public void SetProjectile(Vector3 _dir) // constructor function
    {
        spawnTime = Time.time;
        dir = _dir;
        sr = GetComponent<SpriteRenderer>();
        // if the velocity is less than 0, flip the sprite 
        sr.flipX = _dir.x < 0;
        Vector3 startOfset = ((sr.flipX ? -1 : 1) * Vector3.right + Vector3.down) * 0.05f;
        transform.position += startOfset;
    }
    void Update()
    {
        // if the sprite has not started the death animation, but the projectile has expended its lifetime, start the death animation
        if (Time.time - spawnTime  > lifeTime && !isDead) {
            isDead = true;
            GetComponent<Animator>().SetTrigger("Die");
        }

        // move projectile by direction * deltatime (time since last frame) * the speed
        // isDead ? 0.5f : 1f handles slowing the projectile if its dying
        transform.position = transform.position + ((isDead ? 0.5f : 1f) * speed * Time.deltaTime * dir);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // don't do anything if already dead
        if (isDead)
        {
            return;
        }

        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (enemy.isDead) return;
            enemy.Damage(damage);

            GetComponent<Animator>().SetTrigger("Die");

            return;
        }
        // die
        GetComponent<Animator>().SetTrigger("Die");

        // if its an enemy, damage the enemy


        // if its a node (but not an enemy), toggle it
        if (collision.gameObject.TryGetComponent<Node>(out Node node))
        {
            node.Toggle();
            return;
        }
    }
    public void SelfDestroy()
    {
        Destroy(this.gameObject);
    }
}
