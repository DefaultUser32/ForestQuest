using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public GameStateHandler gameStateHandler;
    [SerializeField] BoxCollider2D floorCollider;
    public List<PlayerAttack> attackList;

    // movement variables
    public float runSpeed;
    public float jumpStartSpeed;
    public float jumpDurationSpeed;
    public float minVelocityForHold;
    public float maxHealth;
    public float health;
    public int coins;
    public bool hasStartedDeath;

    // animation variables
    public float minMovement; // minimum movment to allow player to turn around (flip sprite)
    public bool isAttacking
    {
        get 
        {
            foreach (PlayerAttack attack in attackList)
            {
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == attack.animationInternalName) return true;
            }
            return false;
        }
    }
    public bool isGrounded
    {
        get
        {
            return floorCollider.IsTouchingLayers(floorLayerMask);
        }
    }

    // Self components
    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Internal variables
    private int floorLayerMask;
    private Coroutine jump;
    private PlayerAttack nextAttack;


    void Start()
    {
        gameStateHandler = FindObjectOfType<GameStateHandler>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        floorLayerMask = LayerMask.GetMask("Floor");
    }

    void Update()
    {
        if (hasStartedDeath) return;
        HandleJump();
        HandleAttack();
    }
    private void FixedUpdate()
    {
        if (hasStartedDeath) return;
        HandleMovement();
    }
    public void GetHit(float damage)
    {
        // if the player has already died, stop
        if (hasStartedDeath) return;
        health -= damage;

        // kill player
        if (health < 0)
        {
            hasStartedDeath = true;
            rb.velocity = Vector3.zero;
            anim.SetTrigger("Die");
        }
    }
    public void Die()
    {
        gameStateHandler.RestartLevel();
    }
    public void GetCoin()
    {
        coins++;
    }
    private void HandleAttack() // handles player input of attacks, switching animations, and summoning projectiles
    {
        if (isAttacking) {
            return;
        }

        // check each attack to see if its button is pressed
        foreach (PlayerAttack attack in attackList)
        {
            if (Input.GetKeyDown(attack.attackInput))
            {
                nextAttack = attack;

                // Set AttackType before triggering the attack animation
                anim.SetInteger("AttackIndex", attack.attackIndex);
                anim.SetTrigger("Attack");

                return;
            }
        }
    }
    public void DoAttack() // called by the animation when its time to summon a projectile
    {
        GameObject nextProjectile = Instantiate(nextAttack.attackProjectile, transform.position, nextAttack.attackProjectile.transform.rotation);
        nextProjectile.GetComponent<Projectile>().SetProjectile(sr.flipX ? Vector3.left : Vector3.right);
    }
    private void HandleMovement()
    {
        rb.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * runSpeed, 0));

        // if the players velocity is greater than minMovement, update animations accordingly.  Otherwise, consider the player idle
        bool doAnimate = Mathf.Abs(rb.velocity.x) > minMovement;

        anim.SetBool("IsMoving", doAnimate);
        if (doAnimate)
        {
            sr.flipX = rb.velocity.x < 0;
        }
    }
    private void HandleJump()
    {
        // update falling
        anim.SetBool("IsFalling", !isGrounded);

        // if the space key is not pressed, exit
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }

        // if you are not touching the ground, exit
        if (!isGrounded)
        {
            return;
        }
        
        // if the jump coroutine is null, start the jump.  Otherwise, do nothing
        jump ??= StartCoroutine(StartJump());
    }
    private IEnumerator StartJump()
    {
        // start jump
        rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y);
        anim.SetTrigger("Jump");
        anim.SetBool("IsFalling", Input.GetKey(KeyCode.Space));

        // start adding force
        rb.AddForce(Vector2.up * jumpStartSpeed);
        yield return new WaitForSeconds(0.075f); // wait until the player is off the ground

        // while the space key is pressed, add force
        while (Input.GetKey(KeyCode.Space) && rb.velocity.y >= minVelocityForHold)
        {
            rb.AddForce(Vector2.up * jumpDurationSpeed);
            yield return new WaitForSeconds(0.015f);
        }

        // nullify the coroutine, and exit
        jump = null;
        yield return null;
    }
}
