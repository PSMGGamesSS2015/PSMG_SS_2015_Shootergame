using UnityEngine;
using System.Collections;

public class BeeHive : Shootable {

    public float playerDistance = 4.0f;
    public int damage = 1;
    
	new void Start() {
        base.Start();
	}

	void Update() {
		if (Vector3.Distance (player.transform.position, transform.position) <= playerDistance) {
			player.GetComponent<BasePlayer> ().SubtractHealth (damage);
		} else {
			//bahahahahaha
		}
	}

    protected override void OnKill()
    {
        GetComponent<ParticleSystem>().Play();
    }

}
