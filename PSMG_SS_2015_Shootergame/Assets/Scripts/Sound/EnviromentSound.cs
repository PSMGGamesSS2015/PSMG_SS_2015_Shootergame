using UnityEngine;
using System.Collections;

public class EnviromentSound : MonoBehaviour {

    public AudioClip dayMusic;

    public AudioClip nightMusic;

    public AudioClip screamClip;

    public AudioClip reachedSound;

    public AudioClip collectedSound;

    public AudioClip treeHit;

    public AudioClip timber;

    public AudioClip totem;

    public AudioSource source;

    public AudioSource screamSource;

    public AudioSource totemAudio;

    float factor = 0.05F;

    public bool day = true;

    public bool change = false;

	// Use this for initialization
	void Start () {
        source.volume = 0F;
        source.clip = dayMusic;
        source.Play();
        totemAudio.volume = 0.5F;
        totemAudio.clip = totem;
	}
	
	// Update is called once per frame
	void Update () {
        if (change)
        {
            fadeOut();
            
        } 
        else if (source.volume < 1F)
        {
            fadeIn();
        }
	}

    void fadeIn ()
    {
        if (source.volume < 1F)
        {
            source.volume += factor * Time.deltaTime;
        }
    }

    void fadeOut ()
    {
        if (source.volume > 0.1F)
        {
            source.volume -= factor * Time.deltaTime;
        }
        else
        {
            source.Stop();
            if (day)
            {
                source.clip = dayMusic;
            }
            else
            {
                source.clip = nightMusic;
            }
            change = false;
            source.Play();
        }
    }

    public void playScream()
    {
        screamSource.PlayOneShot(screamClip, 0.8F);
    }

    public void playReached()
    {
        source.PlayOneShot(reachedSound, 0.3F);
    }

    public void playCollected()
    {
        source.PlayOneShot(collectedSound, 0.3F);
    }

    public void playTreeHit()
    {
        source.PlayOneShot(treeHit, 0.8F);
    }

    public void playTimber()
    {
        source.PlayOneShot(timber, 0.8F);
    }

    public void playTotem()
    {
        if (!totemAudio.isPlaying)
        {
            totemAudio.Play();
        }
    }

    public void pauseTotem()
    {
        totemAudio.Pause();
    }

    public void stopTotem()
    {
        totemAudio.Stop();
    }
}
