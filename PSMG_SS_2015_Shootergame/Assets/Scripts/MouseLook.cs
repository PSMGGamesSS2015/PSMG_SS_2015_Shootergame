// Script that handles looking with the mouse. Supports both X and Y but can also just use one of both axes.

using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

    // Set the rotation axes to either both x AND y or just one of both
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;

    // Mouse sensitivity for both directions
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    // Minimum and maximum angle for the rotation that can be reached by the mouse movement in X direction
    public float minimumX = -360F;
    public float maximumX = 360F;

    // Minimum and maximum angle for the rotation that can be reached by the mouse movement in Y direction
    public float minimumY = -60F;
    public float maximumY = 60F;

    // The target X and Y rotations - set automatically
    float rotationX = 0F;
    float rotationY = 0F;

    // original Rotation of the object - set automatically
    Quaternion originalRotation;

    void Start()
    {
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
            
        // Save the current rotation
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // Called if BOTH axes should be manipulated
        if (axes == RotationAxes.MouseXAndY)
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
            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }

        // Called if just the X axis should be manipulated
        else if (axes == RotationAxes.MouseX)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }
        
        // Called if just the Y axis should be manipulated
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
            transform.localRotation = originalRotation * yQuaternion;
        }
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
         return Mathf.Clamp (angle, min, max);
     }

}
