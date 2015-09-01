/* MOVEMENT CONTROLLER
 * Manages the different movement scripts that might be applied to an object.
 * Ensures that only one movement script at a time is responsible for movement to prevent overriding.
 */

using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    // Waypoint Movement script
    private WaypointMovement waypointMovement;
    // Follow Movement scriüt
	private FollowMovement followMovement;

    // Movement modes
    private enum MODE
    {
        None,
        Follow,
        Waypoint
    }
	private MODE mode;

	void Start () {
        // Get the components
        waypointMovement = GetComponent<WaypointMovement>();
		followMovement = GetComponent<FollowMovement> ();
        mode = MODE.None;

        // Set the movement mode
		if (waypointMovement != null) {
            mode = MODE.Waypoint;
		} else if (followMovement != null) {
			mode = MODE.Follow;
		}
	}

    // OnFollowPlayer is called once the Enemy script spots the player and starts following it
    public void OnFollowPlayer()
    {
		StopMoving ();
    }

    // Stop Moving
	void StopMoving() {
		// Check which mode is active and stop the corresponding script
        if (mode == MODE.Follow) {
			followMovement.StopMoving ();
		} else if (mode == MODE.Waypoint) {
			waypointMovement.StopMoving ();
		}
	}

    // OnStopFollowing is called once the Enemy script does not spot/follow the player anymore
    public void OnStopFollowing()
    {
        // Check which mode is active and start the corresponding script
		if (mode == MODE.Follow) {
			followMovement.StartMoving ();
		} else if (mode == MODE.Waypoint) {
			waypointMovement.StartMoving ();
		}
    }
}
