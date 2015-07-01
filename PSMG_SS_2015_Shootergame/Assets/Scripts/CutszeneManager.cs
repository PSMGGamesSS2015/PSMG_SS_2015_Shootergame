using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CutszeneManager : MonoBehaviour {


    private Transform player;

    //public int NumberOfTotalCutscenes;

    public Transform startTrigger;

    public float startDistance = 50.0f;

    //the number of the actual cutscene
    public int CutsceneNumber;

    public GameObject container;

    //all images that belong to that cutscene
    public List<Image> images = new List<Image>();

    //sets the time per image
    public int[] timePerImage;

    //this shows how many images there are per site and how many sites
    public int[] ImagesPerSite;

    //boolean that is true if the cutscene animation is finished
    private bool isCutsceneAnimationFinished = false;

    //the acutal cutscene image that is animated
    private Image cutSceneImage;

    private GameObject actCutscene;

    private bool started = false;

    private bool finished = false;

    private int actImageNumber = 0;

    public List<GameObject> allCutscenes = new List<GameObject>();

    // Use this for initialization
    //At the start of the game all cutscene are loaded.
	void Start () {
        for (int i = 1; i <= allCutscenes.Count; i++)
        {
            Debug.Log(i);
            allCutscenes[i - 1].SetActive(false);
        }
        actCutscene = allCutscenes[CutsceneNumber-1];

        container.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	// Update is called once per frame
    //manage Animation of all images
	void Update () {
        if (!finished)
        {
            if (!started)
            {
                CheckForStart();
            }
            else
            {
                if (isCutsceneAnimationFinished)
                {
                    if (images.Count > actImageNumber)
                    {
                        isCutsceneAnimationFinished = false;
                        cutSceneImage = images[actImageNumber];
                        FadeIn();
                    }
                    else
                    {
                        started = false;
                        finished = true;
                        isCutsceneAnimationFinished = false;
                        FadeOut();
                        //fade out
                    }
                }
            }
        }
	}

    //start the called Cutscene with a variable number of pages
    public void StartCutscene()
    {
        for (int i = 1; i <= images.Count; i++)
        {
            images[i - 1].color = new Color(images[i - 1].color.r, images[i - 1].color.g, images[i - 1].color.b, 0);
        }
        container.SetActive(true);
        actCutscene.SetActive(true);

        isCutsceneAnimationFinished = true;
    }


    void CheckForStart()
    {
        float distance = Vector3.Distance(player.position, startTrigger.position);
        Debug.Log("search");

        if (distance <= startDistance)
        {
            started = true;
            Debug.Log("start");
            StartCutscene();
        }
    }

    //Fade the actual image in
    public void FadeIn()
    {
        StartCoroutine("FadeInCR");
    }

    //Coroutine to fade the actual image in
    private IEnumerator FadeInCR()
    {
        float alpha = 1;
        float currentTime = 0f;
        while (alpha > 0)
        {
            alpha = Mathf.Lerp(1f, 0f, currentTime);
            cutSceneImage.color = new Color(cutSceneImage.color.r, cutSceneImage.color.g, cutSceneImage.color.b, 1 - alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log(actImageNumber);
        yield return new WaitForSeconds(timePerImage[actImageNumber]);
        actImageNumber++;
        isCutsceneAnimationFinished = true;
        yield break;
    }


    //fade the text out
    public void FadeOut()
    {
        StartCoroutine("FadeOutCR");
    }

    //Coroutine to fade the text out (wait for timeToShowText so the text is visible this amount of time)
    private IEnumerator FadeOutCR()
    {
        float alpha = 1;
        float currentTime = 0f;
        while (alpha > 0)
        {
            alpha = Mathf.Lerp(1f, 0f, currentTime);
            //Debug.Log(alpha);
            for (int i = 0; i < images.Count; i++)
            {
                Debug.Log(images[i]);
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, alpha);
            }
            currentTime += Time.deltaTime;
            yield return null;
        }
        isCutsceneAnimationFinished = true;

        for (int i = 0; i < allCutscenes.Count; i++)
        {
            allCutscenes[i].SetActive(true);
        }
        container.SetActive(true);
        yield break;
    }
}
