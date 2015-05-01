using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetLifeMode : MonoBehaviour
{
    //The Player Object
    public PlayerMovement movement;
    
    //the human with dark background
    public Transform human1;

    //the human with light background
    public Transform human2;

    //the bird with dark background
    public Transform bird1;

    //the bird with light background
    public Transform bird2;


    //has the mode changed from flying to not flying (or the other way round) or not
    private bool isModeChanged;

    //is the player flying or not
    private bool isFlying;

    //is the previous mode flying or not flying
    private bool prevMode;


    // Use this for initialization
    void Start()
    {
        isFlying = movement.getMode();
        prevMode = true;
    }


    void OnGUI()
    {
        hasModeChanged();

        //only if the mode has changed, the images should change
        if (isModeChanged)
        {
            bird1.position = new Vector3(74.0f, 68.5f, 0.0f);
            human1.transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            bird2.position = new Vector3(74.0f, 68.5f, 0.0f);
            human2.transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            //Debug.Log(GameObject.Find("lifeenergyBird").transform.position);
            if (isFlying)
            {
                //Debug.Log("Fliegt!!");
                bird2.transform.localScale = Vector3.one * (0f);
                human1.transform.localScale = Vector3.one * (0f);
                bird1.transform.localScale = Vector3.one * (1f);
                bird1.transform.position = GameObject.Find("lifeenergyBird state1").transform.position + new Vector3(25f, 25f, 0f);
                human2.transform.localScale = Vector3.one * (0.5f);
                human2.transform.position = GameObject.Find("lifeenergyHuman state1").transform.position - new Vector3(25f, 25f, 0f);
            }
            else
            {
                //Debug.Log("Fliegt nicht!!");
                human2.transform.localScale = Vector3.one * (0f);
                bird1.transform.localScale = Vector3.one * (0f);
                bird2.transform.localScale = Vector3.one * (0.5f);
                bird2.transform.position = GameObject.Find("lifeenergyBird state1").transform.position - new Vector3(25f, 25f, 0f);
                human1.transform.localScale = Vector3.one * (1f);
                human1.transform.position = GameObject.Find("lifeenergyHuman state1").transform.position + new Vector3(25f, 25f, 0f);
            }
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
