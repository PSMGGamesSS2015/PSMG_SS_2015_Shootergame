// Script that handles the shooting of an arrow

using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Bow : BaseWeapon{

    // Minimum and maximum intensity that can be reached by the arrow
    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;

    // How fast the intensity should grow while bending the bow
    public float intensityGrowth = 1.0f;

    // Arrow prefab
    private GameObject arrowPrefab;

    // Current intensity, set automatically
    private float intensity;

    // Variable that determines if the player is bending the bow, set automatically
    private bool bending;

    public Bow(GameObject parent) : base(parent)
    {
        intensity = minIntensity;
        arrowPrefab = parentPlayer.GetComponent<PlayerPrefabsController>().arrowPrefab;

        setBowSpecifications();
        
    }

    private void setBowSpecifications()
    {
        reserveAmmo = 20;
        magazinSize = 1;
        curAmmo = magazinSize;
    }

	public override void Update () {
        base.Update();

        // If the player is bending the bow (determined by CheckForButton()), increase the current intensity
        if (bending)
        {
            IncreaseIntensity();
        }
	}

    protected override void FireButtonDown()
    {
        bending = true;
        intensity = minIntensity;
    }

    // Increases the current intensity
    void IncreaseIntensity()
    {
        // Increase the intensity
        intensity += intensityGrowth * Time.deltaTime;

        // Make sure the intensity does not exceed our limitations
        Mathf.Clamp(intensity, minIntensity, maxIntensity);
    }

    protected override void Shoot()
    {
        base.Shoot();
        bending = false;

        // Get the direction of the camera to set the arrow rotation like this
        Transform camPos = GameObject.FindGameObjectWithTag("MainCamera").transform;
        
        // Instantiate a new arrow object and call its Shoot() function
        GameObject arrow = GameObject.Instantiate(arrowPrefab, bulletSpawn.position, camPos.rotation) as GameObject;
        arrow.GetComponent<ArrowBehaviour>().Shoot(intensity);

        // Revert intensity back to the minimum value
        intensity = minIntensity;
    }
}
