using UnityEngine;
using System.Collections;

public class Quest : MonoBehaviour {

    /// <summary>
    /// Should the quest be activated from the start on?
    /// </summary>
    public bool activateOnStart = false;

    /// <summary>
    /// Once the quest is activated, how should the quest be started? Either after a defined time or after entering a zone
    /// </summary>
    public enum StartMode { Time = 0, Zone = 1 }
    public StartMode startMode = StartMode.Time;

    /// <summary>
    /// Time delay if StartMode is Time
    /// </summary>
    public float startDelay = 0.0f;

    /// <summary>
    /// Start zone if StartMode is Zone
    /// </summary>
    public Transform startZone;

    /// <summary>
    /// Size of the start zone if StartMode is Zone
    /// </summary>
    public float startZoneSize = 5.0f;

    /// <summary>
    /// Text that will be shown on activation/start/finish/fail - can be left empty
    /// </summary>
    public string activateText;
    public string startText;
    public string finishText;
    public string failText;

    /// <summary>
    /// Time that the text will be shown
    /// </summary>
    public float startTextTime;
    public float activateTextTime;
    public float finishTextTime;
    public float failTextTime;

    /// <summary>
    /// GameObject with the quest script that should be started next
    /// </summary>
    public GameObject nextQuest;

    /// <summary>
    /// The current progress/state of the quest
    /// </summary>
    protected bool activated = false;
    protected bool questStarted = false;
    protected bool questFinished = false;

    protected static GameObject player;
    protected static ShowTutorialText textScript;
    protected static PrefabManager prefabs;

    /// <summary>
    /// Time that the quest was activated at
    /// </summary>
    private float activateTime;

    /// <summary>
    /// Time that the quest was started at
    /// </summary>
    protected float startTime;

    protected ArrayList markers = new ArrayList();

	protected void Start() {
        // Save game objects to static variables
        player = GameObject.FindGameObjectWithTag("Player");
        textScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<ShowTutorialText>();
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
                // ...check for finish conditions
                CheckFinish();
                // ...check for fail conditions
                CheckFail();

                // ...call the (overridable) OnUpdate() method for custom code
                OnUpdate();
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
        // Set activated bool to true
        activated = true;
        // Set the activate time to the current time
        activateTime = Time.time;
        // Show the defined activation text
        ShowUIText(activateText, activateTextTime);

        // Call the (overridable) OnQuestActivated() method
        OnQuestActivated();

        // If the start mode is "Zone"...
        if (startMode == StartMode.Zone)
        {
            // ...create a marker at the starting zone
            CreateMarker(startZone, startZoneSize);
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

        // Restart

        // Call the (overridable) OnQuestFailed() method
        OnQuestFailed();
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
        // Check, if the next quest is defined
        if (nextQuest != null)
        {
            // If yes, get the Quest component and activate the next quest
            nextQuest.GetComponent<Quest>().ActivateQuest();
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

    // Destroy all marker game objects
    private void DestroyMarkers()
    {
        // Iterate markers
        foreach (GameObject marker in markers)
        {
            // Destroy marker
            Destroy(marker);
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
}
