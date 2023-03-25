using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class ai_jump : MonoBehaviour
{

    public AIPath aIPath;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public LayerMask groundLayer;

    public float speed = 6f;
    public float jumpPower = 14f;

    public float air_speed = 3f;

    private bool isFacingRight;
    private bool isGrounded;

    private float horzintal;
    private void Update()
    {
        horzintal = aIPath.velocity.x;

        if (aIPath.velocity.y > 0f && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
        if (aIPath.velocity.y > 0f && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        Flip();
    }


    private void FixedUpdate()
    {

        if (!IsGrounded())
        {
            air_speed = 3f;
        }
        else
        {
            air_speed = speed;
        }
        rb.velocity = new Vector2(rb.velocity.x * air_speed, rb.velocity.y);
        Debug.Log(aIPath.velocity.x);
        Debug.Log(aIPath.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Flip()
    {
        if (isFacingRight && horzintal > 0f || !isFacingRight && horzintal < 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
