/* QUEST FOLLOW
 * A follow quest where the player has to follow a target to a destination.
 */

using UnityEngine;
using System.Collections;

public class QuestFollow : Quest
{
    // The enemy that the players needs to follow
    public GameObject target;

    // The goal that needs to be reached for the quest to finish
    public Transform goal;

    // The player needs to remain within this distance to the target in order for the quest to not fail
    public float failDistance = 50.0f;
    // Distance the finish trigger (set above) needs to have to the goal in order for the quest to finish
    public float goalDistance = 10.0f;

    // Choose if the player or the target have to be within range of the goal to finish the quest
    public enum FinishTrigger { Player = 0, Target = 1 }    
    public FinishTrigger finishTrigger = FinishTrigger.Player;

    // Target that the compass should be facing
    public enum CompassTarget { Target = 0, Goal = 1 }
    public CompassTarget compassTarget = CompassTarget.Target;

    private Transform finishTriggerObject;

    // Starting position of the target (saved in case of fail/death of the player)
    private Vector3 targetStartingPosition;


    protected override void OnStart()
    {
        SetFinishTrigger();
    }

    protected override void OnQuestActivated()
    {
        // Save the starting position of the target
        targetStartingPosition = target.transform.position;
    }

    // If the quest has been reset...
    protected override void OnReset()
    {
        // Reset movement of the target
        target.GetComponent<WaypointMovement>().ResetMovement();
        // Reset the enemy script
		target.GetComponent<Enemy> ().Reset ();
        // Reset the position of the target
        target.transform.position = targetStartingPosition;
    }

    // Configure the finish trigger
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

    protected override void OnQuestStarted()
    {
        target.GetComponent<WaypointMovement>().StartMoving();

        CreateMarker(target.transform, failDistance, false);
        CreateMarker(goal, goalDistance);

        switch (compassTarget)
        {
            case CompassTarget.Target:
                SetGoal(target.transform);
                break;
            case CompassTarget.Goal:
                SetGoal(goal);
                break;
        }
    }

    protected override void CheckFail()
    {
        Vector3 playerPosition = player.transform.position;

        float distance = Vector3.Distance(playerPosition, target.transform.position);

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
