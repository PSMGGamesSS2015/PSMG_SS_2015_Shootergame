/* BEEHIVE
 * BeeHive is a special destroyable object that enables a particle system upon destruction
 */

using UnityEngine;
using System.Collections;

public class BeeHive : Destroyable {

    // Distance at which the player takes damage from the aggressive bees!
    public float playerDistance = 4.0f;
    // Damage that the player is taking while standing too close to the hive.
    public int damage = 1;
    
	new void Start() {
        base.Start();
	}

	void Update() {
        // Check if the player is close to the hive
		if (Vector3.Distance (player.transform.position, transform.position) <= playerDistance) {
            // Substract health
			player.GetComponent<BasePlayer> ().SubtractHealth (damage);
		} else {
			//bahahahahaha
		}
	}

    // Once the hive has been "killed"
    protected override void OnKill()
    {
        // Play the particle system
        GetComponent<ParticleSystem>().Play();
    }

}
