/* TOTEM
 * A totem is any game object that can be activated by the player by standing next to it for a while.
 * It is automatically being deactivated after a specified time.
 * Using additional scripts, totems can have more functionality that is explained in these scripts.
 */

using UnityEngine;
using System.Collections;

public class Totem : MonoBehaviour {

    // Is the totem a bonus totem? Bonus totems can only be activated if a defined amount of "normal" totems are active at the same time. They have additional effects after they have been activated.
    public bool bonusTotem = false;

    // Color that the totem has if it is active
    public Color activatedColor = Color.red;

    // Time that the totem is active after it has been activated
    public float activatedTime = 360.0f;
    // Time that the player needs to stand close to the totem in order to activate it
    public float activatingTime = 5.0f;

    // Range at which the totem starts activating itself
    public float activationRange = 10.0f;

    // Should the totem have an indicator around it?
    public bool showIndicator = false;

    // Is the totem active?
    private bool active = false;
    // "State" of the activation - slowly increases if the player is standing close and decreases if the player is without reach of the totem
    private float activationState = 0.0f;
    // Time that the totem has been active
    private float activeTime = 0.0f;

    // Player transform
    private Transform player;
    // Prefab manager
    private PrefabManager prefabs;

    // Can the totem be activated?
    protected bool activatable = false;

    // Base Color of the totem
    private Color baseColor;

    // Audio script
    private new EnviromentSound audio;

	protected void Start () {
        // Find and get all of the components
        player = GameObject.FindGameObjectWithTag("Player").transform;
        prefabs = GameObject.FindGameObjectWithTag("Prefabs").GetComponent<PrefabManager>();
        audio = GameObject.FindGameObjectWithTag("PlayerSound").GetComponent<EnviromentSound>();

        // Save the totem's initial color
        baseColor = GetComponent<MeshRenderer>().material.color;

        // If an indicator should be created...
        if (showIndicator)
        {
            // Instantiate the range indicator object at the totem's position
            GameObject marker = Instantiate(prefabs.rangeIndicator, transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
            // ...and set it as a child of the totem
            marker.transform.parent = transform;
            // Set the projector's size
            marker.GetComponent<Projector>().orthographicSize = activationRange;
            // Stop the particle system of the indicator
            marker.GetComponentInChildren<ParticleSystem>().Stop();
        }
	}
	
	protected void Update () {
        // If the totem is active AND not a bonus totem
        if (active && !bonusTotem)
        {
            // Reduce the remaining time
            DrainTime();
        }
        else
        {
            // Totem is either inactive, a bonus totem or both - check if the totem is NOT a bonus totem OR activateable
            if (!bonusTotem || activatable)
            {
                // Check for activation of the totem
                CheckForActivation();
            }
        }

        // Set the color of the totem
        MakeColor();
	}

    // Changes the color of the totem based on its activation state
    void MakeColor()
    {
        // If the totem is active...
        if (active)
        {
            // Slowly fade out the color effect
            GetComponent<MeshRenderer>().material.color = Color.Lerp(baseColor, activatedColor, getPct());
        }
        else
        {
            // Slowly fade in the color effect
            GetComponent<MeshRenderer>().material.color = Color.Lerp(baseColor, activatedColor, activationState / activatingTime);
        }
    }

    // Reduce the remaining time of the totem
    void DrainTime()
    {
        // Reduce the activeTime by the time elapsed since the method has been called the last time
        activeTime -= Time.deltaTime;
        // Make sure the time does not exceed 0 or the maximum time
        Mathf.Clamp(activeTime, 0.0f, activatedTime);

        // If the active time has expired...
        if (activeTime <= 0.0f)
        {
            // ...deactivate the totem
            Deactivate();
        }
    }

    // Check for activation of the totem
    void CheckForActivation()
    {
        // If the player is close enough...
        if (Vector3.Distance(player.position, transform.position) <= activationRange)
        {
            // ...play the totem sound
            audio.playTotem();
            // Increase the activation state
            activationState += Time.deltaTime;
        }
        // If the player is too far away and activation state is not 0 yet
        else if (activationState > 0.0f)
        {
            // Reduce the activation state
            activationState -= Time.deltaTime;
            // Stop the totem sound
            audio.pauseTotem();
        }

        // Make sure the activation state cannot exceed its min/max values
        Mathf.Clamp(activationState, 0.0f, activatingTime);

        // Check if the activation state is greater than the needed time for activation
        if (activationState >= activatingTime)
        {
            // Set the totem to active
            active = true;
            // Reset activationState
            activationState = 0.0f;
            // Set the active time to the maximum value
            activeTime = activatedTime;
            // Play sounds
            audio.playCollected();
            audio.playTreeHit();
            // If the totem is a bonus totem
            if (bonusTotem)
            {
                // Play another sound
                audio.playReached();
            }
            // Stop the sounds again
            audio.stopTotem();
            // Call the overridable OnActivation() method
            OnActivation();
        }
    }

    // Returns a bool of the active state
    public bool IsActive()
    {
        return active;
    }

    // Sets the totum (de-)activatable
    public void SetActivatable(bool b)
    {
        activatable = b;
    }

    // Deactivate the totem
    public void Deactivate()
    {
        active = false;
        OnDeactivation();
    }

    // Get the percentage of the remaining time
    public float getPct()
    {
        return activeTime / activatedTime;
    }
    
    protected virtual void OnActivation()
    {

    }

    protected virtual void OnDeactivation()
    {

    }
}
