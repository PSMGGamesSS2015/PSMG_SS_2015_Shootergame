using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Weapons;
using UnityStandardAssets.ImageEffects;

public class BasePlayer : MonoBehaviour {
    /*PLAYER DATA*/
    //actual health of the player
    public int health = 100;

    //maximal health the player can have
    public const int MAX_HEALTH = 100;

    // Maximum amount of Feathers
    public int maxFeathers = 5;

    // Current amount of Feathers
    private int feathers = 1;

    public const float MAX_ENERGY = 100.0f;

    public float energyRegeneration = 5.0f;
    public float energyDrain = 20.0f;
    DepthOfField lowEnergyBlur = null;

    public float sprintDelay = 5.0f;

    private float energy = 100.0f;

    public GameObject birdModel;

    private PlayerMovement movement;

    public ParticleSystem birdMorphEffect;

    private Vector3 homePosition;
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
                Camera.main.transform.localPosition = new Vector3(0, 0, -10.0f);
                RemoveFeather();
                birdMorphEffect.enableEmission = false;
            }
            else
            {
                birdModel.GetComponent<MeshRenderer>().enabled = false;
                Camera.main.transform.localPosition = homePosition;
            }
        }
    }

	// Use this for initialization
	void Awake () {
        birdMorphEffect.enableEmission = false;
        homePosition = Camera.main.transform.localPosition;
        movement = GetComponent<PlayerMovement>();
        lowEnergyBlur = Camera.main.GetComponent<DepthOfField>();
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

        
        Mathf.Clamp(energy, 0.0f, MAX_ENERGY);

        float blurValue = Mathf.Lerp(0.0f, lowEnergyBlur.maxBlurSize, energy / MAX_ENERGY);
        lowEnergyBlur.focalSize = blurValue;
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
        if (energy < MAX_ENERGY)
        {
            energy += energyRegeneration * Time.deltaTime;
        }
    }

    public void FeatherCollected()
    {
        if (feathers < maxFeathers)
        {
            feathers++;
        }
    }

    public int getCurrentFeathers()
    {
        return feathers;
    }

    public void RemoveFeather()
    {
        if (feathers > 0)
        {
            feathers--;
        }
    }

    public float getEnergy()
    {
        return energy;
    }

    public void ActivateSprintBonus()
    {
        energyRegeneration *= 2.0f;
        energyDrain /= 2.0f;
        sprintDelay /= 2.0f;
    }

    public void DeactivateSprintBonus()
    {
        energyRegeneration /= 2.0f;
        energyDrain *= 2.0f;
        sprintDelay *= 2.0f;
    }
}
