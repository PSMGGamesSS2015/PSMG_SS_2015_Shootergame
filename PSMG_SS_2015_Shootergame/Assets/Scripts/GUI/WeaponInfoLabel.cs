using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Weapons;

public class WeaponInfoLabel : MonoBehaviour {

    private Text ammoLabel;
	// Use this for initialization
	void Start () {
        ammoLabel = GetComponent<Text>();
        WeaponController wpc = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>();
        wpc.onCurWeaponInfoChanged += new BaseWeapon.DOnWeaponInfoChanged(onWeaponInfoUpdated);
        onWeaponInfoUpdated(wpc.getActiveWeapon());
	}

    void Update()
    {

    }

	void onWeaponInfoUpdated(BaseWeapon w) {
        ammoLabel.text = w.Name
                + " "
                + w.CurAmmo
                + " | " +
                (w.ReserveAmmo == BaseWeapon.INFINITE_AMMO ? "∞" : w.ReserveAmmo.ToString());
	}
}
