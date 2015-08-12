using UnityEngine;
using System.Collections;

public class BonusTotem : MonoBehaviour {

    public float activatingTime = 5.0f;

    public float activationRange = 10.0f;

    public bool showIndicator = false;

    private bool active = false;
    private float activationState = 0.0f;
    private float activeTime = 0.0f;

    private Transform player;
    private PrefabManager prefabs;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        prefabs = GameObject.FindGameObjectWithTag("Prefabs").GetComponent<PrefabManager>();

        if (showIndicator)
        {
            GameObject marker = Instantiate(prefabs.rangeIndicator, transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
            marker.transform.parent = transform;
            marker.GetComponent<Projector>().orthographicSize = activationRange;
            marker.GetComponentInChildren<ParticleSystem>().Stop();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
