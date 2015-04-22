using UnityEngine;
using System.Collections;

public class setLifeMode : MonoBehaviour {
    public PlayerMovement movement;
    private bool isModeChanged;
    private bool isFlying;
    private bool prevMode;

    // Use this for initialization
	void Start () {
        isFlying = movement.getMode();
        prevMode = true;
	}
	
	// Update is called once per frame
    void OnGUI()
    {
        hasModeChanged();

        if (isModeChanged)
        {
            GameObject.Find("lifeenergyBird").transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            GameObject.Find("lifeenergyHuman").transform.position = new Vector3(74.0f, 68.5f, 0.0f);
            //Debug.Log(GameObject.Find("lifeenergyBird").transform.position);
            if (isFlying)
            {
                Debug.Log("Fliegt!!");
                GameObject.Find("lifeenergyBird").transform.localScale = Vector3.one * (1f);
                GameObject.Find("lifeenergyBird").transform.position = GameObject.Find("lifeenergyBird").transform.position + new Vector3(25f, 25f, 0f);
                GameObject.Find("lifeenergyHuman").transform.localScale = Vector3.one * (0.5f);
                GameObject.Find("lifeenergyHuman").transform.position = GameObject.Find("lifeenergyHuman").transform.position - new Vector3(25f, 25f, 0f);
            }
            else
            {
                Debug.Log("Fliegt nicht!!");
                GameObject.Find("lifeenergyBird").transform.localScale = Vector3.one * (0.5f);
                GameObject.Find("lifeenergyBird").transform.position = GameObject.Find("lifeenergyBird").transform.position - new Vector3(25f, 25f, 0f);
                GameObject.Find("lifeenergyHuman").transform.localScale = Vector3.one * (1f);
                GameObject.Find("lifeenergyHuman").transform.position = GameObject.Find("lifeenergyHuman").transform.position + new Vector3(25f, 25f, 0f);
            }
        }
   	}

    bool hasModeChanged()
    {
        isFlying = movement.getMode();
        Debug.Log("Fly? " + isFlying + ", prev? "+prevMode);
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
        Debug.Log("isModeChanged? " + isModeChanged);
        return isModeChanged;
    }

}
