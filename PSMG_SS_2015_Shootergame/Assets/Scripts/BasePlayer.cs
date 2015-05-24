using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Weapons;

public class BasePlayer : MonoBehaviour {
    /*PLAYER DATA*/
    //actual health of the player
    public int health = 100;

    //maximal health the player can have
    private int maxHealth = 100;

    // Maximum amount of flowers
    public int maxFlowers = 5;

    // Current amount of flowers
    private int flowers = 1;

	// Use this for initialization
	void Awake () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public int getMaxHealth()
    {
        return maxHealth;
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
