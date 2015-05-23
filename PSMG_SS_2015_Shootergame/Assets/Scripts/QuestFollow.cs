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

    private bool questStarted = false;
    private bool questFinished = false;

    private GameObject startIndicator;
    private GameObject failIndicator;
    private GameObject goalIndicator;

	// Use this for initialization
	void Start () {
        if (finishTrigger == FinishTrigger.Player) {
            finishTriggerObject = player;
        } else if (finishTrigger == FinishTrigger.Target) {
            finishTriggerObject = transform;
        }

        startIndicator = CreateIndicator(startTrigger, startDistance);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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

        if (distance <= startDistance)
        {
            Destroy(startIndicator);
            failIndicator = CreateIndicator(transform, failDistance);
            goalIndicator = CreateIndicator(goal, goalDistance);
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
        Debug.Log("Quest started!");

        questStarted = true;
    }

    void QuestFailed()
    {
        Destroy(failIndicator);
        Destroy(goalIndicator);
        questStarted = false;
        Debug.Log("Quest has failed!");
    }

    void QuestFinished()
    {
        Debug.Log("Quest finished!");
        questStarted = false;
        questFinished = true;

        Destroy(failIndicator);
        Destroy(goalIndicator);
    }
}
