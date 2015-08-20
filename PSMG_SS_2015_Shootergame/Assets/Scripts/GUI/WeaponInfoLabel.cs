using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Weapons;

public class WeaponInfoLabel : MonoBehaviour {

    //Label for amount of ammo
    private Text ammoLabel;
    
    private WeaponController wpc;

    //Array with Pictures of the different weapons
    public Image[] weaponImages;

	// Use this for initialization
	void Start () {
        ammoLabel = GetComponent<Text>();
        wpc = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
        wpc.onCurWeaponInfoChanged += new BaseWeapon.DOnWeaponInfoChanged(onWeaponInfoUpdated);
        onWeaponInfoUpdated(wpc.getActiveWeapon());
	}

    //Switch Weapon images with Weapon and show actual reserve ammo
	void onWeaponInfoUpdated(BaseWeapon w) {
        if (w == wpc.getWeaponByName("Bow"))
        {
            weaponImages[0].enabled = true;
            weaponImages[1].enabled = false;
            weaponImages[2].enabled = false;
        }
        else if (w == wpc.getWeaponByName("Blowgun"))
        {
            weaponImages[0].enabled = false;
            weaponImages[1].enabled = true;
            weaponImages[2].enabled = false;
        } 
        else if (w == wpc.getWeaponByName("Tomahawk"))
        {
            weaponImages[0].enabled = false;
            weaponImages[1].enabled = false;
            weaponImages[2].enabled = true;
        }
        Debug.Log(ammoLabel.text);
        ammoLabel.text = (w.ReserveAmmo == BaseWeapon.INFINITE_AMMO ? "∞" : w.ReserveAmmo.ToString());
	}
}
