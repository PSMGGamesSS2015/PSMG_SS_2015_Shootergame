// Script that handles the shooting of an arrow

using UnityEngine;
using System.Collections;

public class ShootArrow : MonoBehaviour {

    // Minimum and maximum intensity that can be reached by the arrow
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;

    // How fast the intensity should grow while bending the bow
    public float intensityGrowth = 1.0f;

    // Arrow prefab
    public GameObject arrowPrefab;

    // Arrow spawn point
    public Transform spawnPoint;

    // Current intensity, set automatically
    private float intensity;

    // Variable that determines if the player is bending the bow, set automatically
    private bool bending;

    void Awake()
    {
        intensity = minIntensity;
    }

	void Update () {
        CheckForButton();

        // If the player is bending the bow (determined by CheckForButton()), increase the current intensity
        if (bending)
        {
            IncreaseIntensity();
        }
	}

    // Checks for input
    void CheckForButton()
    {
        // Check if Fire button is down - only called once, so use a variable
        if (Input.GetButtonDown("Fire1"))
        {
            bending = true;
            intensity = minIntensity;
        }

        // Once Fire button is released, change bending back to false and shoot
        if (Input.GetButtonUp("Fire1"))
        {
            bending = false;
            Shoot();
        }
    }

    // Increases the current intensity
    void IncreaseIntensity()
    {
        // Increase the intensity
        intensity += intensityGrowth * Time.deltaTime;

        // Make sure the intensity does not exceed our limitations
        Mathf.Clamp(intensity, minIntensity, maxIntensity);
    }

    void Shoot()
    {
        // Get the direction of the camera to set the arrow rotation like this
        Transform camPos = GameObject.FindGameObjectWithTag("MainCamera").transform;
        
        // Instantiate a new arrow object and call its Shoot() function
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, camPos.rotation) as GameObject;
        arrow.GetComponent<ArrowBehaviour>().Shoot(intensity);

        // Revert intensity back to the minimum value
        intensity = minIntensity;
    }
}
