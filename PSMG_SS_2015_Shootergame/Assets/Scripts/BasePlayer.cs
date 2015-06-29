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

    public float maxEnergy = 100.0f;

    public float energyRegeneration = 5.0f;
    public float energyDrain = 20.0f;

    public float sprintDelay = 5.0f;

    private float energy = 100.0f;

    public GameObject birdModel;

    private PlayerMovement movement;

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
        movement = GetComponent<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        if (movement.isSprinting())
        {
            DrainEnergy();
        }
        else
        {
            RegenerateEnergy();
        }

        Mathf.Clamp(energy, 0.0f, maxEnergy);
	}

    void DrainEnergy()
    {
        if (energy > 0.0f)
        {
            energy -= energyDrain * Time.deltaTime;
        }
        else
        {
            movement.canSprint = false;
            Invoke("AllowSprinting", sprintDelay);
        }
    }

    public void SubtractHealth(int value)
    {
        health -= value;
    }

    void AllowSprinting()
    {
        movement.canSprint = true;
    }

    void RegenerateEnergy()
    {
        if (energy < maxEnergy)
        {
            energy += energyRegeneration * Time.deltaTime;
        }
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

    public float getEnergy()
    {
        return energy;
    }
}
