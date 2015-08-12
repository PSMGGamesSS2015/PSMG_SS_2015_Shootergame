using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Shootable))]
public class BeeScript : MonoBehaviour {

    public float playerDistance = 4.0f;
    public int damage = 1;

	private GameObject player;
    private Shootable shootable;
    
	void Start() {
		player = GameObject.FindGameObjectWithTag ("Player");
        shootable = GetComponent<Shootable>();
	}

	void Update() {
		if (Vector3.Distance (player.transform.position, transform.position) <= playerDistance) {
			player.GetComponent<BasePlayer> ().SubtractHealth (damage);
		} else {
			//bahahahahaha
		}
        if (shootable.getHealth() == 0.0f)
        {
            GetComponent<ParticleSystem>().Play();
        }
	}

}
