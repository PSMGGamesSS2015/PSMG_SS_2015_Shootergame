/* DESTROYABLE
 * A destroyable is an object that can be destroyed by the player using arrows and the tomahawk.
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.Weapons;

// Require a collider/trigger, because we need to know if an arrow hits the object
[RequireComponent(typeof(Collider))]
public class Destroyable : MonoBehaviour {

    // Shots needed for the object to be destroyed
    public int shotsNeeded = 3;
    // Should the game object be destroyed after it has been shot/hit?
    public bool destroyAfterShot = false;
    // Should the game object fall after it has been shot/hit?
    public bool fallAfterShot = false;

    // Distance between the player of the object in order to count an attack of the tomahawk as a hit
	public float tomahawkAttackDistance = 5.5f;

    // Allow destruction by using the tomahawk
	public bool allowTomahawkAttacks = true;
    // Allow destruction by using the bow/arrows
	public bool allowArrows = true;

    // Delay of the effects
	public float effectDelayTime = 1.0f;

    // Player object
    protected GameObject player;
    // "Health" of the object, equals remaining shots/hits needed
    protected int health;
    // Percentage (health/shotsNeeded)
    protected float pct;

	protected void Start () {
        // Health is the amount of shots/hits needed
        health = shotsNeeded;
        // Percentage is 1 at the startt
        pct = 1.0f;
        // Find the player game object
        player = GameObject.FindGameObjectWithTag("Player");

        // If the object should fall after it has been destroyed...
        if (fallAfterShot)
        {
            // Disable gravity (because obviously we don't want the object to fall before it has been destroyed)
            GetComponent<Rigidbody>().useGravity = false;
            // Freeze the object's position so that it doesn't float through the air after it has been hit by an arrow
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        }

        // If the object ban be destroyed using the tomahawk..
		if (allowTomahawkAttacks) {
            // Get the weapon controller
			WeaponController wpc = player.GetComponent<WeaponController>();
			// Get the Tomahawk weapon
            BaseWeapon tomahawk = wpc.getWeaponByName("Tomahawk");
			// Register the OnAttack function
            tomahawk.OnShotFired += new BaseWeapon.DOnWeaponInfoChanged(OnAttack);
		}
	}

    // If the player attacks using the tomahawk...
	void OnAttack(BaseWeapon w) {
        // Calculate the distance between the player and the object
		float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
		
        // Check if the distance is smaller than the set minimum distance
		if (distance <= tomahawkAttackDistance)
		{
            // Call the OnDamage() function
			OnDamage ();
		}
	}
	
    // If something enters the trigger...
    void OnTriggerEnter(Collider other)
    {
        // Check if arrows should be able to damage the object and check if the object is an arrow
        if (allowArrows && other.gameObject.tag == "Arrow")
        {
            // Get the ArrowBehaviour component
            Assets.Scripts.Weapons.ArrowBehaviour component = other.GetComponent<Assets.Scripts.Weapons.ArrowBehaviour>();

            // If it hasn't hit an object yet...
            if (component.HitObject() == false)
            {
                // ...it now has hit an object!
                component.ObjectHit();

                // Call the OnDamage() function
				OnDamage();
            }
            
        }
    }

    // If the object took damage...
	void OnDamage() {
        // Reduce its health
		health--;
        // Recalculate the percentage
		pct = (float)health / shotsNeeded;
		
        // Check if the health is 0 (which means the object has been destroyed)
		if (health == 0)
		{
            // Invoke the CheckEffects() method after the specified delay
			Invoke("CheckEffects", effectDelayTime);
            // Call the OnKill() method for children scripts
			OnKill();
		}
	}

    // Checks which effects should be applied
    void CheckEffects()
    {
        CheckFall();
        CheckDestroy();
    }

    // Checks if the object should fall
    void CheckFall()
    {
        // If the object should fall
        if (fallAfterShot)
        {
            // Remove the constraints to allow movement of the object
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            // Activate gravity
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    // Checks if the object should be destroyed
    void CheckDestroy()
    {
        // If the object should be destroyed
        if (destroyAfterShot)
        {
            // Shrink and destroy it!
            StartCoroutine(Shrink());
        }
    }

    // Shrink and destroy the object
    IEnumerator Shrink()
    {
        // While the object's scale is greater than 0
        while (transform.localScale.z >= 0.0f) {
            // Slowly shrink it!
            transform.localScale += new Vector3(-0.02f, -0.02f, -0.02f);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Destroy the game object once its scale is 0
        Destroy(gameObject);
    }

    // Get the health percentage
    public float GetHealth()
    {
        return pct;
    }

    // OnKill() method that can be overridden by children classes for additional effects after destroying the object 
    protected virtual void OnKill()
    {

    }
}
