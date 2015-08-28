using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class IntroManager : MonoBehaviour {
    //the player
    private Transform player;

    //Images for Intro. This only works if all images have an alpha = 0 value.
    public List<Image> imagesOfIntro = new List<Image>();

    //Text for Intro. This only works if all texts have an alpha = 0 value.
    public List<Text> textOfIntro = new List<Text>();

    //The Container in UI
    public GameObject intro;

    //actual Image
    private Image actImage;
    //actual Text
    private Text actText;
    //the actual page
    private int actPage = 0;
    //if it is a new page this is true
    private bool newPage = true;

    private bool active = true;

	// Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.GetComponent<PlayerMovement>().canMove = false;
        intro.SetActive(true);

	}
	
	// Update is called once per frame
	void Update () {
        if (newPage)
        {
            newPage = false;
            actImage = imagesOfIntro[actPage];
            actText = textOfIntro[actPage];
            FadeIn();
        }
    }

    //Fade in the actual image
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
            actImage.color = new Color(actImage.color.r, actImage.color.g, actImage.color.b, 1 - alpha);
            actText.color = new Color(actText.color.r, actText.color.g, actText.color.b, 1 - alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }


    //fade out the text
    public void FadeOut()
    {
        StartCoroutine("FadeOutCR");
    }

    //Coroutine to fade out the text and the image and end Intro or show a new page
    private IEnumerator FadeOutCR()
    {
        float alpha = 1;
        float currentTime = 0f;
        while (alpha > 0)
        {
            alpha = Mathf.Lerp(1f, 0f, currentTime);
            //set the alpha value of Text and Image
            actImage.color = new Color(actImage.color.r, actImage.color.g, actImage.color.b, alpha);
            actText.color = new Color(actText.color.r, actText.color.g, actText.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        if (actPage == imagesOfIntro.Count)
        {
            intro.SetActive(false);
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<PlayerMovement>().canMove = true;
            active = false;
        }
        else
        {
            newPage = true;
        }
        yield break;
    }

    //If the button is clicked this is called
    public void NextPage()
    {
        actPage++;
        FadeOut();
    }

    public bool IsActivated()
    {
        return active;
    }
}
