using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenLogBook : MonoBehaviour {

    public GameObject logbook;
    public GameObject allExceptLogbook;

    public Image[] totemColors;
    public Totem[] totems;
    public Image[] totemImages;


    public Text questTitle;
    public Text questDescription;
    public Image questImage;

	// Use this for initialization
	void Start () {
        //setQuestData("Titel!!!", "Das ist eine Test-quest!");
	}
	
	// Update is called once per frame
	void Update () {
        if (totems[0].IsActive())
        {
            SetTotemsVisible(false);
            totemColors[0].fillAmount = totems[0].getPct();
        }

        if (totems[1].IsActive())
        {
            SetTotemsVisible(false);
            totemColors[1].fillAmount = totems[1].getPct();
        }

        if (totems[2].IsActive())
        {
            SetTotemsVisible(false);
            totemColors[2].fillAmount = totems[2].getPct();
        }

        if (totems[3].IsActive())
        {
            SetTotemsVisible(false);
            totemColors[3].fillAmount = totems[3].getPct();
        }

        if (totems[4].IsActive())
        {
            SetTotemsVisible(true);
            totemColors[4].fillAmount = totems[4].getPct();
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

    //public void setQuestData(Image image, string title, string description)
    public void setQuestData(string title, string description)
    {
        //questImage = image;
        questDescription.text = description;
        questTitle.text = title;
    }

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
