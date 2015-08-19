using UnityEngine;
using System.Collections;

public class WeaponSound : MonoBehaviour {

    public AudioClip bowShoot;

    public AudioClip bowSpan;

    public AudioClip swoosh;

    public AudioClip hitSound;

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
        weaponSource.PlayOneShot(bowShoot, 0.12F);
        weaponSource.PlayOneShot(swoosh, 0.1F);
    }

    public void spanBowSound ()
    {
        weaponSource.PlayOneShot(bowSpan, 0.8F);
    }

    public void swingHawkSound ()
    {
        weaponSource.PlayOneShot(swoosh, 0.1F);
    }
    public void playHitSound ()
    {
        weaponSource.PlayOneShot(hitSound, 0.5F);
    }
}
