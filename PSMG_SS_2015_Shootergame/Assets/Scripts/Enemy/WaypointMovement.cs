using UnityEngine;
using System.Collections;

// http://www.attiliocarotenuto.com/83-articles-tutorials/unity/292-unity-3-moving-a-npc-along-a-path
// http://unity3d.com/learn/tutorials/projects/survival-shooter/enemy-one

[RequireComponent(typeof(NavMeshAgent))]

public class WaypointMovement : MonoBehaviour {

    public Transform[] waypoints;

    public bool moveWithoutActivation;

    public bool loop = true;

    Transform currentWaypoint;
    int currentIndex;

    bool moving = false;

    NavMeshAgent nav;

    Transform saveWaypoint;
    int saveIndex;

    float minDistance = 2.0f;
    float distance;

	// Use this for initialization
	void Start () {
        currentIndex = 0;
        currentWaypoint = waypoints[currentIndex];

        if (moveWithoutActivation)
        {
            StartMoving();
        }
	}

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        distance = minDistance;
    }
	
	// Update is called once per frame
	void Update () {
        if (moving)
        {
            MoveToWaypoint();
            CheckIfWaypointReached();
            CheckIfStuck();
        }
	}

    void CheckIfStuck()
    {
        if (nav.velocity.sqrMagnitude == 0.0f)
        {
            Unstuck();
        }
        else
        {
            distance = minDistance;
        }
    }

    void Unstuck()
    {
        distance += 0.1f;
    }

    void MoveToWaypoint()
    {
        nav.SetDestination(currentWaypoint.transform.position);
    }

    void CheckIfWaypointReached()
    {
        if (Vector3.Distance(currentWaypoint.transform.position, transform.position) <= distance)
        {
            currentIndex++;

            if (currentIndex >= waypoints.Length)
            {
                currentIndex = 0;
                if (!loop)
                {
                    StopMoving();
                }
            }

            currentWaypoint = waypoints[currentIndex];
        }
    }

    public void StartMoving()
    {
        Debug.Log("start moving");
        moving = true;
    }

    public void StopMoving()
    {
        moving = false;
        nav.ResetPath();
    }

    public void ResetMovement()
    {
        StopMoving();
        currentIndex = 0;
        currentWaypoint = waypoints[currentIndex];
    }

    public void SaveWaypoint()
    {
        saveWaypoint = currentWaypoint;
        saveIndex = currentIndex;
    }

    public void LoadWaypoint()
    {
        currentWaypoint = saveWaypoint;
        currentIndex = saveIndex;

        distance = minDistance;
    }
}
