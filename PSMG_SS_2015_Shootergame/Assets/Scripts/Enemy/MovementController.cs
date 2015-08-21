using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    private WaypointMovement waypointMovement;
	private FollowMovement followMovement;
    private Enemy enemy;

    private Vector3 savedPosition;

	private string mode;

	// Use this for initialization
	void Start () {
        waypointMovement = GetComponent<WaypointMovement>();
		followMovement = GetComponent<FollowMovement> ();

		if (waypointMovement == null) {
			mode = "follow";
		} else if (followMovement == null) {
			mode = "waypoint";
		}

        enemy = GetComponent<Enemy>();
	}

    public void OnFollowPlayer()
    {
        savedPosition = transform.position;
		StopMoving ();
    }

	void StopMoving() {
		if (mode == "follow") {
			followMovement.StopMoving ();
		} else if (mode == "waypoint") {
			waypointMovement.StopMoving ();
		}
	}

    public void OnStopFollowing()
    {
		if (mode == "follow") {
			followMovement.StartMoving ();
		} else if (mode == "waypoint") {
			waypointMovement.StartMoving ();
		}
    }
}
