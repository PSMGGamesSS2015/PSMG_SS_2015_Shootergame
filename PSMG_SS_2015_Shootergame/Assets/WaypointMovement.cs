using UnityEngine;
using System.Collections;

// http://www.attiliocarotenuto.com/83-articles-tutorials/unity/292-unity-3-moving-a-npc-along-a-path

public class WaypointMovement : MonoBehaviour {

    public Transform[] waypoints;
    public float movementSpeed = 5.0f;
    public float minDistance = 5.0f;

    private Transform currentWaypoint;
    private int currentIndex;

	// Use this for initialization
	void Start () {
        currentIndex = 0;
        currentWaypoint = waypoints[currentIndex];
	}
	
	// Update is called once per frame
	void Update () {
        MoveToWaypoint();

        CheckIfWaypointReached();
	}

    void MoveToWaypoint()
    {
        Vector3 direction = currentWaypoint.transform.position - transform.position;
        Vector3 moveVector = direction.normalized * movementSpeed * Time.deltaTime;
        transform.position += moveVector;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 4 * Time.deltaTime);
    }

    void CheckIfWaypointReached()
    {
        if (Vector3.Distance(currentWaypoint.transform.position, transform.position) <= minDistance)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = 0;
            }

            currentWaypoint = waypoints[currentIndex];
        }
    }
}
