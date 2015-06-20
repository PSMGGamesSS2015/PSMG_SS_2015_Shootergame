using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    private WaypointMovement waypointMovement;
    private Enemy enemy;

    private Vector3 savedPosition;

	// Use this for initialization
	void Start () {
        waypointMovement = GetComponent<WaypointMovement>();
        enemy = GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnFollowPlayer()
    {
        savedPosition = transform.position;
        waypointMovement.StopMoving();
    }

    public void OnStopFollowing()
    {
        waypointMovement.StartMoving();
    }
}
