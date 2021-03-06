﻿// Script that handles looking with the mouse

using UnityEngine;
using System.Collections;

// Require a rigidbody on the player object
[RequireComponent(typeof(Rigidbody))]

public class MouseLook : MonoBehaviour
{

    // Mouse sensitivity for both directions
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    // Minimum and maximum angle for the rotation that can be reached by the mouse movement in X direction
    public const float minimumX = -360F;
    public const float maximumX = 360F;

    // Minimum and maximum angle for the rotation that can be reached by the mouse movement in Y direction
    public const float minimumY = -60F;
    public const float maximumY = 60F;

    // The target X and Y rotations - set automatically
    private float rotationX = 0F;
	private float rotationY = 0F;

    private Rigidbody rigid;

    // original Rotation of the object - set automatically
    private Quaternion originalRotation;

    public GameController gameController;

    public GameObject cameraRotationParent;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        // Make the rigid body not change rotation
        rigid.freezeRotation = true;

		Cursor.visible = false;

        // Save the current rotation
        originalRotation = cameraRotationParent.transform.localRotation;
    }

    void Update()
    {
        if (gameController.state == GameController.GameState.INGAME)
        {
            // Read the mouse input axis
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            // Call the ClampAngle function to make sure the angles don't exceed our set limitations
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            // Create a quaternion out of the calculated rotation
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);



            // Set the object's rotation to the computed new rotation
            rigid.transform.localRotation = originalRotation * xQuaternion;
            cameraRotationParent.transform.localRotation = originalRotation * yQuaternion;
        }
    }

	public float GetRotationY() {
		return rotationY;
	}

    // Makes sure the angle is always in our set limitations and doesnt exceed +/- 360°
    public static float ClampAngle(float angle, float min, float max)
    {
        // Set angle to +360° if it is at -360° and vice versa
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        // Return the angle if it is in our limitations, otherwise the minimum/maximum value
        return Mathf.Clamp(angle, min, max);
    }

}
