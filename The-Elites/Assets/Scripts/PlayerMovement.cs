using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Chronos;
public class PlayerMovement : MonoBehaviour
{

    //super power limit decrease when superPower used
    [SerializeField] private int superPowerLimit = 1;


    [SerializeField] private PostProcessVolume _postProcessVolume;
    private static Vignette _vignette;

    // super power activated or not
    public static bool superPowerActivated = false;
    // super power time limit
    [SerializeField] private float superPowerTimeLimit = 10f;
    // super power timer
    private float superPowerTimer = 0.0f;

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

    private Clock clock;

    Health health;
    void Start()
    {
        RBplayer = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        health = GetComponent<Health>();
        _postProcessVolume.profile.TryGetSettings(out _vignette);
    }


    void Update()
    // Movement of the player left and right
    {
        Direction = Input.GetAxisRaw("Horizontal");
        RBplayer.velocity = new Vector2(Direction * MovementSpeed, RBplayer.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);


        clock = Timekeeper.instance.Clock("NonPlayer");

        //superPower Activation key
        if (Input.GetKeyDown("q") || superPowerActivated)
        {
            activeSuperPower();
        }

        // Jumping of the player
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        UpdateAnimation();

        if (health.isDied)
        {
            StopSuperPower();
        }

    }



    private void activeSuperPower()
    {

        {

            if (superPowerActivated)
            {
                superPowerTimer += Time.deltaTime / Time.timeScale;

            }

            if (superPowerLimit > 0 && !superPowerActivated)
            {
                Debug.Log("superpower activated " + superPowerActivated);
                StartSuperPower();
            }

            if (superPowerTimer > superPowerTimeLimit && superPowerActivated)
            {
                StopSuperPower();
            }

        }

    }



    private void StopSuperPower()
    {
        RBplayer.velocity = new Vector2(0f, 0f);
        clock.localTimeScale = 1f;
        superPowerActivated = false;
        _vignette.active = false;
        superPowerTimer = 0.0f;
    }
    private void StartSuperPower()
    {
        clock.localTimeScale = 0.25f;
        superPowerActivated = true;
        _vignette.active = true;
        superPowerLimit--;
    }

    public void Jump()
    {
        // if (isGrounded)
        //     return;
        Debug.Log("Jump");
        RBplayer.velocity = new Vector2(RBplayer.velocity.x, 0f);
        RBplayer.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
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

}