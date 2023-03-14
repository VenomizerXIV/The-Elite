using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float attackDelay = 1f; // delay between attacks
    private Transform player; // Reference to player object
    public bool canAttack = false;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private Animator animator; // reference to animator component
    public int lightAttackDamage = 10;
    public int heavyAttackDamage = 20;
    public int lightAttackRange = 10;
    public int heavyAttackRange = 20;
    private Health playerhealth;
    public EnemyController enemyController;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyController = GetComponent<EnemyController>();
        animator = GetComponent<Animator>();
        playerhealth = player.GetComponent<Health>();
    }

    void Update()
    {

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0.0f && canAttack)
        {
            Attack();
        }
        isAttacking = false;
    }



    void Attack()
    {
        if (!isAttacking)
        {
            // play attack sound
            // AudioSource.PlayClipAtPoint(attackSound, transform.position);

            // choose random attack animation
            int attackNum = Random.Range(1, 3);
            animator.SetInteger("AttackNum", attackNum);
            animator.SetTrigger("Attack");
            Attack1Damage();
            // set attack timer and flag
            isAttacking = true;
            attackTimer = attackDelay;
        }

    }


    void OnDrawGizmosSelected()
    {
        // Draw a circle around the enemy's attack radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyController.attackRadius);
    }


    public void Attack1Damage()
    {
        // calculate the distance between the enemy and the player
        float distance = Vector2.Distance(transform.position, player.position);

        // check if the player is within the attack range
        if (distance <= lightAttackRange)
        {
            // player is in attack zone, do something
            playerhealth.TakeDamage(lightAttackDamage);
        }
    }

    // This function will be called from the heavy attack animation event
    public void Attack2Damage()
    {
        // calculate the distance between the enemy and the player
        float distance = Vector2.Distance(transform.position, player.position);

        // check if the player is within the attack range
        if (distance <= heavyAttackRange)
        {
            // player is in attack zone, do something
            playerhealth.TakeDamage(heavyAttackDamage);
        }
    }

}