using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Matthew Macdonald
    // 14-01-24
    // Handles all logic for the main enemy

    // health variables
    public float maxHealth; // max health of the enemy
    public float trueHealth; // the actual health of the enemy
    public float visualHealth; // health as shown on the healthbar
    public float healthAnimateSpeed; // how fast does the visualized health catch up to the true health

    // movment variables
    public float speed; // move speed
    public float attackRange; // max distance at which the enemy will start attacking
    public float aproachRange; // max distance at which the ememy will start to aproach

    public virtual bool isAttacking // is true when the current animation is the attack animation
    {
        get
        {
            return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "LightBandit_Attack";
        }
    }
    public virtual bool isHurt // is true when the current animation is the hurt animation
    {
        get
        {
            return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "LightBandid_Hurt";
        }
    }

    // healthbar variables
    [SerializeField] Transform healthBar; // transform of the red portion of the healthbar
    [SerializeField] Transform healthBarBackground; // transform of the backgrond of the healthbar to allow hiding it on death
    private Vector3 healthBarStartingScale;

    // Internal component references.  Made public to allow inheritance, but hidden in the inspector
    [HideInInspector] public Transform player;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public Animator anim;
    /*
     * ANIMATION STATES:
     * 0: IDLE
     * 1: COMBAT IDLE
     * 2: RUNNING
     * TRIGGERS:
     * "Jump"
     * "Recover"
     * "Hurt"
     * "Death"
     * "Attack"
     * BOOLS:
     * "Grounded"
    */

    // internal variables
    private PlayerController pc; // player controller reference
    private Vector2 relativeVector; // the relative vector between the player and enemy
    private bool side; // the side the player is on relative to the enemy: if left then true, if right false
    private float distance; // the absolute magnitude of the relative vector
    private bool isDead; // used to prevent duplicating the death animation

    private void Start() // get references for internal variables
    {
        player = FindObjectOfType<PlayerController>().transform;
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        pc = player.GetComponent<PlayerController>();
        healthBarStartingScale = healthBar.localScale;
    }
    private void Update()
    {
        // prevent attacking dead player
        if (pc.hasStartedDeath)
        {
            anim.SetInteger("AnimState", 0);
            return;
        }

        // prevent moving when dead/hurt
        if (isDead || isHurt) return;

        HandleState();
        HandleHealth();
    }

    public void Damage(float amount) // damage enemy
    {
        rb.velocity = Vector2.zero;
        trueHealth -= amount;
        anim.SetTrigger("Hurt");
    }
    private void HandleHealth() // handle everything to do with health, visuals and state
    {
        // the difference between the amount of health on the healthbar, and the actual amount of health the enemy has
        float healthDifference = trueHealth - visualHealth;

        // if the visual health is close enough to the true health, set it to the true health
        if (Mathf.Abs(healthDifference) < healthAnimateSpeed + 0.05f)
        {
            visualHealth = trueHealth;
        } else
        {
            // move the visual health by the health animate speed
            if (healthDifference > 0)
            {
                visualHealth += healthAnimateSpeed;
            } else
            {
                visualHealth -= healthAnimateSpeed;
            }

        }

        // kill enemy
        if (visualHealth < 0)
        {
            StartCoroutine(HandleDeath());
            return;
        }

        
        // get the scale of the healthbar depending on the visual health (0-1)
        float newScale = visualHealth / maxHealth;

        // if visualhealth or maxhealth are 0, the value will be either Infinity or NaN respectively
        if (float.IsNaN(newScale)) newScale= 0;
        if (float.IsInfinity(newScale)) newScale = 1;

        // get the scale of the bar in accordance with its full scale
        newScale *= healthBarStartingScale.x;
        healthBar.localScale = new (newScale, healthBarStartingScale.y);
    }
    private void HandleState() // handle the state of the enemy
    {
        // update state variables
        relativeVector = transform.position - player.position;
        side = relativeVector.x > 0;
        distance = Mathf.Abs(relativeVector.magnitude);

        // don't do anything if its already attacking
        if (isAttacking) return;

        // if player is in range, attack
        if (distance < attackRange)
        {
            anim.SetTrigger("Attack");
            rb.velocity *= new Vector2(0, 1);
            return;
        }

        // if player is in the aproach range, walk towards them
        if (distance < aproachRange)
        {
            anim.SetInteger("AnimState", 2);
            rb.AddForce(new Vector2((side ? -1 : 1) * speed * Time.deltaTime, 0));
            sr.flipX = !side;
            return;
        }

        // if no other conditions are met, set the enemy to idle
        anim.SetInteger("AnimState", 0);
    }
    private IEnumerator HandleDeath()
    {
        // if this enemy is a node, set itself to active
        if (this.gameObject.TryGetComponent<Node>(out Node node))
        {
            node.Toggle();
        }

        // start death
        anim.SetTrigger("Death");
        isDead = true;

        // disable the healthbar
        healthBar.gameObject.SetActive(false);
        healthBarBackground.gameObject.SetActive(false);

        // wait for a bit before fading
        yield return new WaitForSeconds(0.25f);

        // fade out the enemy
        Color colour = sr.color;
        for (int i = 0; i < 10; i++)
        {
            colour.a -= 0.1f;
            sr.color = colour;
            yield return new WaitForSeconds(0.05f);
        }

        // remove enemy
        Destroy(this.gameObject);
        yield return null;
    }
}
