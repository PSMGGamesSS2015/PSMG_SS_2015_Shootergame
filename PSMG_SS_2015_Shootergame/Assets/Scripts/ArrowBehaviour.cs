/* Every arrow is spawn with this script attached, so use this to control und manipulate every arrow object */

using UnityEngine;
using System.Collections;

public class ArrowBehaviour : MonoBehaviour {

    // force: The force with which the arrow will be shot
    public float force = 500.0f;

	// Called once when the arrow spawns
	void Start () {

	}
	
	// Called once every frame
	void Update () {
	
	}

    /* After instantiation, the arrow would just fall down at its spawnPoint - Shoot provides a function that can be called after an arrow was instantiated to actually shoot it.
     * Needs a float parameter intensity which is a multiplier for the force. Through intensity, the player can bend the bow for a longer time to shoot the arrow with a greater force */

    public void Shoot(float intensity)
    {
        // Get the Rigidbody component of the arrow and add a force to it in the direction that the arrow is currently facing
        GetComponent<Rigidbody>().AddForce(transform.forward * intensity * force);
    }
}
