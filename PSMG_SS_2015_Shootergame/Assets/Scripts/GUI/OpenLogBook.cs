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
    //the Image of the Quest (NOT IMPLEMENTED)
    public Image questImage;

	// Use this for initialization
	void Start () {
        //setQuestData("Titel!!!", "Das ist eine Test-quest!");
	}
	
	//Set the different images of the Totems visible and Invisible.
    //Also set the logbook visible or invisible
	void Update () {
        for (int i = 0; i < totems.Length; i++ )
        {
            if (totems[i].bonusTotem)
            {
                if (totems[i].IsActive())
                {
                    SetTotemsVisible(true);
                    totemColors[i].fillAmount = totems[i].getPct();
                }
            }
            else
            {
                if (totems[i].IsActive())
                {
                    SetTotemsVisible(false);
                    totemColors[i].fillAmount = totems[i].getPct();
                }
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

    //Set the Data of the Quest. (IMAGE NOT IMPLEMENTED)
    public void setQuestData(string title, string description)
    {
        //questImage = image;
        questDescription.text = description;
        questTitle.text = title;
    }

    //Method to set the Totems visible or not
    private void SetTotemsVisible(bool supertotem)
    {
        totemColors[0].enabled = !supertotem;
        totemColors[1].enabled = !supertotem;
        totemColors[2].enabled = !supertotem;
        totemColors[3].enabled = !supertotem;
        totemColors[4].enabled = supertotem;


        totemImages[0].enabled = !supertotem;
        totemImages[1].enabled = !supertotem;
        totemImages[2].enabled = !supertotem;
        totemImages[3].enabled = !supertotem;
        totemImages[4].enabled = supertotem;
    }
}
