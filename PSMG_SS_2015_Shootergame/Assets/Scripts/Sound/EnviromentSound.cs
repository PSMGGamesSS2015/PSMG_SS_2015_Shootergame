using UnityEngine;
using System.Collections;

public class EnviromentSound : MonoBehaviour {

    public AudioClip dayMusic;

    public AudioClip nightMusic;

    public AudioSource source;

    float factor = 0.2F;

    public bool day = true;

    public bool change = false;

	// Use this for initialization
	void Start () {
        //AudioSource[] sources = GetComponents<AudioSource>();
        //source = sources[2];
        source.volume = 0F;
        source.clip = dayMusic;
        source.Play();
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

}
