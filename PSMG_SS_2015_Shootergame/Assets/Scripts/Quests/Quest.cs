﻿/* QUEST
 * Base class for every quest.
 * Basic functionality:
 * > A quest is "hidden" until it is ACTIVATED
 * > Once activated, the quest can be STARTED by the player - either by entering a defined zone or automatically after a defined time
 * > Once started, the goal and fail conditions will be checked until the player either FINISHES or FAILS the quest (death will also fail the quest)
 */

using UnityEngine;
using System.Collections;

public class Quest : MonoBehaviour {

    // Should the quest be activated from the start on?
    public bool activateOnStart = false;

    // Register the quest with the QuestManager? Important if multiple quests are active at the same time.
    public bool registerQuest = true;

    // Should the quest be reset after the player fails?
    public bool resetAfterFail = true;

    // Once the quest is activated, how should the quest be started? Either after a defined time or after entering a zone
    public enum StartMode { Time = 0, Zone = 1 }
    public StartMode startMode = StartMode.Time;

    // Time delay if StartMode is Time
    public float startDelay = 0.0f;

    // Start zone if StartMode is Zone
    public Transform startZone;

    // Size of the start zone if StartMode is Zone
    public float startZoneSize = 5.0f;

    // Title of the quest in the log book ("M" key)
	public string questTitle = "Titel der Quest";
    // Description in the log book
	public string questDescription = "Beschreibung der Quest";
    // Image in the log book
    public Sprite questImage;
    
    // Text that will be shown on activation/start/finish/fail - can be left empty
    public string activateText;
    public string startText;
    public string finishText;
    public string failText;

    // Time that the text will be shown
    public float startTextTime;
    public float activateTextTime;
    public float finishTextTime;
    public float failTextTime;

    // GameObject with the quest script that should be started next - multiple quests possible
    public Quest[] nextQuests;

    // Should the quest use the compass to show the next destination?
    public bool useCompass = true;

    // Fail the quest if an enemy has spotted the player
	public bool failIfSpotted = false;

    // The current progress/state of the quest
    protected bool activated = false;
    protected bool questStarted = false;
    protected bool questFinished = false;

    // GameObjects, Components, ...
    protected static GameObject player;
    protected static BasePlayer basePlayer;
    protected static ShowTutorialText textScript;
	protected static OpenLogBook logBook;
    protected static TurnCompass compass;
    protected static PrefabManager prefabs;
    protected static QuestManager questManager;

    private GameObject canvas;

    // Time that the quest was activated at
    private float activateTime;

    // Time that the quest was started at
    protected float startTime;

    // Markers and highlighters
    protected ArrayList markers = new ArrayList();
	protected ArrayList highlighter = new ArrayList();

    // The player's starting position (for reset)
    private Vector3 playerStartingPosition;

	protected void Start() {
        // Save game objects to static variables
        player = GameObject.FindGameObjectWithTag("Player");
        basePlayer = player.GetComponent<BasePlayer>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        questManager = GameObject.FindGameObjectWithTag("QuestManager").GetComponent<QuestManager>();
        textScript = canvas.GetComponent<ShowTutorialText>();
		logBook = canvas.GetComponent<OpenLogBook> ();
        compass = canvas.GetComponent<TurnCompass>();
        prefabs = GameObject.FindGameObjectWithTag("Prefabs").GetComponent<PrefabManager>();

        // If the quest should be activated on start, activate it
        if (activateOnStart)
        {
            ActivateQuest();
        }

        // Call the (overridable) OnStart() method for custom code
        OnStart();
	}
	
	protected void Update () {
        // If the quest is activated....
        if (activated)
        {
            // And if the quest has not been started yet...
            if (!questStarted)
            {
                // ...check the starting conditions
                CheckStart();
            }
            // And if the quest has already been started...
            else
            {
                // ...call the (overridable) OnUpdate() method for custom code
                OnUpdate();

                // ...check for fail conditions
                CheckFail();

                // ...check for finish conditions
                CheckFinish();                
            }
        }
        foreach (GameObject marker in markers) {
            if (marker != null)
            {
                marker.transform.Rotate(0, 40 * Time.deltaTime, 0, Space.World);
            }
        }
	}

    // Checks if the quest has been started
    private void CheckStart()
    {
        // If the quest should be started after a certain time...
        if (startMode == StartMode.Time)
        {
            // Check if the difference between the current time and the time of activation is greater than the defined delay
            if (Time.time - activateTime >= startDelay)
            {
                // If it is greater, start the quest
                StartQuest();
            }
        }
        // If the quest should be started after entering a zone...
        else if (startMode == StartMode.Zone)
        {
            // Calculate the distance between the player and the start zone and check if it is lower than the defined size of the zone
            if (Vector3.Distance(player.transform.position, startZone.position) <= startZoneSize)
            {
                // If the distance is smaller than the size, the player is in the zone and the quest is started
                StartQuest();
            }
        }
        
    }

    // Activate the quest
    public void ActivateQuest()
    {
        if (registerQuest)
        {
            questManager.SetQuest(this);
        }

        // Set activated bool to true
        activated = true;
        // Set the activate time to the current time
        activateTime = Time.time;
        // Show the defined activation text
        ShowUIText(activateText, activateTextTime);
		// Update LogBook texts
        SetLogBookText(questImage, questTitle, questDescription);

        playerStartingPosition = player.transform.position;

        basePlayer.Save();

        // Call the (overridable) OnQuestActivated() method
        OnQuestActivated();

        // If the start mode is "Zone"...
        if (startMode == StartMode.Zone)
        {
            // ...create a marker at the starting zone
            CreateMarker(startZone, startZoneSize);

            SetGoal(startZone);
        }
    }

    // Start the quest
    public void StartQuest()
    {
        // Set started bool to true
        questStarted = true;
        // Set the start time to the current time
        startTime = Time.time;
        // Show the defined start text
        ShowUIText(startText, startTextTime);
        
        // Destroy all the markers (which should only be the start zone marker if the start mode is "Zone")
        DestroyMarkers();

        // Call the (overridable) OnQuestStarted() method
        OnQuestStarted();
    }

    // Quest failed
    protected void QuestFailed()
    {
        // Set started bool to false
        questStarted = false;
        // Show the defined fail text
        ShowUIText(failText, failTextTime);

        // Destroy all the markers
        DestroyMarkers();

        // Call the (overridable) OnQuestFailed() method
        OnQuestFailed();

        RestartQuest();
    }
	
	protected void QuestFailed(bool restart)
	{
		questStarted = false;
		ShowUIText(failText, failTextTime);
		DestroyMarkers();
		OnQuestFailed();

		if (restart) {
			RestartQuest ();
		}
	}

    public void ForceReset()
    {
        QuestFailed(false);
        Reset();
        ActivateQuest();
    }

    protected void RestartQuest()
    {
        if (resetAfterFail)
        {
            Reset();
        }

        ActivateQuest();
    }

    protected void Reset()
    {
        player.transform.position = playerStartingPosition;
        basePlayer.Load();
        OnReset();
    }

    // Quest finished
    protected void QuestFinished()
    {
        // Set activate, started, finished bools to false
        activated = false;
        questStarted = false;
        questFinished = false;

        // Destroy all the markers
        DestroyMarkers();

        // Show the defined finish text
        ShowUIText(finishText, finishTextTime);

        // Activate the next quest
        ActivateNextQuest();

        // Call the (overridable) OnQuestFinished() method
        OnQuestFinished();
    }

    // Activate the next quest
    private void ActivateNextQuest()
    {
        if (nextQuests != null)
        {
            foreach (Quest quest in nextQuests)
            {
                quest.GetComponent<Quest>().ActivateQuest();
            }
        }
    }

    // Create a marker on the set position and with the set size
    protected void CreateMarker(Transform position, float size)
    {
        // Instantiate the prefab of the prefab manager on the set position
        GameObject marker = Instantiate(prefabs.rangeIndicator, position.position, Quaternion.Euler(90, 0, 0)) as GameObject;
        // Set the marker object as a child of the set transform
        marker.transform.parent = position;
        // Set the size of the marker
        marker.GetComponent<Projector>().orthographicSize = size;

        // Add the marker to the marker array list
        markers.Add(marker);
    }

    // Create a marker without the particle system
    protected void CreateMarker(Transform position, float size, bool particles)
    {
        GameObject marker = Instantiate(prefabs.rangeIndicator, position.position, Quaternion.Euler(90, 0, 0)) as GameObject;
        marker.transform.parent = position;
        marker.GetComponent<Projector>().orthographicSize = size;
        if (!particles)
        {
            marker.GetComponentInChildren<ParticleSystem>().Stop();
        }
        markers.Add(marker);
    }

	// Create a highlighter particle system
	protected void CreateHighlight(Transform position)
	{
		GameObject highlight = Instantiate(prefabs.highlighter, position.position, Quaternion.Euler(90, 0, 0)) as GameObject;
		highlight.transform.parent = position;
		highlighter.Add(highlight);
	}

    protected void SetGoal(Transform t)
    {
        if (useCompass)
        {
            compass.SetGoal(t);
        }
    }

	public void PlayerSpotted() {
		if (failIfSpotted) {
			QuestFailed ();
		}
	}

    // Destroy all marker game objects
    private void DestroyMarkers()
    {
        // Iterate markers
        foreach (GameObject marker in markers)
        {
            // Destroy marker
            Destroy(marker);
        }

		// Iterate highlighter
		foreach (GameObject highlight in highlighter)
		{
			// Destroy highlighter
			Destroy(highlight);
		}
    }

    // Show text in the UI
    private void ShowUIText(string text, float time)
    {
        // Check if text and time are not null
        if (text != "" && time > 0.0f)
        {
            // Access the text script and show the set text for the set time
            textScript.showTextinUI(text, time);
        }
    }

	private void SetLogBookText(Sprite image, string title, string description) {
		logBook.setQuestData (image, title, description);
	}

    protected virtual void CheckFinish()
    {

    }

    protected virtual void CheckFail()
    {

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

    protected virtual void OnStart()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void OnReset()
    {

    }
}
