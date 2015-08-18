using UnityEngine;
using System.Collections;

public class PlayerSound : MonoBehaviour {
    
    public AudioClip grass;

    public AudioClip highGrass;

    public AudioClip rock1;

    public AudioClip rock2;

    public AudioClip water;

    public AudioClip forest;


    public AudioClip eagle;

    public AudioClip groundSound;

    private AudioSource aSource;

    private int surfaceIndex = 0;

    private float soundDelay = 0;

    private float jumpDelay = 0;

    public bool sprinting = false;

    public bool crouching = false;

    public bool sneaking = false;

    public bool moving = false;

	// Use this for initialization
	void Start () {
        aSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
        
        if (moving)
        {
            playMoveSound(0.1F, 0.1F);
        }
        else if (sprinting)
        {
            playMoveSound(-0.1F, 0.3F);
        }
        else if (crouching)
        {
            playMoveSound(0.3F, 0.03F);
        }
        else if (sneaking)
        {
            playMoveSound(0.3F, 0.03F);
        }

        surfaceIndex = TerrainSurface.GetMainTexture(transform.position);
        Debug.Log("SurfaceIndex: " + surfaceIndex);
	}

    private void playMoveSound(float delay, float vol)
    {

        AudioClip thud = grass;

        float offset = 0;

        switch (surfaceIndex)
        {
            case 0:
                offset = -0.2F;
                thud = grass;
                break;
            case 1:
                offset = -0.1F;
                thud = grass;
                break;
            case 2:
                offset = -0.2F;
                thud = rock2;
                break;
            case 3:
                offset = -0.25F;
                int rnd3 = Random.Range(1, 5);
                if (rnd3 == 1)
                {
                    thud = grass;
                }
                else
                {
                    thud = rock1;
                }
                break;
            case 4:
                thud = highGrass;
                break;
            case 5:
                int rnd5 = Random.Range(1, 3);
                if (rnd5 == 1)
                {
                    thud = rock1;
                }
                else
                {
                    thud = rock2;
                }
                break;
            case 7:
                thud = forest;
                break;
            case 8:
                thud = forest;
                break;
            case 9:
                thud = grass;
                offset = -0.2F;
                break;
            case 10:
                thud = grass;
                offset = -0.2F;
                break;
            default:
                thud = rock1;
                break;
        }

        if (Time.time >= soundDelay)
        {
            aSource.PlayOneShot(thud, vol);
            soundDelay = Time.time + thud.length + delay + offset;
        }

        moving = false;
        crouching = false;
        sprinting = false;
        sneaking = false;

    }

    public void playMorph ()
    {
        Invoke("playEagleSound", 3.0F);
    }

    public void playGround ()
    {
        if (Time.time >= jumpDelay)
        {
            aSource.PlayOneShot(groundSound, 0.05F);
            jumpDelay = Time.time + groundSound.length;
            playMoveSound(0, 0.2F);
        }
    }

    private void playEagleSound ()
    {
        aSource.PlayOneShot(eagle, 1F);
    }

}
