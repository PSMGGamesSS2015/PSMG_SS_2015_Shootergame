/* SCRIPT IS NOT BEING USED! IRRELEVANT */

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

    public float baseSpeed = 3.5f;

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
        float playerDistance = Vector3.Distance(player.position, goal);
        float ghostDistance = Vector3.Distance(transform.position, goal);

        float factor = 1 / Mathf.Log10(Vector3.Distance(transform.position, player.transform.position));

        factor *= Mathf.Pow(ghostDistance / playerDistance, 2);

        nav.speed = baseSpeed * factor;
	}

    public void SetGoal (Vector3 position)
    {
        goal = position;

        nav.SetDestination(goal);
    }
}
