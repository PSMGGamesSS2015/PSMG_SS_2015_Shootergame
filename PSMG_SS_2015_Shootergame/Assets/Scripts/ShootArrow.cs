using UnityEngine;
using System.Collections;

public class ShootArrow : MonoBehaviour {

    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;

    public float intensityGrowth = 1.0f;

    public GameObject arrowPrefab;
    public Transform spawnPoint;

    private float intensity;
    private bool bending;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(intensity);

        CheckForButton();

        if (bending)
        {
            IncreaseIntensity();
        }
	}

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

    void IncreaseIntensity()
    {
        if (intensity < maxIntensity)
        {
            intensity += intensityGrowth * Time.deltaTime;

            // If intensity is larger than the set maxIntensity, set it to maxIntensity
            if (intensity > maxIntensity)
            {
                intensity = maxIntensity;
            }
        }
    }

    void Shoot()
    {
        // Instantiate a new arrow object and call its Shoot() function
        GameObject arrow = Instantiate(arrowPrefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
        arrow.GetComponent<ArrowBehaviour>().Shoot(intensity);

        intensity = 0.0f;
    }
}
