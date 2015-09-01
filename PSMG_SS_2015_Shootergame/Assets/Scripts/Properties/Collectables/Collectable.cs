/* COLLECTABLE
 * A collectable is any game object that the player can collect.
 * The script provides methods to create different collectable objects with their own effects upon collection.
 */

using UnityEngine;
using System.Collections;

// Requires a collider (trigger) to work, because we're using OnTriggerEnter
[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour {

    // States if the object has been collected
    private bool collected = false;
    private EnviromentSound audioController;

    void Start()
    {
        audioController = GameObject.FindGameObjectWithTag("PlayerSound").GetComponent<EnviromentSound>();
    }

    // If someone enters the trigger...
    void OnTriggerEnter(Collider other)
    {
        // If the thing that entered the trigger is the player...
        if (other.gameObject.tag == "Player")
        {
            // ...play the collected sound
            audioController.playCollected();
            // Set collected to true
            collected = true;
            // Deactivate the game object
            gameObject.SetActive(false);
            // Call the OnCollect method
            OnCollect(other);
        }
    }

    // Reset the object
	public void Reset() {
        // Set the game object to active
		gameObject.SetActive (true);
        // Set collected to false
		collected = false;
	}

    // Returns a bool which is true if the object has been collected by the player
    public bool IsCollected()
    {
        return collected;
    }

    // Destroys the game object
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    // OnCollect method that can be used by child scripts to implement additional functionality upon collection
    protected virtual void OnCollect(Collider player)
    {
        
    }
}
