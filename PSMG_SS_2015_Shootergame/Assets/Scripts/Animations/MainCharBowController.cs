using UnityEngine;
using System.Collections;

public class MainCharBowController : MonoBehaviour {

	private Animator anim;



	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		bool walk = Input.GetButton ("Vertical");
		bool run = Input.GetButton ("Sprint");
		bool shot = Input.GetButton ("Fire1");
		bool jump = Input.GetButton ("Jump");

		anim.SetBool ("walk", walk);
		anim.SetBool ("run", run);
		anim.SetBool ("shot", shot);
		anim.SetBool ("jump", jump);

	}


}
