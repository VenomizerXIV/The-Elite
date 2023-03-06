using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerMovement : MonoBehaviour
{

    //super power delegate function and event
    //it notify all subscribed object
    public delegate void superPower();
    public static event superPower superPowerInfo;

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


        //superPower Activation key
        if (Input.GetKeyDown("q") || superPowerActivated)
        {
            activeSuperPower();
        }

        // Jumping of the player
        if (Input.GetButtonDown("Jump"))
        {
            RBplayer.velocity = new Vector2(RBplayer.velocity.x, JumpPower);
            Debug.Log(JumpPower);
        }
        UpdateAnimation();
    }



    private void activeSuperPower()
    {

        if (superPowerActivated)
        {
            superPowerTimer += Time.deltaTime / Time.timeScale;
            Debug.Log("timer +++ " + superPowerTimer);
        }

        if (superPowerInfo != null && superPowerLimit > 0 && !superPowerActivated)
        {
            superPowerInfo();
            FreezeTimeStart();
        }

        if (superPowerTimer > superPowerTimeLimit && superPowerActivated)
        {
            FreezeTimeStop();
        }


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



    private void FreezeTimeStart()
    {
        Time.timeScale = TimeScale;
        RBplayer.mass *= 4;
        RBplayer.gravityScale *= 4;
        superPowerLimit--;
        anim.speed = 2;
        MovementSpeed /= Time.timeScale;
        JumpPower /= Time.timeScale;
        superPowerActivated = true;
        _vignette.active = true;
        Debug.Log("Super Power Activated ");
    }
    private void FreezeTimeStop()
    {
        superPowerActivated = false;
        RBplayer.mass /= 4;
        RBplayer.gravityScale /= 4;
        MovementSpeed *= Time.timeScale;
        JumpPower *= Time.timeScale;
        anim.speed = 1;
        superPowerActivated = false;
        superPowerTimer = 0.0f;
        Time.timeScale = 1f;
        _vignette.active = false;
        Debug.Log("Super Power Timeout");
    }
}
