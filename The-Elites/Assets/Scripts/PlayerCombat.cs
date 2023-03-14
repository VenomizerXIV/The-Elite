using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attack1Duration = 0.5f; // Duration of the first attack animation
    public float attack2Duration = 0.5f; // Duration of the second attack animation
    public float cooldownTime = 1f; // Cooldown time between attacks
    public float timeToNextAttack = 0.5f; // Time limit for initiating the second attack
    public bool canAttack = true; // Whether the player can currently attack

    private Animator anim; // Reference to the Animator component
    private bool isAttacking = false; // Whether the player is currently attacking
    private int attackStep = 0; // Which step of the attack the player is on
    private float timeSinceAttack = 0f; // Time since the last attack
    private int clicks = 0; // Number of left mouse button clicks in quick succession
    private float clickTime = 0.3f; // Maximum time between left mouse button clicks for a double-click

    public int lightAttackDamage = 10;
    public int heavyAttackDamage = 20;
    public int lightAttackRange = 10;
    public int heavyAttackRange = 20;
    public LayerMask attackLayer;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player can attack
        if (canAttack && !isAttacking)
        {
            // Check for left mouse button input
            if (Input.GetMouseButtonDown(0))
            {
                clicks++;
                Debug.Log(clicks);
                // If the player has clicked twice in quick succession, initiate the attack sequence
                if (clicks == 2)
                {
                    // Start the attack sequence with attack1
                    anim.SetTrigger("attack1");
                    isAttacking = true;
                    attackStep = 1;

                    // Reset the click counter
                    clicks = 0;

                    // Reset the time since the last attack
                    timeSinceAttack = 0f;

                    // Disable the ability to attack during the cooldown time
                    canAttack = false;
                    Invoke("EnableAttack", cooldownTime);
                }
                else
                {
                    // Start the timer for a double-click
                    Invoke("ResetClicks", clickTime);
                }
            }
        }

        // If the player is on the second step of the attack
        if (attackStep == 2)
        {
            // Check for left mouse button input
            if (Input.GetMouseButtonDown(0))
            {
                // Start the second attack animation
                anim.SetTrigger("attack2");
                isAttacking = true;
                attackStep = 0;

                // Reset the click counter
                clicks = 0;

                // Disable the ability to attack during the cooldown time
                canAttack = false;
                Invoke("EnableAttack", cooldownTime);
            }
        }

        // Update the time since the last attack
        timeSinceAttack += Time.deltaTime;

        // If enough time has passed since the last attack, reset the attack step and click counter
        if (attackStep == 1 && timeSinceAttack > clickTime)
        {
            attackStep = 0;
            clicks = 0;
        }
    }


    void OnDrawGizmosSelected()
    {
        // Draw a circle around the player lightAttackRange zone
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lightAttackRange);
        // Draw a circle around the Player heavyAttackRange zone
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, heavyAttackRange);

    }


    // Function to enable the ability to attack after the cooldown time
    void EnableAttack()
    {
        canAttack = true;
    }

    // Function to reset the click counter
    void ResetClicks()
    {
        clicks = 0;
    }



    // Function to be called at the end of the first attack animation
    public void AttackStep2()
    {
        attackStep = 2;
        isAttacking = false;
    }

    // Function to be called at the end of the second attack animation
    public void EndAttack()
    {
        isAttacking = false;
    }

    // This function will be called from the light attack animation event
    public void Attack1Damage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(lightAttackRange, 1), 0, attackLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            Health health = hitCollider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(lightAttackDamage);
                Debug.Log("Damage : " + health);
            }
        }
    }

    // This function will be called from the heavy attack animation event
    public void Attack2Damage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(heavyAttackRange, 1), 0, attackLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            Debug.Log(hitCollider.tag);
            Health health = hitCollider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(heavyAttackDamage);
            }
        }
    }

}