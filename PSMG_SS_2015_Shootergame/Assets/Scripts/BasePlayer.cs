using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class BasePlayer : MonoBehaviour {
    /*PLAYER DATA*/
    public int health = 100;

    /* WEAPON STUFF */
    public BaseWeapon[] weapons;
    private const int MAX_WEAPONS = 4;
    private int numWeapons = 1;
    private int curWeaponIndex = 0;


	// Use this for initialization
	void Awake () {
        
        weapons = new BaseWeapon[MAX_WEAPONS];
        weapons[0] = new Bow(gameObject);
        Debug.Log("tralalla");
	}
	
	// Update is called once per frame
	void Update () {
        weapons[curWeaponIndex].Update();
	}
}
