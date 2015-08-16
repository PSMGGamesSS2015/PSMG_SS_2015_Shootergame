using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenLogBook : MonoBehaviour {

    public GameObject logbook;
    public GameObject allExceptLogbook;

    public Image soiltotemColor;
    public Image watertotemColor;
    public Image airtotemColor;
    public Image firetotemColor;


    public Totem soiltotem;
    public Totem watertotem;
    public Totem airtotem;
    public Totem firetotem;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (soiltotem.IsActive())
        {
            soiltotemColor.fillAmount = soiltotem.getPct();
        }

        if (watertotem.IsActive())
        {
            watertotemColor.fillAmount = watertotem.getPct();
        }

        if (airtotem.IsActive())
        {
            airtotemColor.fillAmount = airtotem.getPct();
        }

        if (firetotem.IsActive())
        {
            firetotemColor.fillAmount = firetotem.getPct();
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
}
