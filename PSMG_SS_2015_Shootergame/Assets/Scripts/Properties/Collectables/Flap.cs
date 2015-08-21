using UnityEngine;
using System.Collections;

public class Flap : Collectable {
	
	protected override void OnCollect(Collider player) {
		PlayerMovement component = player.GetComponent<PlayerMovement> ();
		component.AddFlap ();
	}
}
