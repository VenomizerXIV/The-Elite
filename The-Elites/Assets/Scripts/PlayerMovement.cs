using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{

    //super power delegate function and event
    //it notify all subscribed object
    public delegate void superPower();
    // public static event superPower superPowerInfo;

    //super power limit decrease when superPower used
    [SerializeField] private int superPowerLimit = 1;


    [SerializeField] private PostProcessVolume _postProcessVolume;
    private static Vignette _vignette;

    // super power activated or not
    private bool superPowerActivated = false;
    // super power time limit
    [SerializeField] private float superPowerTimeLimit = 10f;
    // super power timer
    private float superPowerTimer = 0.0f;

    [SerializeField] private float TimeScale = 0.5f;

    private enum PlayerMtype
    {
        idle, Running, Jump, Fall
    }
    private SpriteRenderer sprite;
    private Animator anim;
    private Rigidbody2D RBplayer;
    private float Direction;
    [SerializeField] private float MovementSpeed = 7f;
    [SerializeField] private float JumpPower = 8f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;
    private bool isGrounded;
    void Start()
    {
        RBplayer = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        _postProcessVolume.profile.TryGetSettings(out _vignette);
    }


    void Update()
    // Movement of the player left and right
    {
        Direction = Input.GetAxisRaw("Horizontal");
        RBplayer.velocity = new Vector2(Direction * MovementSpeed, RBplayer.velocity.y);
       
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);


        //superPower Activation key
        if (Input.GetKeyDown("q") || superPowerActivated)
        {
            activeSuperPower();
            // Debug.Log("activated super power");
        }

        // Jumping of the player
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // RBplayer.velocity = new Vector2(RBplayer.velocity.x, JumpPower);
            Jump();
        }
        UpdateAnimation();
    }



    private void activeSuperPower()
    {

        if (superPowerActivated)
        {
            superPowerTimer += Time.deltaTime / Time.timeScale;

        }

        if (superPowerLimit > 0 && !superPowerActivated)
        {
            Debug.Log("superpower activated " + superPowerActivated);
            // superPowerInfo();
            FreezeTimeStart();
        }

        if (superPowerTimer > superPowerTimeLimit && superPowerActivated)
        {
            FreezeTimeStop();
        }

    }


    public void Jump()
    {
        // if (isGrounded)
        //     return;
        Debug.Log("Jump");
        RBplayer.velocity = new Vector2(RBplayer.velocity.x, 0f);
        RBplayer.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
    }



    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Debug.Log("dddddddddddddddd : " + collision.gameObject.tag);
    //     if (collision.gameObject.tag == "ground")
    //     {
    //         isGrounded = true;
    //         RBplayer.velocity = Vector3.zero;
    //         RBplayer.angularVelocity = 0f;

    //     }
    // }

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
        if (RBplayer.velocity.y > 0.1f && !isGrounded)
        {
            State = PlayerMtype.Jump;
        }
        else if (RBplayer.velocity.y < -0.1f && !isGrounded)
        {
            State = PlayerMtype.Fall;
        }

        anim.SetInteger("state", (int)State);
    }

    private void FreezeTimeStart()
    {
        Time.timeScale = TimeScale;
        RBplayer.mass *= 1 / TimeScale * 2;
        RBplayer.gravityScale *= 1 / TimeScale * 2;
        superPowerLimit--;
        anim.speed = 1 / TimeScale;
        MovementSpeed /= Time.timeScale;
        JumpPower = 250;
        superPowerActivated = true;
        _vignette.active = true;
        Debug.Log("Super Power Activated ");
    }
    private void FreezeTimeStop()
    {
        superPowerActivated = false;
        RBplayer.mass /= 1 / TimeScale * 2;
        RBplayer.gravityScale /= 1 / TimeScale * 2;
        MovementSpeed *= Time.timeScale;
        JumpPower = 15;
        anim.speed = 1;
        superPowerActivated = false;
        superPowerTimer = 0.0f;
        Time.timeScale = 1f;
        _vignette.active = false;
        Debug.Log("Super Power Timeout");
    }
}
