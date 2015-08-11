using UnityEngine;
using System.Collections;

public class FollowQuest : Quest {

    /// <summary>
    /// The enemy that the players needs to follow
    /// </summary>
    public GameObject target;

    /// <summary>
    /// The goal that needs to be reached for the quest to finish
    /// </summary>
    public Transform goal;

    /// <summary>
    /// The player needs to remain within this distance to the target in order for the quest to not fail
    /// </summary>
    public float failDistance = 50.0f;
    /// <summary>
    /// Distance the finish trigger (set above) needs to have to the goal in order for the quest to finish
    /// </summary>
    public float goalDistance = 10.0f;

    public enum FinishTrigger { Player = 0, Target = 1 }
    /// <summary>
    /// Choose if the player or the target have to be within range of the goal to finish the quest
    /// </summary>
    public FinishTrigger finishTrigger = FinishTrigger.Player;

    private Transform finishTriggerObject;

	void Start () {
        base.Start();
	}

    protected override void OnStart()
    {
        SetFinishTrigger();
    }

    void SetFinishTrigger()
    {
        switch (finishTrigger)
        {
            case FinishTrigger.Player: finishTriggerObject = player.transform;
                break;
            case FinishTrigger.Target: finishTriggerObject = target.transform;
                break;
        }
    }
	
	void Update () {
        base.Update();
	}

    protected override void OnQuestStarted()
    {
        target.GetComponent<WaypointMovement>().StartMoving();

        CreateMarker(target.transform, failDistance, false);
        CreateMarker(goal, goalDistance);
    }

    protected override void CheckFail()
    {
        Vector3 playerPosition = player.transform.position;

        float distance = Vector3.Distance(playerPosition, transform.position);

        if (distance >= failDistance)
        {
            QuestFailed();
        }
    }

    protected override void CheckFinish()
    {
        float distance = Vector3.Distance(finishTriggerObject.position, goal.position);

        if (distance <= goalDistance)
        {
            QuestFinished();
        }
    }
}
