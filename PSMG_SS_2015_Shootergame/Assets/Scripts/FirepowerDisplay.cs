using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Weapons;

public class FirepowerDisplay : MonoBehaviour {

    //shows the intensity of the bow
    public Image bowIntensity;

    //the bow script
    private Bow bow;

	// Use this for initialization
    void Start()
    {
        //get the Bow 
        WeaponController wpc = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
        bow = (Bow)wpc.getWeaponByName("Bow");
	
	}
	
	// Update is called once per frame
    void Update()
    {
        bowIntensity.fillAmount = (bow.getBowIntensity() - Bow.MIN_INTENSITY) / (Bow.MAX_INTENSITY - Bow.MIN_INTENSITY);
	
	}
}
