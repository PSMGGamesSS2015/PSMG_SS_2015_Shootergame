using UnityEngine;
using System.Collections;

public class ObjectRotation : MonoBehaviour {

    public float xRotationSpeed;
    public float yRotationSpeed;
    public float zRotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(xRotationSpeed * Time.deltaTime, yRotationSpeed * Time.deltaTime, zRotationSpeed * Time.deltaTime);
	
	}
}
