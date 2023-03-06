using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum PlayerMtype
    {
        idle , Running , Jump , Fall
    }
    private SpriteRenderer sprite;
    private Animator anim;
    private Rigidbody2D RBplayer;
    private float Direction;
    [SerializeField]private float MovementSpeed = 7f;
    [SerializeField]private float JumpPower = 8f;
    void Start()
    {
        RBplayer = GetComponent<Rigidbody2D>();
        anim =  GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    // Movement of the player left and right
    {
        Direction = Input.GetAxisRaw("Horizontal");
        RBplayer.velocity = new Vector2(Direction * MovementSpeed, RBplayer.velocity.y);

        // Jumping of the player
        if (Input.GetButtonDown("Jump"))
        {
            RBplayer.velocity = new Vector2(RBplayer.velocity.x, JumpPower);
        }
        UpdateAnimation();
    }
    // Animation of the player
    private void UpdateAnimation()
    {
        PlayerMtype State;
        if (Direction > 0f)
        {
            State = PlayerMtype.Running;
            sprite.flipX = false;
        }
        else if (Direction < 0f)
        {
            State = PlayerMtype.Running;
            sprite.flipX = true;

        }
        else
        {
            State = PlayerMtype.idle;
        }
        if (RBplayer.velocity.y > 0.1f)
        {
            State = PlayerMtype.Jump;
        }
        else if (RBplayer.velocity.y < -0.1f)
        {
            State = PlayerMtype.Fall;
        }
        
        anim.SetInteger("state", (int)State);

}
}
