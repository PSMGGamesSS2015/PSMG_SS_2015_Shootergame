﻿// Handle player movement (movement in X/Y directions, jumping, fly mode)

using UnityEngine;
using System.Collections;

// Require a rigidbody on the player object
[RequireComponent(typeof(Rigidbody))]

// Require a BoxCollider on the player object - change if Collider changes!!
[RequireComponent(typeof(BoxCollider))]

public class PlayerMovement : MonoBehaviour 
{
    // Movement speed of the player
    public float speed = 5.0f;

    // Flying speed of the player
    public float flySpeed = 10.0f;

    // Gravity that is applied to the player while not flying
    public float gravity = 15.0f;

    // Gravity that is applied to the player while flying
    public float flyGravity = 3f;

    // Variable that determines whether the player should accelerate while falling in fly mode
    public bool accelerateWhileFlying = false;

    // Limit the maximum change in velocity to prevent strange behaviour
    public float maxVelocityChange = 10.0f;

    // Determines if the player is able to move
    public bool canMove = true;

    // Determines if the player is able to jump
    public bool canJump = true;

    // Determines if the player is able to fly
    public bool canFly = true;

    // Height of a jump
    public float jumpHeight = 2.0f;

    // Height of a wing flap in fly mode
    public float flapHeight = 3.0f;

    // Delay between flaps in seconds
    public float flapDelay = 2.0f;

    // Amount of flaps per activation of fly mode
    public int flapAmount = 10;

    // Initial height when activating fly mode
    public float initialFlyHeight = 10.0f;

    // Maximum absolute fly height (might want to change to relative to ground)
    public float maximumFlyHeight = 150.0f;

    // True, if the player is on the ground
    private bool grounded = false;

    // True as soon as the player starts falling for the first time after activating flyMode
    private bool fallingWhileFlying = false;

    // True as soon as the player activates flyMode
    private bool flyModeActivated = false;

    // Saves flap time to prevent spamming the flap button - CAUTION: need to find another solution if pause gets implemented
    private float flapTime = 0.0f;

    // Saves the raming amount of flaps while in fly mode
    private int remainingFlaps = 0;

    void Awake()
    {
        // Freeze the rotation of the player's rigidbody to prevent unwanted behaviour
        GetComponent<Rigidbody>().freezeRotation = true;

        // Don't use gravity for the player for more freedom
        GetComponent<Rigidbody>().useGravity = false;
    }

    void FixedUpdate()
    {
        // If the player is on the ground...
        if (grounded)
        {
            // ...handle movement if he is allowed to
            if (canMove)
            {
                HandleMovement();
            }

            // ...handle jumping if he is allowed to
            if (canJump) 
            {
                CheckForJump();
            }

            // ...handle activation of fly mode if he is allowed to
            if (canFly)
            {
                CheckForFlyMode();
            }
        }

        // If the player is not on the ground, in fly mode and not in the "initial jump phase" anymore...
        else if (fallingWhileFlying)
        {
            // ...handle movement and flying
            HandleMovement();
            HandleFlying();
        }

        // Apply gravity
        ApplyGravity();

        // Assume that the player is not on the ground
        grounded = false;
    }

    void CheckForFlyMode()
    {
        // If the button for activation of fly mode is pressed...
        if (Input.GetButton("Fly"))
        {
            // ...reset the amount of flaps available to the player
            remainingFlaps = flapAmount;

            // ...set the fly mode to activated
            flyModeActivated = true;

            // ...jump as high as set
            Jump(initialFlyHeight);
        }
    }

    // If the player is on the ground (again), reset all relevant variables
    void OnCollisionStay()
    {
        grounded = true;
        fallingWhileFlying = false;
        flyModeActivated = false;
    }

    void HandleMovement()
    {
        // Get input values
        float InputX = Input.GetAxis("Horizontal");
        float InputY = Input.GetAxis("Vertical");

        // modify factor so that diagonal movement isn't faster
        float inputModifyFactor = (InputX != 0.0f && InputY != 0.0f) ? 0.7071f : 1.0f;

        // Calculate how fast we should be moving
        Vector3 targetVelocity = new Vector3(InputX * inputModifyFactor, 0, InputY * inputModifyFactor);

        // Move into the right direction
        targetVelocity = transform.TransformDirection(targetVelocity);

        // Use the appropriate speed modifier, depending on if the player is in fly mode or not
        targetVelocity *= flyModeActivated ? flySpeed : speed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void CheckForJump()
    {
        // If the jump button is pressed...
        if (Input.GetButton("Jump"))
        {
            // ...perform a jump with the defined jump height
            Jump(jumpHeight);
        }
    }

    void Jump(float height)
    {
        // Get the player's current velocity
        Vector3 velocity = GetComponent<Rigidbody>().velocity;

        // Calculate the speed needed to reach the defined height
        float verticalSpeed = Mathf.Sqrt(2 * height * gravity);

        // Create a vector from these values
        Vector3 jumpVector = new Vector3(velocity.x, verticalSpeed, velocity.z);

        // Apply the vector to the player's rigidbody
        GetComponent<Rigidbody>().velocity = jumpVector;
    }

    void HandleFlying()
    {
        // Get the player's y position
        float yPosition = transform.position.y;

        // Calculate the distance of the player to the maximum fly height
        float distanceToLimit = maximumFlyHeight - yPosition;

        // Initialize the actual flap height that we are going to use to the set flap height
        float modifiedFlapHeight = flapHeight;

        // If the player would exceed the maximum fly height by flapping...
        if (distanceToLimit < flapHeight)
        {
            // Set the flap height to the value with which the player is going to reach the maximum fly height
            modifiedFlapHeight = distanceToLimit;
        }

        // If the player still has remaing flaps, presses the flap button and the last flap was longher than flapDelay seconds ago...
        if (remainingFlaps > 0 && Input.GetButton("Flap") && ((Time.time - flapTime) >= flapDelay))
        {
            // Substract 1 from the remaining flaps
            remainingFlaps--;
            
            // Save the current time
            flapTime = Time.time;

            // Perform a "jump" with the flap height we have set above
            Jump(modifiedFlapHeight);
        }
    }

    void ApplyGravity()
    {
        // If the player is not falling while flying...
        if (!fallingWhileFlying)
        {
            // Create a new vector with just a y component that is calculated using the gravity and the player's mass
            Vector3 gravityVector = new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0);

            // Add the gravity vector to the player's rigidbody
            GetComponent<Rigidbody>().AddForce(gravityVector);

            // Check if the player is "falling while flying": If the fly mode is activated and the player's y velocity is below 0 (which means that he started falling), set the variable to true
            if (flyModeActivated && GetComponent<Rigidbody>().velocity.y < 0)
            {
                fallingWhileFlying = true;
            }
        }

        // If the player is falling while flying...
        else
        {
            // ...and if the player should accelerate while he is flying...
            if (accelerateWhileFlying)
            {
                // ...create a new vector with just a y component that is calculated using the fly gravity and the player's mass
                Vector3 gravityVector = new Vector3(0, -flyGravity * GetComponent<Rigidbody>().mass, 0);
                GetComponent<Rigidbody>().AddForce(gravityVector);
            }

            // ...and if the player should not accelerate while he is flying...
            else
            {
                // NOT YET IMPLEMENTED 
                Vector3 gravityVector = new Vector3(0, -flyGravity * GetComponent<Rigidbody>().mass, 0);
                GetComponent<Rigidbody>().AddForce(gravityVector);
            }
        }
        
    }

    // Returns flyModeActivated for UI scripts
    public bool getMode()
    {
        return flyModeActivated;
    }
}