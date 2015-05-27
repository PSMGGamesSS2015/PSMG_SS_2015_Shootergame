using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Weapons;

public class SetUIVisualisation : MonoBehaviour
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

    //shows the remaining flaps of the bird
    public Image flapBar;
        
    //shows the remaining health of the human
    public Image healthBar;

    //shows the intensity of the bow
    public Image bowIntensity;

    //the bow script
    private Bow bow;

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

        WeaponController wpc = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();

        //get the bow
        bow = (Bow)wpc.getWeaponByName("Bow");
        
        //Debug.Log("barFlapsBlue: " + barFlapsBlue.position);
    }

    void Update()
    {
        float intensity = Mathf.Lerp(bow.getMinIntensity(), bow.getMaxIntensity(), bow.getBowIntensity());
        bowIntensity.fillAmount = Mathf.Max(intensity, 0.001f);
    }

    void OnGUI()
    {
        hasModeChanged();
        
        //only if the mode has changed, the images should change
        if (isModeChanged)
        {
            //Debug.Log("red " + barHealthRed.position);
            //Debug.Log("blue" + barFlapsBlue.position);

            bird1.transform.transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            human1.transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            bird2.transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            human2.transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            healthBar.transform.position = new Vector3(142.0f, 99.0f, 0.0f);
            flapBar.transform.position = new Vector3(142.0f, 99.0f, 0.0f);

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

                //change position of the bird1 image and scale it to 1
                bird1.transform.localScale = Vector3.one * (1f);
                bird1.transform.position = GameObject.Find("lifeenergyBird state1").transform.position + new Vector3(25f, 25f, 0f);

                //change position of the human2 image and scale it to 0.5
                human2.transform.localScale = Vector3.one * (0.5f);
                human2.transform.position = GameObject.Find("lifeenergyHuman state2").transform.position - new Vector3(25f, 25f, 0f);

                //change position and size of the healthbar
                healthBar.transform.position = GameObject.Find("healthbar").transform.position - new Vector3(75f, 55f, 0f);
                healthBar.transform.localScale = Vector3.one * (0.5f);

                //change position and size of the flapbar
                flapBar.transform.localScale = Vector3.one * (1.0f);

            }
            else
            {
                flapBar.fillAmount = 1;

                //Debug.Log("Fliegt nicht!!");
                //bird1 and human2 are not visible
                bird1.SetActive(false);
                human2.SetActive(false);
                //bird2 and human1 are visible
                bird2.SetActive(true);
                human1.SetActive(true);

                //change position of the bird2 image and scale it to 0.5
                bird2.transform.localScale = Vector3.one * (0.5f);
                bird2.transform.position = GameObject.Find("lifeenergyBird state2").transform.position - new Vector3(25f, 25f, 0f);

                //change position of the human1 image and scale it to 1
                human1.transform.localScale = Vector3.one * (1f);
                human1.transform.position = GameObject.Find("lifeenergyHuman state1").transform.position + new Vector3(25f, 25f, 0f);

                //change position and size of the healthbar
                healthBar.transform.localScale = Vector3.one * (1.0f);

                //change position and size of the flapbar
                flapBar.transform.position = GameObject.Find("flapbar").transform.position - new Vector3(75f, 55f, 0f);
                flapBar.transform.localScale = Vector3.one * (0.5f);
            }
        }

        //show remaining flaps if the player is flying, else show the health of the player
        if (isFlying)
        {
            flappercent = (float)movement.getRemainingFlaps() / (float)movement.flapAmount;

            flapBar.fillAmount = Mathf.Max(flappercent, 0.001f);
        }
        else
        {
            healthpercent = (float)player.health / (float)player.getMaxHealth();
            
            healthBar.fillAmount = Mathf.Max(healthpercent, 0.001f);
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
