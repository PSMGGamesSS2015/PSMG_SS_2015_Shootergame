﻿using UnityEngine;
using System.Collections;

public class WeaponSound : MonoBehaviour {

    public AudioClip bowShoot;

    public AudioClip bowSpan;

    private AudioSource weaponSource;

	// Use this for initialization
	void Start () {
        weaponSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void shootBowSound ()
    {
        weaponSource.PlayOneShot(bowShoot, 1F);
    }

    public void spanBowSound ()
    {
        weaponSource.PlayOneShot(bowSpan, 1F);
    }

}