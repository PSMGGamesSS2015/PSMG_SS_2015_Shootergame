using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {

    public AudioClip thud;

    private AudioSource aSource;

    private float soundDelay = 0;

    private bool sprinting = false;

    private bool crouching = false;

    private bool sneaking = false;

    private bool moving = false;

    private int test = 0;

	// Use this for initialization
	void Start () {
        updateMovement();
        aSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

        //updateMovement();

        if (moving && !sprinting && !crouching)
        {
            playMoveSound(0.05F, 0.1F);
        }
        else if (sprinting)
        {
            playMoveSound(-0.1F, 1F);
        }
        else if (moving && crouching)
        {
            playMoveSound(0.1F, 0.03F);
        }



	}

    private void updateMovement()
    {

        //moving = GetComponent<PlayerMovement>().moving;
        //crouching = GetComponent<PlayerMovement>().crouching;
        //sprinting = GetComponent<PlayerMovement>().sprinting;
        //sneaking = GetComponent<PlayerMovement>().sneaking;

    }

    private void playMoveSound(float delay, float vol)
    {

        if (Time.time >= soundDelay)
        {
            aSource.PlayOneShot(thud, vol);
            soundDelay = Time.time + thud.length-0.7F + delay;
        }

        moving = false;
        crouching = false;
        sprinting = false;
        sneaking = false;

    }
}
