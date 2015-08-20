using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Weapons;

public class FirepowerDisplay : MonoBehaviour {

    //shows the intensity of the bow
    public Image bowIntensity;

    //the crosshair
    public Image crosshair;
    
    //the bow script
    private Bow bow = null;

    //the weaponcontroller
    private WeaponController wpc = null;

	// Use this for initialization
    void Start()
    {
        //get the Bow 
        wpc = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
        //StartCoroutine(initBowObject(wpc));
        bow = (Bow)wpc.getWeaponByName("Bow");
	}

	//if the active Weapon is the bow show the crosshair and fill the fire power display
    //else show nothing
    void Update()
    {
        if (wpc.getActiveWeapon() == bow)
        {
            //Debug.Log((bow.getBowIntensity() - Bow.MIN_INTENSITY) / (Bow.MAX_INTENSITY - Bow.MIN_INTENSITY));
            bowIntensity.fillAmount = (bow.getBowIntensity() - Bow.MIN_INTENSITY) / (Bow.MAX_INTENSITY - Bow.MIN_INTENSITY);
            crosshair.enabled = true;
        }
        else
        {
            crosshair.enabled = false;
            bowIntensity.fillAmount = 0;
        }
	}
}
