using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShowTutorialText : MonoBehaviour {

    //Tutorial text
    public Text textfield;

    //Tutorial text background
    public Image background;

    //the time the text should be shown
    private float timeToShowText = 5f;

    //Time to fade text in or out
    private float textFadeDuration = 1.0f;

    private bool animationReady = true;

    private List<string[]> textElements = new List<string[]>();

    // Use this for initialization
    void Start()
    {
        showTextinUI("Test text zum schaun wie das alles aussieht! ", 10);
    }

    void Update()
    {
        if (textElements.Count > 0)
        {
            if (animationReady)
            {
                animationReady = false;
                textfield.enabled = true;
                textfield.text = textElements[0][0];
                timeToShowText = float.Parse(textElements[0][1]);
                textfield.fontSize = int.Parse(textElements[0][2]);
                //Debug.Log(textElements[0][0] + " " + textElements[0][1] + " " + textElements[0][2]);
                textElements.RemoveAt(0);
                FadeIn();
                FadeOut();
            }
        }
    }

    private void addToQueue(string text, float time, float duration)
    {
        string[] arr = { text, time.ToString(), duration.ToString() };
        textElements.Add(arr);
    }


    //show the text for a specific time at the ui and then fade it out
    public void showTextinUI(string text, float time)
    {
        showTextinUI(text, time, 30);
    }

    //show the text for a specific time at the ui and then fade it out
    public void showTextinUI(string text, float time, int textSize)
    {
        addToQueue(text, time, textSize);
    }

    //Fade the text in
    public void FadeIn()
    {
        StartCoroutine("FadeInCR");
    }

    //Coroutine to fade the text in
    private IEnumerator FadeInCR()
    {
        float alpha = 1;
        float currentTime = 0f;
        while (alpha > 0)
        {
            alpha = Mathf.Lerp(1f, 0f, currentTime / textFadeDuration);
            textfield.color = new Color(textfield.color.r, textfield.color.g, textfield.color.b, 1 - alpha);
            background.color = new Color(background.color.r, background.color.g, background.color.b, 1 - alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
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
        yield return new WaitForSeconds(timeToShowText);
        while (alpha > 0)
        {
            alpha = Mathf.Lerp(1f, 0f, currentTime / textFadeDuration);
            textfield.color = new Color(textfield.color.r, textfield.color.g, textfield.color.b, alpha);
            background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        animationReady = true;
        yield break;
    }
}
