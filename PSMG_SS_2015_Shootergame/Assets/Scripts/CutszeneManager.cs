using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CutszeneManager : MonoBehaviour {

    //boolean that is true if the cutscene animation is finished
    private bool isCutsceneAnimationFinished = false;

    //true if the Cutscene has more than one page (NOT IMPLEMENTED)
    private bool morePages = false;
    
    //the container for all cutscene pictures
    public GameObject container;

    //the number of all existing Cutscenes
    public int NumberOfCutscenes;
    
    //a List that contains all existing cutscenes
    private List<GameObject> cutscenes = new List<GameObject>();
    //a List that contains all Images which belong to the actual Cutscene
    private List<Image> cutsceneImages = new List<Image>();

    //the acutal cutscene image that is animated
    private Image cutSceneImage;
    //the acutal cutscene that should be shown
    private GameObject actCutscene;
    
    private bool first = true;


    // Use this for initialization
    //At the start of the game all cutscene are loaded.
	void Start () {
        for (int i = 1; i <= NumberOfCutscenes; i++)
        {
            actCutscene = GameObject.Find("cutscene " + i);
            Debug.Log(actCutscene);
            cutscenes.Add(actCutscene);
            actCutscene.SetActive(false);
        }
        container.SetActive(false);

        startCutscene(1);
    }
	
	// Update is called once per frame
    //manage Animation of all images
	void Update () {
        if (isCutsceneAnimationFinished)
        {
            if (cutsceneImages.Count > 0)
            {
                isCutsceneAnimationFinished = false;
                cutSceneImage = cutsceneImages[0];
                cutsceneImages.RemoveAt(0);
                FadeIn();
            }
            else
            {
                container.SetActive(false);
                actCutscene.SetActive(false);
                if(first) 
                {
                    first = false;
                    startCutscene(2);
                }
                //fade out
            }
        }
	}

    //start the called Cutscene with one page
    private void startCutscene(int numberOfCutscene)
    {
        startCutscene(numberOfCutscene, 1);
    }

    //start the called Cutscene with a variable number of pages
    public void startCutscene(int numberOfCutscene, int pagesCount)
    {
        if(pagesCount > 1) {
            morePages = true;
        }

        //get the actual cutscene and set it active
        actCutscene = cutscenes[numberOfCutscene - 1];
        container.SetActive(true);
        actCutscene.SetActive(true);

        //get all images that belong to the actual cutscene and make them unvisible
        for (int i = 1; i <= actCutscene.transform.childCount; i++)
        {
            cutsceneImages.Add(actCutscene.transform.Find("Image " + i).GetComponent<Image>());
            cutsceneImages[i - 1].color = new Color(cutsceneImages[i - 1].color.r, cutsceneImages[i - 1].color.g, cutsceneImages[i - 1].color.b, 0);
        }
        isCutsceneAnimationFinished = true;
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
        yield return new WaitForSeconds(3f);
        isCutsceneAnimationFinished = true;
        yield break;
    }
}
