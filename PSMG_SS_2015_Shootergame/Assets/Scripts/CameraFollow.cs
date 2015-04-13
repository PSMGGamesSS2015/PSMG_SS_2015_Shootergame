using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    Vector3 offset;

	void Start () {
        offset = transform.position - target.position;
	}
	
	void FixedUpdate () {
        Vector3 targetCamPos = target.position + offset;
        transform.position = targetCamPos;
	}
}
