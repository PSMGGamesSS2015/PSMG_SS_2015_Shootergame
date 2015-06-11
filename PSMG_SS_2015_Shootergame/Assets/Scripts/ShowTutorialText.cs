using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowTutorialText : MonoBehaviour {

    //Tutorial text
    public Text textfield;

    //the time the text should be shown
    private float timeToShowText = 5f;

    //Time to fade text in or out
    private float textFadeDuration = 1.0f;

    // Use this for initialization
    void Start()
    {
        //showTextinUI("Test text zum schaun wie das alles aussieht! ", 5);
        //showTextinUI("Test text zum schaun wie das alles aussieht! ", 5, Color.red, 50);
    }

    //show the text for a specific time at the ui and then fade it out
    public void showTextinUI(string text, float time)
    {
        textfield.enabled = true;
        textfield.text = text;
        FadeIn();
        FadeOut();
    }

    //show the text for a specific time at the ui and then fade it out
    public void showTextinUI(string text, float time, Color textColor, int textSize)
    {
        textfield.enabled = true;
        textfield.text = text;
        textfield.color = textColor;
        textfield.fontSize = textSize;

        FadeIn();
        FadeOut();
    }

    //show the text for a specific time at the ui and then fade it out
    public void showTextinUI(string text, float time, int textSize)
    {
        textfield.enabled = true;
        textfield.text = text;
        textfield.fontSize = textSize;

        FadeIn();
        FadeOut();
    }

    //Fade the text in
    public void FadeIn()
    {
        StartCoroutine("FadeInCR");
    }

    //Coroutine to fade the text in
    private IEnumerator FadeInCR()
    {
        float currentTime = 0f;
        while (currentTime < textFadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / textFadeDuration);
            textfield.color = new Color(textfield.color.r, textfield.color.g, textfield.color.b, 1 - alpha);
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

    //Coroutine to fade the text out (wait for timeToShowText so the text is visible this amount of time
    private IEnumerator FadeOutCR()
    {
        float currentTime = 0f;
        yield return new WaitForSeconds(timeToShowText);
        while (currentTime < textFadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / textFadeDuration);
            textfield.color = new Color(textfield.color.r, textfield.color.g, textfield.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield break;
    }
}
