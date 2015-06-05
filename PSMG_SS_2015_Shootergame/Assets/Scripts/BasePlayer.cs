using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Weapons;

public class BasePlayer : MonoBehaviour {
    /*PLAYER DATA*/
    //actual health of the player
    public int health = 100;

    //maximal health the player can have
    public const int MAX_HEALTH = 100;

    // Maximum amount of flowers
    public int maxFlowers = 5;

    // Current amount of flowers
    private int flowers = 1;

    public GameObject birdModel;

    private bool isInFlyMode = false;
    public bool FlyMode
    {
        get
        {
            return isInFlyMode;
        }
        set
        {
            isInFlyMode = value;
            if (isInFlyMode)
            {
                birdModel.GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                birdModel.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

	// Use this for initialization
	void Awake () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void FlowerCollected()
    {
        if (flowers < maxFlowers)
        {
            flowers++;
        }
    }

    public int getCurrentFlowers()
    {
        return flowers;
    }

    public void RemoveFlower()
    {
        if (flowers > 0)
        {
            flowers--;
        }
    }
}
