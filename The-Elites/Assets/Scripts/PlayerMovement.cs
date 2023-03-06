using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D RBplayer;
    private float Direction;
    [SerializeField]private float MovementSpeed = 7f;
    [SerializeField]private float JumpPower = 8f;
    void Start()
    {
        RBplayer = GetComponent<Rigidbody2D>();
        
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
    }


}
