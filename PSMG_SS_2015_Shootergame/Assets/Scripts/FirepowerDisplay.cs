using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Weapons;

public class FirepowerDisplay : MonoBehaviour {

    //shows the intensity of the bow
    public Image bowIntensity;

    //the bow script
    private Bow bow = null;

    //private bool initialized = false;

	// Use this for initialization
    void Start()
    {
        //get the Bow 
        WeaponController wpc = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
        //StartCoroutine(initBowObject(wpc));
        bow = (Bow)wpc.getWeaponByName("Bow");
	}

    //IEnumerator initBowObject(WeaponController wpc)
    //{
    //    while (bow == null)
    //    {
    //        bow = (Bow)wpc.getWeaponByName("Bow");
    //    }
    //    yield return null;
    //}
	
	// Update is called once per frame
    void Update()
    {
        bowIntensity.fillAmount = (bow.getBowIntensity() - Bow.MIN_INTENSITY) / (Bow.MAX_INTENSITY - Bow.MIN_INTENSITY);
	}
}
