using UnityEngine;
using System.Collections;

public class hawk_test : MonoBehaviour {

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		anim.SetBool ("Flap", true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
