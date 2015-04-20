using UnityEngine;
using System.Collections;

public class ArrowBehaviour : MonoBehaviour {

    public float force = 500.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shoot(float intensity)
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * intensity * force);
    }
}
