// Handle player movement (movement in X/Y directions, jumping, fly mode)

using UnityEngine;
using System.Collections;

// Require a rigidbody on the player object
[RequireComponent(typeof(Rigidbody))]

// Require a CapsuleCollider on the player object
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerMovement : MonoBehaviour 
{
    // Movement speed of the player
    public float speed = 5.0f;

    // Sneaking speed of the player
    public float sneakingSpeed = 2.0f;

    // Crouching speed of the player
    public float crouchingSpeed = 2.0f;

    // Sneaking speed of the player
    public float sprintingSpeed = 10.0f;

    // Speed modifier for faster/slower movement
    public float speedModifier = 1.0f;

    // Slope limit in degrees, player will slide down if beyond the limit
    public float slopeLimit = 45.0f;

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

    // True, if the player is sneaking
    private bool sneaking = false;

    // True, if the player is moving
    private bool moving = false;

    // True, if the player is crouching
    private bool crouching = false;

    // True, if the player is sneaking
    private bool sprinting = false;

    // True as soon as the player starts falling for the first time after activating flyMode
    private bool fallingWhileFlying = false;

    // True when the player flaps until he is falling down again - necessary for gravity without acceleration
    private bool flapping = false;

    // True as soon as the player activates flyMode
    private bool flyModeActivated = false;

    // Saves flap time to prevent spamming the flap button - CAUTION: need to find another solution if pause gets implemented
    private float flapTime = 0.0f;

    // Saves the raming amount of flaps while in fly mode
    private int remainingFlaps = 0;

    // Save collider height to reset after crouching
    private float colliderHeight = 0.0f;

    // Weapon controller for animations
    private Assets.Scripts.Weapons.WeaponController weaponController;

    void Start()
    {
        colliderHeight = GetComponent<CapsuleCollider>().height;
        weaponController = GetComponent<Assets.Scripts.Weapons.WeaponController>();
    }

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
                // Check if the player has activated any special movement mode other than flying
                CheckMovementType();
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

        // Reset special movement modes
        moving = false;
        sneaking = false;
        sprinting = false;
        crouching = false;
    }

    // Check if the player has activated a special type of movement
    void CheckMovementType()
    {
        if (Input.GetButton("Sprint"))
        {
			if (Input.GetAxis("Vertical") == 1) {
				sprinting = true;
			}
        }

		if (Input.GetButton("Sneak") && Input.GetButton("Forward"))
        {
            sneaking = true;
        }

        if (Input.GetButton("Crouch"))
        {
            crouching = true;
            GetComponent<CapsuleCollider>().height = colliderHeight / 2;
        }
        else
        {
            GetComponent<CapsuleCollider>().height = colliderHeight;
        }
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

        moving = InputX > 0 || InputY > 0 ? true : false;

        // modify factor so that diagonal movement isn't faster
        float inputModifyFactor = (InputX != 0.0f && InputY != 0.0f) ? 0.7071f : 1.0f;

        // Calculate how fast we should be moving
        Vector3 targetVelocity = new Vector3(InputX * inputModifyFactor, 0, InputY * inputModifyFactor);

        // Move into the right direction
        targetVelocity = transform.TransformDirection(targetVelocity);

        // Use the appropriate speed modifier, depending on if the player is in fly mode or not
        targetVelocity *= GetSpeedModifier();

        // Apply the speed modifier that can be changed through methods
        targetVelocity *= speedModifier;

        // If the player is not flying, get the modifier based on the slope
        if (!flyModeActivated)
        {
            targetVelocity *= GetSlopeModifier(InputX, InputY);
        }

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);

        AnimatePlayer();
    }

    void AnimatePlayer()
    {

        if (sprinting)
        {
            //weaponController.getActiveWeapon().Animator.SetBool("walk", false);
            weaponController.getActiveWeapon().Animator.SetBool("run", true);
        }
        else if (moving)
        {
            weaponController.getActiveWeapon().Animator.SetBool("run", false);
            weaponController.getActiveWeapon().Animator.SetBool("walk", true);
        }
        else
        {
            weaponController.getActiveWeapon().Animator.SetBool("walk", false);
            weaponController.getActiveWeapon().Animator.SetBool("run", false);
        }
        
    }

    float GetSlopeModifier(float x, float y)
    {
        // Initialize our modifier variable
        float modifier = 1.0f;

        // First, check if we have exceeded the maximum slope - if yes, prevent movement by returning 0
        if (CheckMaxSlope())
        {
            return 0.0f;
        }

        // If max slope has not been exceeded...
        else
        {
            // Initialize variables for the raycast and the modifier
            RaycastHit hit;

            // Create the vector for the direction the player is moving to
            Vector3 direction = x * transform.right + y * transform.forward;
            // Set y component to 0 
            direction.y = 0;

            // If the ray hits terrain....
            if (Physics.Raycast(transform.position, direction, out hit, 3.0f))
            {
                // Calculate the dot product between the up vector and our movement vector
                modifier = Vector3.Dot(Vector3.up, hit.normal);
            }

            // Return the power to 2 for slower movement in steep terrain
            return Mathf.Pow(modifier, 2);
        }
    }

    bool CheckMaxSlope()
    {
        // Assume the player is allowed to jump
        canJump = true;

        // Initialize raycast variable
        RaycastHit hit;

        // Cast a ray to the terrain below
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 3.0f))
        {
            // Get the angle of the terrain
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            
            // If the angle is greater than our set slope limit..
            if (angle >= slopeLimit)
            {
                // Disallow jumping
                canJump = false;
                // Add a force that makes the player slide down
                GetComponent<Rigidbody>().AddForce(new Vector3(0, -Mathf.Sqrt(angle), 0), ForceMode.VelocityChange);
                // Return true since we have exceeded the maximum slope
                return true;
            }
        }

        // If we're still here, the maximum slope has not been exceeded
        return false;
    }

    float GetSpeedModifier()
    {
        if (flyModeActivated) {
            return flySpeed;
        }
        else if (sneaking) {
            return sneakingSpeed;
        }
        else if (crouching)
        {
            return crouchingSpeed;
        }
        else if (sprinting)
        {
            return sprintingSpeed;
        }

        return speed;
    }

    void CheckForJump()
    {
        // If the jump button is pressed...
        if (Input.GetButton("Jump"))
        {
            // ...perform a jump with the defined jump height
            Jump(jumpHeight);

            weaponController.getActiveWeapon().Animator.SetTrigger("Jump");
        }
    }

    void Jump(float height)
    {
        // Get the player's current velocity
        Vector3 velocity = GetComponent<Rigidbody>().velocity;

        float modifiedGravity = fallingWhileFlying ? flyGravity : gravity;

        // Calculate the speed needed to reach the defined height
        float verticalSpeed = Mathf.Sqrt(2 * height * modifiedGravity);

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
        if (remainingFlaps > 0 && Input.GetButton("Flap") && modifiedFlapHeight > 0 && ((Time.time - flapTime) >= flapDelay))
        {
            // Substract 1 from the remaining flaps
            remainingFlaps--;
            
            // Save the current time
            flapTime = Time.time;

            // Perform a "jump" with the flap height we have set above
            Jump(modifiedFlapHeight);

            flapping = true;
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
            if (accelerateWhileFlying || flapping)
            {
                // ...create a new vector with just a y component that is calculated using the fly gravity and the player's mass
                Vector3 gravityVector = new Vector3(0, -flyGravity * GetComponent<Rigidbody>().mass, 0);
                GetComponent<Rigidbody>().AddForce(gravityVector);

                if (GetComponent<Rigidbody>().velocity.y <= -flyGravity)
                {
                    flapping = false;
                }
            }

            // ...and if the player should not accelerate while he is flying...
            else
            {
                float xVelocity = GetComponent<Rigidbody>().velocity.x;
                float zVelocity = GetComponent<Rigidbody>().velocity.z;
                GetComponent<Rigidbody>().velocity = new Vector3(xVelocity, -flyGravity, zVelocity);
            }
        }        
    }

    public void AllowMove(bool value)
    {
        canMove = value;
    }

    public void AllowFly(bool value)
    {
        canFly = value;
    }

    public void AllowJump(bool value)
    {
        canJump = value;
    }

    public void IncreaseSpeed(float factor)
    {
        speedModifier *= factor;
    }

    public void DecreaseSpeed(float factor)
    {
        speedModifier /= factor;
    }

    public void ResetSpeed()
    {
        speedModifier = 1.0f;
    }

    // Returns flyModeActivated for UI scripts
    public bool getMode()
    {
        return flyModeActivated;
    }

    public int getRemainingFlaps()
    {
        return remainingFlaps;
    }
}