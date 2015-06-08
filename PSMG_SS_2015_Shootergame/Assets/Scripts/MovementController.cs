/* !!!!!!!!!!!!!!!
 * !!!!!!!!!!!!!!!
 * NOT WORKING YET
 * NOT WORKING YET
 * !!!!!!!!!!!!!!!
 !!!!!!!!!!!!!!!*/

using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    private WaypointMovement waypointMovement;
    private Enemy enemy;

	// Use this for initialization
	void Start () {
        waypointMovement = GetComponent<WaypointMovement>();
        enemy = GetComponent<Enemy>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void followPlayer()
    {
        // waypointMovement.StopMoving();
    }

    public void goHome()
    {
        // waypointMovement.GoHome();
    }
}
