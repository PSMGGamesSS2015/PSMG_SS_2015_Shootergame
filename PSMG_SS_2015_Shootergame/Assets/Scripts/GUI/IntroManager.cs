﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class IntroManager : Quest {

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

    private bool active = false;

    private bool animationIsReady = false;
    public Text buttonText;

    //Called if the quest is started. sets the intro active
	protected override void OnQuestStarted() {
		Cursor.visible = true;
		player.GetComponent<PlayerMovement>().AllowMove(false);
		intro.SetActive(true);
		active = true;
	}

    //loads new image and text if its a new page
	protected override void OnUpdate() {
		if (newPage)
		{
			newPage = false;
			actImage = imagesOfIntro[actPage];
			actText = textOfIntro[actPage];
			FadeIn();
		}
        ChangeButtonColor();
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
        animationIsReady = true;
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
            player.GetComponent<PlayerMovement>().AllowMove(true);
            active = false;
            
            QuestFinished();

			Cursor.visible = false;
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
        if(animationIsReady) {
            actPage++;
            FadeOut();
            animationIsReady = false;
        }
    }

    //If the qeust is activated this returns true
    public bool IsActivated()
    {
        return active;
    }

    //changes the button color if the animation is not ready
    private void ChangeButtonColor()
    {
        if (!animationIsReady)
        {
            buttonText.color = Color.gray;
        }
        else
        {
            buttonText.color = Color.black;
        }
    }
}
