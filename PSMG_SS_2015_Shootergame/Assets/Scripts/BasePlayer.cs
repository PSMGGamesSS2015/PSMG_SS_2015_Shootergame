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

    private QuestManager questManager;

    public const float MAX_ENERGY = 100.0f;

    public float energyRegeneration = 5.0f;
    public float energyDrain = 20.0f;
    DepthOfField lowEnergyBlur = null;

    public float sprintDelay = 5.0f;

    private float energy = 100.0f;

    public GameObject birdModel;

    private PlayerMovement movement;

    public ParticleSystem birdMorphEffect;

    private Transform cameraParent;
    private Vector3 cameraStandardPosition;

    private int featherSave = 0;

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
                birdModel.SetActive(true);
                //Camera.main.transform.localPosition = new Vector3(0, 0, -10.0f);
                RemoveFeather();
                birdMorphEffect.enableEmission = false;
                Camera.main.transform.parent.gameObject.GetComponent<SmoothThirdPersonCamera>().enabled = true;
                Camera.main.transform.parent.parent = null;
            }
            else
            {
                //birdModel.GetComponent<MeshRenderer>().enabled = false;
                birdModel.SetActive(false);
                Camera.main.transform.parent.gameObject.GetComponent<SmoothThirdPersonCamera>().enabled = false;
                //Camera.main.transform.localPosition = homePosition;
                Camera.main.transform.parent.parent = transform;
                Camera.main.transform.parent.localPosition = Vector3.zero;
                Camera.main.transform.parent.localRotation = new Quaternion(0, 0, 0, 0);
            }
        }
    }

    private void onHawkFlap()
    {
        birdModel.GetComponent<Animator>().SetTrigger("Flap");
    }

    private bool inPlummetMode = false;
    private float absRotation = 0.0f;
    private void onPlummetStatus(bool status)
    {
        birdModel.GetComponent<Animator>().SetBool("Plummet", status);

        if (status && !inPlummetMode)
        {
            inPlummetMode = true;

        } else if (!status && inPlummetMode)
        {
            inPlummetMode = false;
        }

        float rotationAmount = 50.0f * Time.deltaTime;

        if (absRotation > -30.0f && inPlummetMode)
        {
            birdModel.transform.Rotate(rotationAmount * -1.0f, 0, 0);
            absRotation -= rotationAmount;
        } else if (absRotation < 0 && !inPlummetMode)
        {
            birdModel.transform.Rotate(rotationAmount, 0, 0);
            absRotation += rotationAmount;
        }

    }

    void Start()
    {
        questManager = GameObject.FindGameObjectWithTag("QuestManager").GetComponent<QuestManager>();

        PlayerMovement pm = GetComponent<PlayerMovement>();
        pm.OnHawkFlap += onHawkFlap;
        pm.OnPlummetStatus += onPlummetStatus;
    }

	// Use this for initialization
	void Awake () {
        birdMorphEffect.enableEmission = false;
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

        CheckDeath();
	}

    void CheckDeath()
    {
        if (isDead) return;
        if (health <= 0.0f)
        {
            // Player Dead!!!
            PlayerDead();

            Invoke("ResetPlayer", 3.0f);           
        }
    }

    private bool isDead = false;
    void PlayerDead()
    {
        isDead = true;
        GetComponent<WeaponController>().getActiveWeapon().Animator.SetBool("Dead", true);
    }

    void ResetPlayer()
    {
        isDead = false;
        health = MAX_HEALTH;
        GetComponent<WeaponController>().getActiveWeapon().Animator.SetBool("Dead", false);
        questManager.ResetCurrentQuest();
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

    public void Save()
    {
        featherSave = feathers;
    }

    public void Load()
    {
        feathers = featherSave;
    }
}
