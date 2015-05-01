using UnityEngine;
using System.Collections;

public class MinimapCamera : MonoBehaviour {

    // Target that should be followed
    public Transform followTarget;

    // y position (height) of the camera
    public float height = 150.0f;

    // If true, the camera will copy the target's y rotation
    public bool rotate = false;

    // Toggle if minimap is activated or not
    public bool active = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // If the minimap is activated...
        if (active)
        {
            // Set the camera component to true
            GetComponent<Camera>().enabled = true;

            // Position the camera above the player
            transform.position = new Vector3(followTarget.position.x, height, followTarget.position.z);

            // If we want the camera to rotate
            if (rotate)
            {
                // Use the target's y rotation and 90 degree for x rotation
                transform.eulerAngles = new Vector3(90, followTarget.rotation.y * 360, 0);
            }
        }
        // If the minimap is not activated
        else
        {
            // Set the camera component to false
            GetComponent<Camera>().enabled = false;
        }
        
	}
}
