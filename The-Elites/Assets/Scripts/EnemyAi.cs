using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float speed = 3.0f; // Enemy movement speed
    public float damage = 10.0f; // Damage inflicted on player
    public float zoneRadius = 5.0f; // Radius of enemy's zone
    public float attackRadius = 1.0f; // Radius at which enemy will attack player
    public LayerMask playerLayer; // Layer mask for player object

    private Transform player; // Reference to player object
    private bool isFollowing = false; // Is the enemy following the player?

    void Start()
    {
        // Find the player object by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Check if player is within the enemy's zone
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < zoneRadius)
        {
            // Move towards the player
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // If the player is within attack radius, damage them
            if (distanceToPlayer < attackRadius)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }

            // Set isFollowing flag to true
            isFollowing = true;
        }
        else
        {
            // Set isFollowing flag to false
            isFollowing = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a circle around the enemy's zone
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, zoneRadius);

        // Draw a circle around the enemy's attack radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public bool IsFollowingPlayer()
    {
        return isFollowing;
    }
}
