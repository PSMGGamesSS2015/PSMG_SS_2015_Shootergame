using UnityEngine;
using System.Collections;

public class QuestFollow : MonoBehaviour {

    public GameObject rangeIndicatorProjector;

    /// <summary>
    /// The object that needs to remain within range of the target - usually the plyaer
    /// </summary>
    public Transform player;

    /// <summary>
    /// The trigger that will start the quest - the quest will start if the player is within a set distance of the trigger
    /// </summary>
    public Transform startTrigger;

    /// <summary>
    /// The goal that needs to be reached for the quest to finish
    /// </summary>
    public Transform goal;

    public enum FinishTrigger { Player = 0, Target = 1 }
    /// <summary>
    /// Choose if the player or the target have to be within range of the goal to finish the quest
    /// </summary>
    public FinishTrigger finishTrigger = FinishTrigger.Player;

    /// <summary>
    /// Distance to the start trigger to start the quest
    /// </summary>
    public float startDistance = 50.0f;
    /// <summary>
    /// The player needs to remain within this distance to the target in order for the quest to not fail
    /// </summary>
    public float failDistance = 50.0f;
    /// <summary>
    /// Distance the finish trigger (set above) needs to have to the goal in order for the quest to finish
    /// </summary>
    public float goalDistance = 20.0f;

    private Transform finishTriggerObject;

    private bool activated = false;
    private bool questStarted = false;
    private bool questFinished = false;

    private GameObject startIndicator;
    private GameObject failIndicator;
    private GameObject goalIndicator;

    // Just for testing purposes - need to implement savegames!
    private Vector3 start_player;
    private Vector3 start_target;
    private Vector3 start_trigger;
    private Vector3 start_goal;

    public ShowTutorialText textScript;

    public string activateText;
    public string startText;
    public string finishText;
    public string failText;

    public float startTextTime;
    public float activateTextTime;
    public float finishTextTime;
    public float failTextTime;

	// Use this for initialization
	void Start () {
        // For test purposes only
        // ----------------------
        ActivateQuest();
        // ----------------------

        SetFinishTrigger();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (activated)
        {
            if (!questStarted)
            {
                CheckForQuestStart();
            }
            else
            {
                CheckFail();
                CheckFinish();
            }
        }        
	}

    void SetFinishTrigger()
    {
        switch (finishTrigger)
        {
            case FinishTrigger.Player: finishTriggerObject = player;
                break;
            case FinishTrigger.Target: finishTriggerObject = transform;
                break;
        }
    }

    GameObject CreateIndicator(Transform parent, float range)
    {
        GameObject indicator = Instantiate(rangeIndicatorProjector, parent.position, Quaternion.Euler(90, 0, 0)) as GameObject;
        indicator.transform.parent = parent;
        indicator.GetComponent<Projector>().orthographicSize = range;

        return indicator;
    }

    void CheckForQuestStart()
    {
        float distance = Vector3.Distance(player.position, startTrigger.position);
        float targetDistance = Vector3.Distance(transform.position, startTrigger.position);

        if (distance <= startDistance && targetDistance <= (failDistance * 0.75f))
        {
            StartQuest();
        }
    }

    void CheckFail()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance >= failDistance)
        {
            QuestFailed();
        }
    }

    void CheckFinish()
    {
        float distance = Vector3.Distance(finishTriggerObject.position, goal.position);
        if (distance <= goalDistance)
        {
            QuestFinished();
        }
    }

    void StartQuest() 
    {
        questStarted = true;

        textScript.showTextinUI(startText, startTextTime);

        GetComponent<WaypointMovement>().StartMoving();

        SaveStartParameters();

        Destroy(startIndicator);

        failIndicator = CreateIndicator(transform, failDistance);
        goalIndicator = CreateIndicator(goal, goalDistance);        
    }

    void QuestFailed()
    {
        Destroy(failIndicator);
        Destroy(goalIndicator);

        questStarted = false;

        textScript.showTextinUI(failText, failTextTime);

        ResetQuest();
    }

    void QuestFinished()
    {
        questStarted = false;
        questFinished = true;

        textScript.showTextinUI(finishText, finishTextTime);

        Destroy(failIndicator);
        Destroy(goalIndicator);

        // For test purposes only
        // ----------------------
        ActivateQuest();
        // ----------------------
    }

    void SaveStartParameters()
    {
        start_player = player.transform.position;
        start_target = transform.position;
        start_trigger = startTrigger.transform.position;
        start_goal = goal.transform.position;
        GetComponent<WaypointMovement>().SaveWaypoint();
    }

    void ResetQuest()
    {
        player.transform.position = start_player;
        transform.position = start_target;
        startTrigger.transform.position = start_trigger;
        goal.transform.position = start_goal;
        GetComponent<WaypointMovement>().LoadWaypoint();
        GetComponent<WaypointMovement>().StopMoving();

        // For test purposes only
        // ----------------------
        ActivateQuest();
        // ----------------------
    }

    public void ActivateQuest()
    {
        textScript.showTextinUI(activateText, activateTextTime);

        activated = true;
        startIndicator = CreateIndicator(startTrigger, startDistance);
    }
}
