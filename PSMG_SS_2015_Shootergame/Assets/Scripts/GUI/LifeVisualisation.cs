using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LifeVisualisation : MonoBehaviour {

    //The Player Object
    public GameObject playerObject;

    //The Player Object
    private PlayerMovement movement;

    //The base player
    private BasePlayer player;

    //the human with dark background
    public GameObject human1;

    //the human with light background
    public GameObject human2;

    //the bird with dark background
    public GameObject bird1;

    //the bird with light background
    public GameObject bird2;

    //shows the remaining flaps of the bird
    public Image flapBar;

    //shows the remaining health of the human
    public Image healthBar;

    //shows the remaining energy to sprint of the human
    public Image energyBar;

    //has the mode changed from flying to not flying (or the other way round) or not
    private bool isModeChanged;

    //is the player flying or not
    private bool isFlying;

    //is the previous mode flying or not flying
    private bool prevMode;

    //percentage of player health
    private float healthpercent;

    //percentage of birds remaining flaps
    private float flappercent;

    //percentage of energy to sprint
    private float energy;

    //Whether we are currently interpolating or not
    private bool _isLerping;

    //move the bird panel to this vector 
    private Vector3 birdVector;

    //move the human panel to this vector 
    private Vector3 humanVector;

    //small vector for the smaller icon
    private Vector3 smallVector;

    //bigger vector for the bigger icon
    private Vector3 bigVector;
    
    //the panel with human healthbar and the icon
    public Transform panelHuman;

    //the panel with bird flapbar and the icon
    public Transform panelBird;

    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;

    //The time taken to move from the start to finish positions
    public float timeTakenDuringLerp = 1f;

	// Use this for initialization
    void Start()
    {
        movement = playerObject.GetComponent<PlayerMovement>();
        player = playerObject.GetComponent<BasePlayer>();

        smallVector = new Vector3(0.6f, 0.6f, 0.6f);
        bigVector = new Vector3(1.0f, 1.0f, 1.0f);

        isFlying = movement.getMode();
        //the player starts as a human
        prevMode = true;
	
	}

    void FixedUpdate()
    {
        if (_isLerping)
        {
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;
            panelHuman.transform.localScale = Vector3.Lerp(panelHuman.transform.localScale, humanVector, percentageComplete);
            panelBird.transform.localScale = Vector3.Lerp(panelBird.transform.localScale, birdVector, percentageComplete);
        }
    }

    
    //change between flight and move modes
    void OnGUI()
    {
        hasModeChanged();
        
        //only if the mode has changed, the images should change
        if (isModeChanged)
        {
            if (isFlying)
            {
                //Debug.Log("Fliegt!!");
                //bird2 and human1 are not visible
                bird2.SetActive(false);
                human1.SetActive(false);
                //bird1 and human2 are visible
                bird1.SetActive(true);
                human2.SetActive(true);
                
                //set desired size vector of human and bird
                birdVector = bigVector;
                humanVector = smallVector;

                StartLerping();

            }
            else
            {
                //reset the flap bar
                flapBar.fillAmount = 1;

                //Debug.Log("Fliegt nicht!!");
                //bird1 and human2 are not visible
                bird1.SetActive(false);
                human2.SetActive(false);
                //bird2 and human1 are visible
                bird2.SetActive(true);
                human1.SetActive(true);

                //set desired size vector of human and bird
                birdVector = smallVector;
                humanVector = bigVector;

                StartLerping();
            }
        }

        //show remaining flaps if the player is flying, else show the health of the player
        if (isFlying)
        {
            flappercent = (float)movement.getRemainingFlaps() / (float)movement.flapAmount;

            flapBar.fillAmount = Mathf.Max(flappercent, 0.001f);
            energyBar.fillAmount = Mathf.Max(0, 0.001f);
        }
        else
        {
            healthpercent = (float)player.health / (float)BasePlayer.MAX_HEALTH;
            energy = (float)player.getEnergy() / (float)BasePlayer.MAX_ENERGY;

            energyBar.fillAmount = Mathf.Max(energy, 0.001f);
            healthBar.fillAmount = Mathf.Max(healthpercent, 0.001f);
        }

    }

    // Called to begin the linear interpolation
    void StartLerping()
    {
        _isLerping = true;
        _timeStartedLerping = Time.time;
    }



    //check if the Mode has changed or not
    bool hasModeChanged()
    {
        isFlying = movement.getMode();

        if (isFlying && prevMode)
        {
            isModeChanged = false;
            prevMode = isFlying;
        }
        else if (isFlying && !prevMode)
        {
            isModeChanged = true;
            prevMode = isFlying;
        }
        else if (!isFlying && prevMode)
        {
            isModeChanged = true;
            prevMode = isFlying;
        }
        else if (!isFlying && !prevMode)
        {
            isModeChanged = false;
            prevMode = isFlying;
        }

        return isModeChanged;
    }
}
