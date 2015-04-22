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
            if (isFlying)
            {
                Debug.Log("FLiegt!!");
                GameObject.Find("lifeenergyBird").transform.localScale = Vector3.one * (1.1f);
                GameObject.Find("lifeenergyBird").transform.position = transform.position + new Vector3(10f, 10f, 0f);
                GameObject.Find("lifeenergyHuman").transform.localScale = Vector3.one * (1f);
                GameObject.Find("lifeenergyHuman").transform.position = transform.position - new Vector3(10f, 10f, 0f);
            }
            else
            {
                Debug.Log("FLiegt nicht!!");
                GameObject.Find("lifeenergyBird").transform.localScale = Vector3.one * (1f);
                GameObject.Find("lifeenergyBird").transform.position = transform.position - new Vector3(10f, 10f, 0f);
                GameObject.Find("lifeenergyHuman").transform.localScale = Vector3.one * (1.1f);
                GameObject.Find("lifeenergyHuman").transform.position = transform.position + new Vector3(10f, 10f, 0f);
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
