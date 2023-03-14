using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float cooldownTime = 1f;
    private float nextFireTime = 0.5f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 0.5f;

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
        // Check if Attack1 animation has finished playing
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= cooldownTime && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            // Reset Attack1 bool if player did not complete the combo attack
            if (noOfClicks < 2)
            {
                anim.SetBool("Attack1", false);
            }
        }
        // Check if Attack2 animation has finished playing
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= cooldownTime && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            // Reset both Attack1 and Attack2 bools after combo attack is completed
            anim.SetBool("Attack1", false);
            anim.SetBool("Attack2", false);
            noOfClicks = 0;
        }

        // Reset combo attack if maxComboDelay time has passed since last click
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            anim.SetBool("Attack1", false);
            anim.SetBool("Attack2", false);
            noOfClicks = 0;
        }

        // Check if it's time to register another click
        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
    }

    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;

        if (noOfClicks == 1)
        {
            anim.SetBool("Attack1", true);
        }

        noOfClicks = Mathf.Clamp(noOfClicks, 0, 2);

        // Check if the Attack1 animation is playing and the max combo delay has not been reached
        if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < cooldownTime)
        {
            // Set Attack2 to true to prepare for the combo attack
            anim.SetBool("Attack2", true);
        }
        else if (noOfClicks >= 2 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            // Start the Attack2 animation
            anim.SetBool("Attack1", false);
            anim.SetBool("Attack2", true);
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