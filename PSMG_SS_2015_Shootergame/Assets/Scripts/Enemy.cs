using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    private int health = 100;

    private bool hasSpottedPlayer = false;

    private static List<GameObject> enemies = new List<GameObject>();

    private MovementController movementController;

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
                Enemy e = enemy.GetComponent<Enemy>();
                if (e.HasSpottedPlayer == false)
                    e.HasSpottedPlayer = true;
            }
        }
    }
    

    private BasePlayer player;
    private Vector3 playerPosition;

    // Position where the enemy is when following starts
    private Vector3 startingPosition;

    private bool goingHome = false;

    private Rigidbody body;

    public GameObject head;

    private NavMeshAgent nav;

    private float lastAttackTimestamp = 0;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        this.player = player.GetComponent<BasePlayer>();

        nav = GetComponent<NavMeshAgent>();

        movementController = GetComponent<MovementController>();

        enemies.Add(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        playerPosition = player.transform.position;

        float distance = Vector3.Distance(transform.position, playerPosition);

        if (hasSpottedPlayer)
        {
            transform.LookAt(playerPosition);

            if (distance > 5.0f && distance < 30.0f)
            {
                nav.SetDestination(playerPosition);

                movementController.OnFollowPlayer();
            }
            else if (distance > 30.0f)
            {
                HasSpottedPlayer = false;

                // Move back to where the enemy was when following started
                nav.SetDestination(startingPosition);

                goingHome = true;
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
                if (!goingHome)
                {
                    startingPosition = transform.position;
                }
                
                HasSpottedPlayer = true;
            }

            if (goingHome)
            {
                if (Vector3.Distance(transform.position, startingPosition) <= 3.0f)
                {
                    movementController.OnStopFollowing();
                    goingHome = false;
                }
            }
        }
	}


}
