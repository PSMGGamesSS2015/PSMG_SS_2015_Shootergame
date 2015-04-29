using UnityEngine;
using System.Collections;

public class animController : MonoBehaviour {

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Vertical")) {
			anim.SetBool ("walk", true);
			if (Input.GetButton ("Sprint")) {
				anim.SetBool("run", true);
			}
			else {
				anim.SetBool("run", false);
			}
		}



		else {
			anim.SetBool("walk", false);
			anim.SetBool ("run", false);
		}
	}
}
