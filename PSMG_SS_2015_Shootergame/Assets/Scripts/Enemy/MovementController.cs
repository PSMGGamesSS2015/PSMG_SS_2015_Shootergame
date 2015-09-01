using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    private WaypointMovement waypointMovement;
	private FollowMovement followMovement;

    private enum MODE
    {
        Follow,
        Waypoint
    }
	private MODE mode;

	// Use this for initialization
	void Start () {
        waypointMovement = GetComponent<WaypointMovement>();
		followMovement = GetComponent<FollowMovement> ();

		if (waypointMovement == null) {
			mode = MODE.Follow;
		} else if (followMovement == null) {
			mode = MODE.Waypoint;
		}
	}

    public void OnFollowPlayer()
    {
		StopMoving ();
    }

	void StopMoving() {
		if (mode == MODE.Follow) {
			followMovement.StopMoving ();
		} else if (mode == MODE.Waypoint) {
			waypointMovement.StopMoving ();
		}
	}

    public void OnStopFollowing()
    {
		if (mode == MODE.Follow) {
			followMovement.StartMoving ();
		} else if (mode == MODE.Waypoint) {
			waypointMovement.StartMoving ();
		}
    }
}
