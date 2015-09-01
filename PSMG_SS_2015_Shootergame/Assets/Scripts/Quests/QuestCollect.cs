/* QUEST COLLECT
 * Quest where the player needs to collect Collectable objects.
 */

using UnityEngine;
using System.Collections;

public class QuestCollect : Quest {

    // Collectable objects that need to be collected
    public Collectable[] targets;
    // Should the objects be highlighted?
	public bool highlight = true;

    // Lists for the collected and yet to be collected objects
    private ArrayList toCollect;
    private ArrayList collected;

    // Progress of the quest
    private float progress;

    protected override void OnQuestStarted()
    {
        // Initialize the ArrayLists
		toCollect = new ArrayList();
		collected = new ArrayList();
        // Set progress to 0
		progress = 0.0f;
		
        // Add all of the targets to the toCollect list
		for (int i = 0; i < targets.Length; i++)
		{
			toCollect.Add(targets[i]);
		}

        // If the objects should be highlighted...
		if (highlight) {
            // Iterate the targets
			foreach (Collectable c in targets)
			{
                // Create highlights for each target
				CreateHighlight(c.transform);
			}
		}

        // Set the goal to the first element in the list
        SetGoal(((Collectable)toCollect[0]).transform);
    }

    // On reset of the quest
	protected override void OnReset() {
        // Reset every collectable object
		foreach (Collectable c in targets) {
			c.Reset ();
		}
	}

    protected override void OnUpdate()
    {
        CheckTargets();
    }

    // Check if an object has been collected
    void CheckTargets()
    {
        // For every object in the toCollect list...
        for (int i = 0; i < toCollect.Count; i++)
        {
            // Check if the object has been collected
            if (((Collectable)toCollect[i]).IsCollected())
            {
                TargetCollected(i);
            }
        }
    }

    // An object has been collected
    void TargetCollected(int index)
    {
        // Add the object to the collected list
        collected.Add(toCollect[index]);
        // Remove it from the toCollect list
        toCollect.RemoveAt(index);

        // Update the progress
        progress = (float) collected.Count / (toCollect.Count + collected.Count);

        // Check if there are still objects to be collected
        if (toCollect.Count > 0)
        {
            // If yes, update the goal of the compass
            SetGoal(((Collectable)toCollect[0]).transform);
        }
    }

    // Check if the quest has been finished
    protected override void CheckFinish()
    {
        // If the progress is equal to/greater than 1
        if (progress >= 1.0f)
        {
            // Quest is finished!
            QuestFinished();
        }
    }
}