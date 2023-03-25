using UnityEngine;

public class Slowdown : MonoBehaviour
{
    [SerializeField] private float slowdownFactor = 0.5f;
    [SerializeField] private float slowdownDuration = 2f;
    private bool isSlowingDown = false;
    private float slowdownTimer = 0f;
    private float originalFixedDeltaTime = 0f;
    private Collider2D playerCollider = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player enters the trigger, save its collider and start the time slowdown
        if (other.CompareTag("Player"))
        {
            playerCollider = other;
            originalFixedDeltaTime = Time.fixedDeltaTime;
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = originalFixedDeltaTime * slowdownFactor;
            isSlowingDown = true;
            slowdownTimer = slowdownDuration;
        }
    }

    private void Update()
    {
        // Check if the game is currently in slow motion
        if (isSlowingDown)
        {
            // Decrement the slowdown timer
            slowdownTimer -= Time.unscaledDeltaTime;

            // If the timer has expired, resume normal time scale and stop slowing down
            if (slowdownTimer <= 0f)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = originalFixedDeltaTime;
                isSlowingDown = false;
            }

            // If the player is inside the time slowdown trigger, freeze the physics and apply normal movement
            if (playerCollider != null && playerCollider.bounds.Intersects(GetComponent<Collider2D>().bounds))
            {
                Rigidbody2D playerRb = playerCollider.GetComponent<Rigidbody2D>();
                playerRb.velocity = playerRb.velocity.normalized * (playerRb.velocity.magnitude / slowdownFactor);
                Time.timeScale = 1f;
                Time.fixedDeltaTime = originalFixedDeltaTime;
            }
            else
            {
                // If the player is outside the time slowdown trigger, resume the physics simulation and restore time scale
                playerCollider = null;
                Time.timeScale = slowdownFactor;
                Time.fixedDeltaTime = originalFixedDeltaTime * slowdownFactor;
            }
        }
    }
}
