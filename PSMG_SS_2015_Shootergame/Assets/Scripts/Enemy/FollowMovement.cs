using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]

public class FollowMovement : MonoBehaviour {

	public Transform target;
	public float minDistance = 5.0f;

	private bool moving = true;

	private NavMeshAgent nav;

	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		if (moving) {
			if (Vector3.Distance(transform.position, target.position) >= minDistance) {
				nav.SetDestination(target.position);
			}
		}
	}

	public void StartMoving()
	{
		moving = true;
	}
	
	public void StopMoving()
	{
		moving = false;
	}
}
