/* FLAP
 * Flap is a collectable object that spawns in fly mode. Collecting it increases the amount of available flaps during the flight session by one.
 */

using UnityEngine;
using System.Collections;

public class Flap : Collectable {
	
    // Once object has been collected...
	protected override void OnCollect(Collider player) {
        // Get the player's movement component
		PlayerMovement component = player.GetComponent<PlayerMovement> ();
        // Add an additional flap!
		component.AddFlap ();
	}
}
