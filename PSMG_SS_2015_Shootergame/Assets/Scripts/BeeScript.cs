using UnityEngine;
using System.Collections;

public class BeeScript : MonoBehaviour {

	private GameObject player;

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Arrow") {
			GetComponent<Rigidbody> ().useGravity = true;
			GetComponent<ParticleSystem>().Play();
		}
	}

	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player");

	}

	void Update() {
		if (Vector3.Distance (player.transform.position, transform.position) <= 4.0f) {
			player.GetComponent<BasePlayer> ().SubtractHealth (1);
		} else {
			//bahahahahaha
		}
	}

}
