using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighlightButton : MonoBehaviour {
    public Text buttonText;

	// Use this for initialization
	void Start () {
	
	}

    public void OnPointerEnter()
    {
        buttonText.color = new Color(0.57F, 1F, 0.74F, 1F);
    }

    public void OnPointerExit()
    {
        buttonText.color = Color.white;
    }
}
