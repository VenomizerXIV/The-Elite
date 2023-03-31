using Chronos;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public float jumpForce = 10f;
    public float speed = 3.0f; // Enemy movement speed
    public float zoneRadius = 5.0f; // Radius of enemy's zone
    public float attackRadius = 2.0f; // Radius at which enemy will attack player
    private Transform player; // Reference to player object
    public bool isFollowing = false;
    public bool isAttack;
    public Animator animator; // reference to animator component
    private EnemyCombat enemyCombat;
    private Rigidbody2D rb;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;
    private EnemyHP health;
    Clock clock;
    void Start()
    {
        // Find the player object by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyCombat = GetComponent<EnemyCombat>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHP>();
        clock = Timekeeper.instance.Clock("NonPlayer");

    }

    void Update()
    {

        if (health.currentHealth > 0)
        {
            // attackTimer -= Time.deltaTime;
            // Check if player is within the enemy's zone
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);


            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);


            Vector2 direction = player.position - transform.position;

            if (direction.x < 0.1f)
            {
                transform.localScale = new Vector3(1, 1, 1); // Facing right
            }
            else if (direction.x > -0.1f)
            {
                transform.localScale = new Vector3(-1, 1, 1); // Facing left
            }

            if (distanceToPlayer < zoneRadius)
            {
                // If the player is within attack radius, damage them
                if (distanceToPlayer < attackRadius)
                {
                    isFollowing = false;
                    isAttack = true;
                }
                else
                {
                    // Move towards the player
                    rb.velocity = new Vector2(direction.x * speed * clock.timeScale, rb.velocity.y);
                    isFollowing = true;
                    isAttack = false;
                }

                if (isGrounded && player.position.y - transform.position.y >= 2)
                {
                    Jump();
                    // rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                }
            }
            else
            {
                // Set isFollowing flag to false
                isFollowing = false;
            }

            enemyCombat.canAttack = isAttack;
            animator.SetBool("isMoving", isFollowing);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", rb.velocity.y);

        }
    }


    void OnDrawGizmosSelected()
    {
        // Draw a circle around the enemy's zone
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, zoneRadius);

    }

    public void Jump()
    {
        if (!isGrounded)
            return;
        // Debug.Log("Jump");
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce * clock.timeScale, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;


        }
    }


}
