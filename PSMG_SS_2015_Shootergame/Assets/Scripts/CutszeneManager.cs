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

    //container with all cutscenes and background image
    public GameObject container;

    //all images that belong to that cutscene
    public List<Image> images = new List<Image>();

    //sets the time per image
    public float[] timePerImage;

    //this shows how many images there are per site and how many sites
    public int[] ImagesPerSite;

    //boolean that is true if the cutscene animation is finished
    private bool isCutsceneAnimationFinished = false;

    //the acutal cutscene image that is animated
    private Image cutSceneImage;

    //the actual cutscene
    private GameObject actCutscene;

    //is the cutscene started
    private bool started = false;

    //is the cutscene finished
    private bool finished = false;

    //the number of the actual Image
    private int actImageNumber = 0;

    //list with all cutscenes
    public List<GameObject> allCutscenes = new List<GameObject>();

    // Use this for initialization
	void Start () {
        actCutscene = allCutscenes[CutsceneNumber-1];

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	// Update is called once per frame
    //manage Animation of all images, start fade in and out
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
                        //show new image
                        isCutsceneAnimationFinished = false;
                        cutSceneImage = images[actImageNumber];
                        FadeIn();
                    }
                    else
                    {
                        //end cutscene
                        started = false;
                        finished = true;
                        isCutsceneAnimationFinished = false;
                        FadeOut();
                    }
                }
            }
        }
	}

    //start the called Cutscene (with a variable number of pages)
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

    //check if the player is in the range of the cutscene trigger
    void CheckForStart()
    {
        float distance = Vector3.Distance(player.position, startTrigger.position);

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

    //Coroutine to fade in the actual image
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

    //Coroutine to fade out the text (wait for timeToShowText so the text is visible this amount of time)
    private IEnumerator FadeOutCR()
    {
        float alpha = 1;
        float currentTime = 0f;
        while (alpha > 0)
        {
            alpha = Mathf.Lerp(1f, 0f, currentTime);
            //set the alpha value of all Images of the cutscene 
            for (int i = 0; i < images.Count; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, alpha);
            }
            currentTime += Time.deltaTime;
            yield return null;
        }
        isCutsceneAnimationFinished = true;

        //set all Cutscenes inactive so they're no longer visible
        for (int i = 0; i < allCutscenes.Count; i++)
        {
            allCutscenes[i].SetActive(false);
        }
        container.SetActive(false);
        yield break;
    }
}
