using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Shootable : MonoBehaviour {

    public int shotsNeeded = 3;
    public bool destroyAfterShot = false;
    public bool fallAfterShot = false;

    protected GameObject player;
    protected int health;
    protected float pct;

	// Use this for initialization
	protected void Start () {
        health = shotsNeeded;
        pct = 1.0f;
        player = GameObject.FindGameObjectWithTag("Player");

        if (fallAfterShot)
        {
            GetComponent<Rigidbody>().useGravity = false;
        }
	}
	
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arrow")
        {

            health--;
            pct = health / shotsNeeded;

            if (health == 0)
            {
                CheckEffects();
                OnKill();
            }
        }
    }

    void CheckEffects()
    {
        CheckFall();
        CheckDestroy();
    }

    void CheckFall()
    {
        if (fallAfterShot)
        {
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

    public float GetHealth()
    {
        return pct;
    }

    protected virtual void OnKill()
    {

    }
}
