/* FOLLOW MOVEMENT
 * The game object will follow the specified target using a NavMeshAgent.
 * It will always keep a minimum distance to it.
 * If it gets separated too far from the target, it will teleport behind it.
 */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]

public class FollowMovement : MonoBehaviour {

    // Target that should be followed
	public Transform target;
	
    // Minimum distance to the target
    public float minDistance = 5.0f;
    
    // Maximum distance to the target
    public float maxDistance = 30.0f;

    // Bool that is true if the object should be moving
	private bool moving = true;

	private NavMeshAgent nav;

	void Start () {
		nav = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
        // If the object should be moving
		if (moving) {
            // Calculate the distance between the object and the target and check if it is greater than the minimum distance
			if (Vector3.Distance(transform.position, target.position) >= minDistance) {
                // Set the current position of the target as the new destination of the NavMeshAgent
				nav.SetDestination(target.position);
            }
            else
            {
                // If the object is too close to the target, reset the NavMeshAgent's path to stop moving
                nav.ResetPath();
            }

            // Now check if the object is too far away from the target
            CheckDistance();
		}
	}

    // Check if the object is too far away from the target
   private void CheckDistance()
    {
       // Check if the distance between the object and the target is greater than the maximum distance
        if (Vector3.Distance(transform.position, target.position) >= maxDistance)
        {
            // If true, teleport the object behind the target
            transform.position = target.transform.position - 2.0f * target.transform.forward;
        }
    }

    // Start moving
	public void StartMoving()
	{
		moving = true;
	}
	
    // Stop moving
	public void StopMoving()
	{
		moving = false;
	}
}
