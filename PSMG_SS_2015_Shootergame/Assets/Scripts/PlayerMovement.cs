// Handle player movement (movement in X/Y directions, jumping, fly mode)

using UnityEngine;
using System.Collections;

// Require a rigidbody on the player object
[RequireComponent(typeof(Rigidbody))]

// Require a BoxCollider on the player object - change if Collider changes!!
[RequireComponent(typeof(BoxCollider))]

public class PlayerMovement : MonoBehaviour
{
    // Enumeration object for easier selection of the flying mode in the editor
    public enum mode { Gliding = 0, Flapping = 1 }
    public mode flyMode = mode.Gliding;

    // Movement speed of the player
    public float speed = 5.0f;

    // Flying speed of the player
    public float flySpeed = 10.0f;

    // Gravity that is applied to the player while not flying
    public float gravity = 15.0f;

    // Gravity that is applied to the player while flying
    public float flyGravity = 3f;

    // NOT YET IMPLEMENTED - variable that determines whether the player should accelerate while flying
    public bool accelerateWhileFlying = false;

    // Limit the maximum change in velocity to prevent strange behaviour
    public float maxVelocityChange = 10.0f;

    // Determines if the player is able to jump - NOTE: Does not affect "flapping" in Flapping FlyMode despite using the Jump() function for flaps
    public bool canJump = true;

    // Height of a jump
    public float jumpHeight = 2.0f;

    // Initial flyHeight for Gliding FlyMode - TODO: Also apply for Flapping FlyMode
    public float initialFlyHeight = 10.0f;

    // True, if the player is on the ground
    private bool grounded = false;

    // True as soon as the player starts falling for the first time after activating flyMode
    private bool fallingWhileFlying = false;

    // True as soon as the player activates flyMode
    private bool flyModeActivated = false;

    void Awake()
    {
        // Freeze the rotation of the player's rigidbody to prevent unwanted behaviour
        GetComponent<Rigidbody>().freezeRotation = true;
        // Don't use gravity for the player for more freedom
        GetComponent<Rigidbody>().useGravity = false;
    }

    void FixedUpdate()
    {
        // If the player is either standing on the ground or falling while fly mode is activated...
        if (grounded || fallingWhileFlying)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = transform.TransformDirection(targetVelocity);

            // Use the appropriate speed modifier, depending on if the player is in fly mode or not
            targetVelocity *= fallingWhileFlying ? flySpeed : speed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = GetComponent<Rigidbody>().velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            if (canJump && Input.GetButton("Jump") && !flyModeActivated)
            {
                Jump(velocity);
            }

            // Fly
            if (Input.GetButton("Fly"))
            {
                flyModeActivated = true;

                // In Gliding fly mode...
                if (flyMode == 0)
                {
                    // ...make the player jump very high!
                    GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, initialFlyHeight, velocity.z);
                }
                // In Flapping fly mode...
                else
                {
                    // ...make the player jump normal once
                    Jump(velocity);
                }
            }
        }

        // We apply gravity manually for more tuning control
        if (!fallingWhileFlying)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));

            if (GetComponent<Rigidbody>().velocity.y <= 0 && flyModeActivated)
            {
                fallingWhileFlying = true;
            }
        // Use different gravitation while flying - NOTE: needs to be dynamic based on accelerateWhileFlying variable
        } else {
            if (flyMode == 0)
            {
                // Don't use a force for gravity because we don't want the falling speed to increase
                GetComponent<Rigidbody>().velocity = new Vector3(0, -flyGravity, 0);
            }
            else
            {
                // Use "normal" gravity when in Flapping FlyMode
                GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));

                // Allow the player to jump while in Flapping FlyMode
                if (Input.GetButton("Jump"))
                {
                    Vector3 velocity = GetComponent<Rigidbody>().velocity;
                    Jump(velocity);
                }
            }
            
        }

        // Assume that the player is not on the ground
        grounded = false;
    }

    // If the player is on the ground (again), reset all relevant variables
    void OnCollisionStay()
    {
        grounded = true;
        fallingWhileFlying = false;
        flyModeActivated = false;
    }

    // Jump with the given velocity
    void Jump(Vector3 velocity)
    {
        GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    // Returns flyModeActivated
    public bool getMode()
    {
        return flyModeActivated;
    }
}