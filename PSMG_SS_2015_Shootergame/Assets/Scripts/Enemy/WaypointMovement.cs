/* WAYPOINT MOVEMENT
 * Using an array of waypoints (any game object, including empties), the object will move to every waypoint in the defined order.
 * SOURCES
 * http://www.attiliocarotenuto.com/83-articles-tutorials/unity/292-unity-3-moving-a-npc-along-a-path
 * http://unity3d.com/learn/tutorials/projects/survival-shooter/enemy-one
 */

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]

public class WaypointMovement : MonoBehaviour {

    // The waypoints in the order that they should be visited
    public Transform[] waypoints;

    // Set to TRUE if the object should move as soon as the game starts, without external activation
    public bool moveWithoutActivation;

    // Set to TRUE if the object should go to the first waypoint again after it has reached the last one
    public bool loop = true;

    // The waypoint that the object is currently moving towards
    private Transform currentWaypoint;
    // The index in the waypoints array of the current waypoint
    private int currentIndex;

    // If true, the object is moving
    private bool moving = false;

    // Nav Mesh Agent
    private NavMeshAgent nav;

    // Save the current waypoint
    private Transform saveWaypoint;
    // Save the index of the current waypoint
    private int saveIndex;

    // Minimum distance to the waypoint to consider it as reached
    float minDistance = 2.0f;
    /* Actual distance that is being used to determine if the waypoint has been reached - changes dynamically to allow unstucking the object
     * in case the waypoint is or has become unreachable */
    float distance;

	void Start () {
        // Set the current index to 0 and retrieve the corresponding waypoint
        currentIndex = 0;
        currentWaypoint = waypoints[currentIndex];

        // Get the NavMeshAgent
        nav = GetComponent<NavMeshAgent>();
        // Initialize distance as minDistance
        distance = minDistance;

        // If moveWithoutActivation is true...
        if (moveWithoutActivation)
        {
            // ...start moving!
            StartMoving();
        }
	}
    	
	void Update () {
        // If the object should be moving...
        if (moving)
        {
            MoveToWaypoint();
            CheckIfWaypointReached();
            CheckIfStuck();
        }
	}

    // Move to the next waypoint...
    void MoveToWaypoint()
    {
        // ...by setting the destination of the NavMeshAgent to the position of the current waypoint
        nav.SetDestination(currentWaypoint.transform.position);
    }

    // Check if a waypoint has been reached
    void CheckIfWaypointReached()
    {
        // Calculate the distance between the object and the current waypoint and check if it is lower than "distance"
        if (Vector3.Distance(currentWaypoint.transform.position, transform.position) <= distance)
        {
            // If true, the waypoint has been reached!
            WaypointReached();
        }
    }

    // Set the next waypoint
    void WaypointReached()
    {
        // Increase the index
        currentIndex++;

        // Check if the index is greater than the amount of waypoints
        if (currentIndex >= waypoints.Length)
        {
            // If true, the object has visited all waypoints! Reset the current index to 0
            currentIndex = 0;

            // If we don't want the object to loop the movement...
            if (!loop)
            {
                // ... stop moving!
                StopMoving();
            }
        }

        // Set the current waypoint (which is the first element of the array)
        currentWaypoint = waypoints[currentIndex];
    }

    // Check if the object is stuck
    void CheckIfStuck()
    {
        // Check if the velocity of the object is 0, which means that it does not move anymore
        if (nav.velocity.sqrMagnitude == 0.0f)
        {
            // If the velocity is 0, the object must have been stuck, so unstuck it!
            Unstuck();
        }
        else
        {
            // If the velocity is anything greater than 0, the object is obviously not stuck, so we can reset the distance variable to minDistance
            distance = minDistance;
        }
    }

    // Unstuck the object
    void Unstuck()
    {
        // Slowly increase the distance needed to reach the next waypoint
        distance += 1.0f * Time.deltaTime;
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
        nav.ResetPath();
    }

    // Reset the movement
    public void ResetMovement()
    {
        // Stop moving
        StopMoving();
        // Set index to 0
        currentIndex = 0;
        // Set current waypoint to the first element in the array
        currentWaypoint = waypoints[currentIndex];
    }

    // Save the data of the current waypoint
    public void SaveWaypoint()
    {
        saveWaypoint = currentWaypoint;
        saveIndex = currentIndex;
    }

    // Load the data of the saved waypoint
    public void LoadWaypoint()
    {
        currentWaypoint = saveWaypoint;
        currentIndex = saveIndex;

        distance = minDistance;
    }
}
