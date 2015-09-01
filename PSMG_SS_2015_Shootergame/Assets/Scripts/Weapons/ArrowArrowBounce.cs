using UnityEngine;
using System.Collections;

public class ArrowArrowBounce : MonoBehaviour {
	private float size;

	// Use this for initialization
	void Start () {
		size = transform.localScale.x;
		Debug.Log (size);
	}
	
	// Update is called once per frame
	void Update () {
		if (size < 0.2f) {
			transform.localScale += new Vector3(0.1f,0.1f,0.1f);
		} else if (size > 1.0f) {
			transform.localScale += new Vector3(-0.1f,-0.1f,-0.1f);
		}
	
	}
}
