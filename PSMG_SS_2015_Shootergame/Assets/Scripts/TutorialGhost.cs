using UnityEngine;
using System.Collections;

public class TutorialGhost : MonoBehaviour {

    /// <summary>
    /// Maximum distance that the ghost can be away from the player
    /// </summary>
    public float maxDistance = 50.0f;

    /// <summary>
    /// Seconds after which the ghost returns to the player if he hasn't reached the goal yet
    /// </summary>
    public float returnTime = 30.0f;

    private NavMeshAgent nav;
    private Vector3 goal;
    private Transform player;

	// Use this for initialization
	void Start () {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Vector3.Distance(transform.position, player.transform.position) <= maxDistance) {
            nav.SetDestination(goal);
        }
        else
        {
            nav.ResetPath();
        }
	}

    public void SetGoal (Vector3 position)
    {
        goal = position;
    }
}
