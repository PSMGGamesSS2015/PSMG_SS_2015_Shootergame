using UnityEngine;
using System.Collections;

public class setLifeMode : MonoBehaviour {
    PlayerMovement playerMovement = new PlayerMovement();
	
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (playerMovement.getMode())
        {
            
        }
        //Debug.Log("flightMode: " + playerMovement.getMode());
	}
}
