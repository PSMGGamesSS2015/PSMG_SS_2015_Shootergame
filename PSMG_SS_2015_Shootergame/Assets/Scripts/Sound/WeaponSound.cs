using UnityEngine;
using System.Collections;

public class WeaponSound : MonoBehaviour {

    public AudioClip bowShoot;

    public AudioClip bowSpan;

    public AudioClip swoosh;

    public AudioClip hitSound;

    public AudioSource weaponSource;

	// Use this for initialization
	void Start() {
	}
	
	// Update is called once per frame
	void Update() {
	    
	}

    public void shootBowSound()
    {
        weaponSource.PlayOneShot(bowShoot, 0.12F);
        weaponSource.PlayOneShot(swoosh, 0.1F);
    }

    public void spanBowSound()
    {
        Invoke("spanBow", 0.7F);
    }

    private void spanBow()
    {
        weaponSource.PlayOneShot(bowSpan, 0.8F);
    }

    public void swingHawkSound()
    {
        Invoke("swingTomahawk", 0.6F);
    }

    private void swingTomahawk()
    {
        weaponSource.PlayOneShot(swoosh, 0.1F);
    }

    public void playHitSound()
    {
        weaponSource.PlayOneShot(hitSound, 0.5F);
    }

}
