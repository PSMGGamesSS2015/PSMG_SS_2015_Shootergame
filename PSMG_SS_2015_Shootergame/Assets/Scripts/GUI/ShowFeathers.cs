using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowFeathers : MonoBehaviour {
    //the player
    private BasePlayer player;
    //the text element for the amount of feathers
    public Text featherText;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
	}
	
	// update the amount of feathers the player collected
	void Update () {
        featherText.text = player.getCurrentFeathers().ToString();
	}
}
