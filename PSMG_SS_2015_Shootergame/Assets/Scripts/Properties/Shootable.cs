using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Shootable : MonoBehaviour {

    public int shotsNeeded = 3;
    public bool destroyAfterShot = false;
    public bool fallAfterShot = false;

    private GameObject player;
    private int health;
    private float pct;

	// Use this for initialization
	void Start () {
        health = shotsNeeded;
        player = GameObject.FindGameObjectWithTag("Player");

        if (fallAfterShot)
        {
            GetComponent<Rigidbody>().useGravity = false;
        }
	}
	
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter!");
        if (other.gameObject.tag == "Arrow")
        {

            health--;
            health = health / shotsNeeded;

            if (health == 0)
            {
                checkEffects();
            }
        }
    }

    void checkEffects()
    {
        CheckFall();
        CheckDestroy();
    }

    void CheckFall()
    {
        if (fallAfterShot)
        {
            Debug.Log("falling now!");
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    void CheckDestroy()
    {
        if (destroyAfterShot)
        {
            Destroy(gameObject);
        }
    }

    public float getHealth()
    {
        return pct;
    }
}
