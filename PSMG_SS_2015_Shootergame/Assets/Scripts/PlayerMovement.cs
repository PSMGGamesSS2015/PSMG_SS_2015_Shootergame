using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

//Change if Collider changes
[RequireComponent(typeof(BoxCollider))]

public class PlayerMovement : MonoBehaviour
{
    public int flyMode = 0;
    public float speed = 5.0f;
    public float flySpeed = 10.0f;
    public float gravity = 15.0f;
    public float flyGravity = 3f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    public float initialFlyHeight = 10.0f;
    private bool grounded = false;
    private bool flying = false;
    private bool flightMode = false;

    void Awake()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().useGravity = false;
    }

    void FixedUpdate()
    {
        if (grounded || flying)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetVelocity = transform.TransformDirection(targetVelocity);

            targetVelocity *= flying? flySpeed : speed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = GetComponent<Rigidbody>().velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            if (canJump && Input.GetButton("Jump") && !flightMode)
            {
                Jump(velocity);
            }

            // Fly
            if (Input.GetButton("Fly"))
            {
                flightMode = true;

                if (flyMode == 0)
                {
                    GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, initialFlyHeight, velocity.z);
                }
                else
                {
                    Jump(velocity);
                }
            }
        }

        // We apply gravity manually for more tuning control
        if (!flying) {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));

            if(GetComponent<Rigidbody>().velocity.y <= 0 && flightMode) {
                flying = true;
            }
        } else {
            // Don't use a force because we don't want the falling speed to increase
            if (flyMode == 0)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, -flyGravity, 0);
            }
            else
            {
                GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));
                if (Input.GetButton("Jump"))
                {
                    Vector3 velocity = GetComponent<Rigidbody>().velocity;
                    Jump(velocity);
                }
            }
            
        }

        grounded = false;
    }

    void OnCollisionStay()
    {
        grounded = true;
        flying = false;
        flightMode = false;
    }

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
}