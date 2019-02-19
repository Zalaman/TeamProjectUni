using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    //Jumping floats to calculate jump height and the time spent in air which will be used later on
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f ;

    //How long it takes to get the character moving on ground/in air.
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    //base move speed
    float moveSpeed = 6;
    float gravity;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallJumpLeap;
    public float wallSlideSpeedMax = 3;


    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;



    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        //This calculation calcualtes gravity by using jumpheight devided by the power of the Apex height
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //This is for testing purposeses and should be deleted
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        int wallDirectionX = (controller.collisions.left) ? -1 : 1;

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;
            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }
        }    

        //This section is the set up for collision detection for both horizontal and vertical axis
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding)
            {
                if (wallDirectionX == input.x)
                {
                    velocity.x = -wallDirectionX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirectionX* wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirectionX * wallJumpLeap.x;
                    velocity.y = wallJumpLeap.y;
                }
            }
            if (controller.collisions.below)
            {
                velocity.y = jumpVelocity;
            }
        }

        //float t is used for readability 
        float t = (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne;
        //This part is used for making all transactions smooth so there is no gittering/ glitching out.
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, t);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
