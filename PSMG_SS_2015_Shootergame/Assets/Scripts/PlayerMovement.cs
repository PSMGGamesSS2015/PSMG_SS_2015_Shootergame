﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

//Change if Collider changes
[RequireComponent(typeof(BoxCollider))]

public class PlayerMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 5.0f;
    public float flySpeed = 10.0f;
    public float gravity = 15.0f;
    public float flyGravity = 1.5f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    public float initialFlyHeight = 10.0f;
    private bool grounded = false;
    private bool flying = false;

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
            if (canJump && Input.GetButton("Jump") && !flying)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
            }

            // Fly
            if (Input.GetButton("Fire1"))
            {
                GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, initialFlyHeight, velocity.z);
            }

            Debug.Log(transform.position.y);
        }

        // We apply gravity manually for more tuning control
        if (!flying) {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));

            // TO DO: change from static 30 to dynamic max-height that will be reached by activating fly mode - 
            // might want to adjust initial jump so that y will match initialFlyHeight
            if(transform.position.y > 30) {
                flying = true;
            }

        } else {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, -flyGravity * GetComponent<Rigidbody>().mass, 0));
        }

        grounded = false;
    }

    void OnCollisionStay()
    {
        grounded = true;
        flying = false;
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
}