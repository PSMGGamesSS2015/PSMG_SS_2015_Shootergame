using UnityEngine;
using System.Collections;

public class CollectFlower : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            BasePlayer component = other.GetComponent<BasePlayer>();

            if (component.getCurrentFlowers() < component.maxFlowers)
            {
                component.FlowerCollected();
                Destroy(gameObject);
            }     
        }
    }
}
