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

    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;



    Controller2D controller;

    private void Start()
    {
        controller = GetComponent<Controller2D>();

        //This calculation calcualtes gravity by using jumpheight devided by the power of the Apex height
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        //This is for testing purposeses and should be deleted
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }
    private void Update()
    {
        //This section is the set up for collision detection for both horizontal and vertical axis
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(Input.GetKeyDown(KeyCode.Space)&& controller.collisions.below)
        {
            velocity.y = jumpVelocity;
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
