using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    private int health = 100;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            if (health <= 0)
            {
                // Initiate death
                Debug.LogError("enemy died");
            }
        }
    }

    private bool hasSpottedPlayer = false;

    public static List<GameObject> enemies = new List<GameObject>();

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
            Debug.Log("SpottedPlayer: " + value);
            if (hasSpottedPlayer == true)
            {
                //head.GetComponent<MeshRenderer>().material.color = Color.red;
                notifyCloseEnemies();
            }
            else
            {
                //head.GetComponent<MeshRenderer>().material.color = Color.white;
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


    private GameObject playerObject;
    private BasePlayer player;
	private static QuestManager questManager;
    private Vector3 playerPosition;

    // Position where the enemy is when following starts
    private Vector3 startingPosition;

    private bool goingHome = false;

    private NavMeshAgent nav;

    private float lastAttackTimestamp = 0;

    private const float fieldOfView = 110f;
    private const float MAX_DISTANCE_TO_SIGHT = 30.0f;
	// Use this for initialization
	void Start () {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        
        this.player = playerObject.GetComponent<BasePlayer>();

        nav = GetComponent<NavMeshAgent>();

        movementController = GetComponent<MovementController>();

		questManager = GameObject.FindGameObjectWithTag ("QuestManager").GetComponent<QuestManager> ();

        enemies.Add(gameObject);
	}

	public void Reset() {
		foreach (GameObject o in enemies) {
			o.GetComponent<Enemy>().ResetValues();
		}
	}

	public void ResetValues() {
		health = 100;
		hasSpottedPlayer = false;
		goingHome = false;
	}
	
	// Update is called once per frame
	void Update () {
        playerPosition = playerObject.transform.position;

        float distance = Vector3.Distance(transform.position, playerPosition);
        Vector3 directionToPlayer = playerPosition - transform.position;
        if (hasSpottedPlayer)
        {
            //transform.LookAt(playerPosition);
            Quaternion rotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = rotation;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

            if (distance > 5.0f && distance < MAX_DISTANCE_TO_SIGHT)
            {
                //movementController.OnFollowPlayer();
                nav.SetDestination(playerPosition);
            }
            else if (distance > MAX_DISTANCE_TO_SIGHT)
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

            if ((distance < 10.0f) || // if player is standing too close to enemy
                (Vector3.Angle(directionToPlayer, transform.forward) < (fieldOfView * 0.5f))) // if player is in field of view 
            {
                // Player maybe spotted but make sure to check if something is blocking sight:
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer.normalized, out hit, MAX_DISTANCE_TO_SIGHT))
                {
                    if (hit.collider.gameObject == playerObject)
                    {
                        // Player wasn't too far away and nothing in between to block the view.
                        if (!goingHome)
                        {
                            startingPosition = transform.position;
                        }

                        HasSpottedPlayer = true;
						questManager.OnPlayerSpotted();
                    }
                }
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
