using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowFeathers : MonoBehaviour {
    BasePlayer player;
    public Text featherText;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<BasePlayer>();
	}
	
	// Update is called once per frame
	void Update () {
        featherText.text = player.getCurrentFeathers().ToString();
	}
}
