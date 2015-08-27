using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenLogBook : MonoBehaviour {

    //Gameobject of the logbook
    public GameObject logbook;

    //Gameobject of the Panel. Everything on the UI except the logbook
    public GameObject allExceptLogbook;

    //Get the colored images of the Totems in the UI 
    public Image[] totemColors;
    //Get the Totems (in the 3d world)
    public Totem[] totems;
    //Get the images of the Totems in the UI 
    public Image[] totemImages;

    //the Title of the Quest
    public Text questTitle;
    //the Description of the Quest
    public Text questDescription;
    //the Image of the Quest
    public Image questImage;
    //placeholder for quest image
    public Sprite placeholder;

	// Use this for initialization
	void Start () {
        //setQuestData(placeholder, "Titel!!!", "Das ist eine Test-quest!");
	}
	
	//Set the different images of the Totems visible and Invisible.
    //Also set the logbook visible or invisible
	void Update () {
        for (int i = 0; i < totems.Length; i++ )
        {
            if (totems[i].IsActive())
            {
                totemColors[i].fillAmount = totems[i].getPct();
            }
            else
            {
                totemColors[i].fillAmount = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            logbook.SetActive(true);
            allExceptLogbook.SetActive(false);
        }
        else if (Input.GetKeyUp(KeyCode.M))
        {
            logbook.SetActive(false);
            allExceptLogbook.SetActive(true);
        }
	}

    //Set the Data of the Quest.
    public void setQuestData(Sprite image, string title, string description)
    {
        if (image != null)
        {
            questImage.sprite = image;
        }
        else
        {
            questImage.sprite = placeholder;
        }
        questDescription.text = description;
        questTitle.text = title;
    }
}
