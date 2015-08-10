using UnityEngine;
using System.Collections;

public class Quest : MonoBehaviour {

    public bool activateOnStart = false;

    public string activateText;
    public string startText;
    public string finishText;
    public string failText;

    public float startTextTime;
    public float activateTextTime;
    public float finishTextTime;
    public float failTextTime;

    public GameObject nextQuest;

    protected bool activated = false;
    protected bool questStarted = false;
    protected bool questFinished = false;

    protected static GameObject player;
    protected static ShowTutorialText textScript;
    protected static TutorialGhost ghost;
    protected static PrefabManager prefabs;

    protected float startTime;

    protected ArrayList markers;

	// Use this for initialization
	protected virtual void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        textScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<ShowTutorialText>();
        //ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<TutorialGhost>();
        prefabs = GameObject.FindGameObjectWithTag("Prefabs").GetComponent<PrefabManager>();

        if (activateOnStart)
        {
            ActivateQuest();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ActivateQuest()
    {
        activated = true;
        ShowUIText(activateText, activateTextTime);

        OnQuestActivated();

        // Create marker
        // Set ghost
    }

    public void StartQuest()
    {
        questStarted = true;
        ShowUIText(startText, startTextTime);

        startTime = Time.time;

        OnQuestStarted();
    }

    private void QuestFailed()
    {
        questStarted = false;
        ShowUIText(failText, failTextTime);

        DestroyMarkers();

        // Restart

        OnQuestFailed();
    }

    private void QuestFinished()
    {
        activated = false;
        questStarted = false;
        questFinished = false;

        DestroyMarkers();

        ShowUIText(finishText, finishTextTime);

        ActivateNextQuest();

        OnQuestFinished();
    }

    private void ActivateNextQuest()
    {
        if (nextQuest != null)
        {
            nextQuest.GetComponent<Quest>().ActivateQuest();
        }
    }

    private void CreateMarker(Transform position, float size)
    {
        GameObject marker = Instantiate(prefabs.rangeIndicator, position.position, Quaternion.Euler(90, 0, 0)) as GameObject;
        marker.transform.parent = position;
        marker.GetComponent<Projector>().orthographicSize = size;

        markers.Add(marker);
    }

    private void DestroyMarkers()
    {
        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }
    }

    private void ShowUIText(string text, float time)
    {
        textScript.showTextinUI(text, time);
    }

    protected virtual void OnQuestActivated()
    {

    }

    protected virtual void OnQuestStarted()
    {

    }

    protected virtual void OnQuestFinished()
    {

    }

    protected virtual void OnQuestFailed()
    {

    }
}
