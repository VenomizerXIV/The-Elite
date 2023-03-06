using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //super power delegate function and event
    //it notify all subscribed object
    public delegate void superPowerActivated();
    public static event superPowerActivated superPowerInfo;

    //super power limit decrease when superPower used
    [SerializeField] private int superPowerLimit = 1;
    private Rigidbody2D RBplayer;
    private float Direction;
    [SerializeField] private float MovementSpeed = 7f;
    void Start()
    {
        RBplayer = GetComponent<Rigidbody2D>();

    }


    void Update()
    // Movement of the player left and right
    {
        Direction = Input.GetAxisRaw("Horizontal");
        RBplayer.velocity = new Vector2(Direction * MovementSpeed, RBplayer.velocity.y);
        
        
        //superPower Activation key
        if (Input.GetKeyDown("q"))
        {
            if (superPowerInfo != null && superPowerLimit > 0)
            {
                superPowerInfo();
                superPowerLimit--;
            }
        }
    }

    //Detect collisions between the GameObjects with Colliders attached
    // void OnCollisionEnter(Collision collision)
    // {
    //     //Check for a match with the specified name on any GameObject that collides with your GameObject
    //     if (collision.gameObject.name == "superPowerActivator")
    //     {
    //         //If the GameObject's name matches the one you suggest, output this message in the console
    //         Debug.Log("Do something here");
    //     }

    //     //Check for a match with the specific tag on any GameObject that collides with your GameObject
    //     if (collision.gameObject.tag == "MyGameObjectTag")
    //     {
    //         //If the GameObject has the same tag as specified, output this message in the console
    //         Debug.Log("Do something else here");
    //     }
    // }
}
