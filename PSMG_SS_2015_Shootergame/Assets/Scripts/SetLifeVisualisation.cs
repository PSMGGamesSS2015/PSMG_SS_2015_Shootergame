using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetLifeVisualisation : MonoBehaviour
{
    //The Player Object
    public PlayerMovement movement;

    //The base player
    public BasePlayer player;
    
    //the human with dark background
    public GameObject human1;

    //the human with light background
    public GameObject human2;

    //the bird with dark background
    public GameObject bird1;

    //the bird with light background
    public GameObject bird2;

    //Border of the health bar
    public Transform barBorderHealth;

    //Bar for the health
    public Transform barHealthRed;

    //Border of the flapcount bar
    public Transform barBorderFlaps;

    //Bar for the flapcount
    public Transform barFlapsBlue;

    //alpha cutoff Bar Flap
    public Renderer newFlapBar;

    //alpha cutoff Bar Health
    public Renderer newHealthBar;

    //Camera for Canvas
    public Camera canvasCamera;


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
    

    // Use this for initialization
    void Start()
    {
        isFlying = movement.getMode();
        prevMode = true;
        Debug.Log("barFlapsBlue: " + barFlapsBlue.position);
    }

    void Update()
    {
        //canvasCamera.orthographicSize = 100;
    }

    void OnGUI()
    {
        hasModeChanged();
        
        //only if the mode has changed, the images should change
        if (isModeChanged)
        {

            bird1.transform.transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            human1.transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            bird2.transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            human2.transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            barBorderHealth.position = new Vector3(124.0f, 95.5f, 0.0f);
            barBorderFlaps.position = new Vector3(124.0f, 95.5f, 0.0f);
            barHealthRed.position = new Vector3(886.1f, 102.1f, 1.0f);
            barFlapsBlue.position = new Vector3(886.1f, 102.1f, 1.0f);
            //Debug.Log(GameObject.Find("lifeenergyBird").position);
            if (isFlying)
            {
                //Debug.Log("Fliegt!!");
                //bird2 and human1 are not visible
                bird2.SetActive(false);
                human1.SetActive(false);
                //bird1 and human2 are visible
                bird1.SetActive(true);
                human2.SetActive(true);

                //scale the border for flaps to 1 and the border for the health to 0.5, also change position of the healthbar
                barBorderFlaps.localScale = Vector3.one * (1f);
                barBorderHealth.localScale = Vector3.one * (0.5f);
                barBorderHealth.position = GameObject.Find("bar border health").transform.position - new Vector3(60f, 50f, 0f);

                //scale the flaps bar to 1 and the health bar to 0.5, also change position of the healthbar
                barFlapsBlue.localScale = Vector3.one * (10f);
                barHealthRed.localScale = Vector3.one * (5f);
                barHealthRed.position = GameObject.Find("red health bar").transform.position - new Vector3(60f, 50f, 0f);
                //barHealthRed.position = Vector3.Lerp(new Vector3(886.1f, 102.1f, 1.0f), new Vector3(946.1f, 162.1f, 1.0f), Time.deltaTime * 2.5f);
                
                //change position of the bird1 image and scale it to 1
                bird1.transform.localScale = Vector3.one * (1f);
                bird1.transform.position = GameObject.Find("lifeenergyBird state1").transform.position + new Vector3(25f, 25f, 0f);

                //change position of the human2 image and scale it to 0.5
                human2.transform.localScale = Vector3.one * (0.5f);
                human2.transform.position = GameObject.Find("lifeenergyHuman state2").transform.position - new Vector3(25f, 25f, 0f);
            }
            else
            {
                newFlapBar.material.SetFloat("_Cutoff", 0.01f);

                //Debug.Log("Fliegt nicht!!");
                //bird1 and human2 are not visible
                bird1.SetActive(false);
                human2.SetActive(false);
                //bird2 and human1 are visible
                bird2.SetActive(true);
                human1.SetActive(true);

                //scale the border for health to 1 and the border for the flaps to 0.5, also change position of the flapsbar
                barBorderFlaps.localScale = Vector3.one * (0.5f);
                barBorderFlaps.position = GameObject.Find("bar border flaps").transform.position - new Vector3(60f, 50f, 0f);
                barBorderHealth.localScale = Vector3.one * (1f);

                //scale the flaps bar to 0.5 and the health bar to 1, also change position of the flapsbar
                barFlapsBlue.localScale = Vector3.one * (5f);
                barFlapsBlue.position = GameObject.Find("blue flaps bar").transform.position - new Vector3(60f, 50f, 0f);
                barHealthRed.localScale = Vector3.one * (10f);

                //change position of the bird2 image and scale it to 0.5
                bird2.transform.localScale = Vector3.one * (0.5f);
                bird2.transform.position = GameObject.Find("lifeenergyBird state2").transform.position - new Vector3(25f, 25f, 0f);

                //change position of the human1 image and scale it to 1
                human1.transform.localScale = Vector3.one * (1f);
                human1.transform.position = GameObject.Find("lifeenergyHuman state1").transform.position + new Vector3(25f, 25f, 0f);
            }
        }

        //show remaining flaps if the player is flying, else show the health of the player
        if (isFlying)
        {
            flappercent = (float)movement.getRemainingFlaps() / (float)movement.flapAmount;

            //Workaround - need to be fixed
            if (flappercent == 1)
            {
                flappercent = 0.99f;
            }

            Debug.Log("flappercent " + flappercent);
            newFlapBar.material.SetFloat("_Cutoff", 1-flappercent);
        }
        else
        {
            healthpercent = (float)player.health / (float)player.getMaxHealth();
            
            //Workaround - need to be fixed
            if (healthpercent == 1)
            {
                healthpercent = 0.99f;
            }

            Debug.Log("healthpercent " + healthpercent);
            newHealthBar.material.SetFloat("_Cutoff", 1 - healthpercent);
        }

    }

    //check if the Mode has changed or not
    bool hasModeChanged()
    {
        isFlying = movement.getMode();
        //Debug.Log("Fly? " + isFlying + ", prev? "+prevMode);
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
        //Debug.Log("isModeChanged? " + isModeChanged);
        return isModeChanged;
    }

}
