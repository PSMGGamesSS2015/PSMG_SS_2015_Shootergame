using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    private int health = 100;

    private bool hasSpottedPlayer = false;

    private static List<GameObject> enemies = new List<GameObject>();

    public bool HasSpottedPlayer
    {
        get
        {
            return hasSpottedPlayer;
        }
        set
        {
            hasSpottedPlayer = value;
            if (hasSpottedPlayer == true)
            {
                head.GetComponent<MeshRenderer>().material.color = Color.red;
                notifyCloseEnemies();
            }
            else
            {
                head.GetComponent<MeshRenderer>().material.color = Color.white;
            }
        }
    }

    private void notifyCloseEnemies()
    {
        for (int i = 0; i < enemies.Count; i++) {
            GameObject enemy = enemies[i];

            if (enemy == this) continue;

            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < 10)
            {
                enemy.GetComponent<Enemy>().HasSpottedPlayer = true;
            }
        }
    }
    

    private BasePlayer player;
    private Transform playerPosition;

    private Rigidbody body;

    public GameObject head;

    private float lastAttackTimestamp = 0;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform;
        this.player = player.GetComponent<BasePlayer>();
        enemies.Add(gameObject);
	}
	
	// Update is called once per frame
	void Update () {

        float distance = Vector3.Distance(transform.position, playerPosition.position);

        if (hasSpottedPlayer)
        {
            transform.LookAt(playerPosition);

            

            if (distance > 5.0f && distance < 30.0f)
            {
                body.AddForce(transform.forward * 30);
            }
            else if (distance > 30.0f)
            {
                HasSpottedPlayer = false;
            }
            else
            {
                if (Time.time > lastAttackTimestamp + 1.0f)
                {
                    player.health -= 5;
                    lastAttackTimestamp = Time.time;
                }
            }
        }
        else
        {
            // try to spot player

            // if player is standing to close to enemy
            if (distance < 20.0f)
            {
                HasSpottedPlayer = true;
            }
        }
	}


}
