/* QUEST DESTROY
 * A quest where one or multiple objects have to be destroyed.
 */

using UnityEngine;
using System.Collections;

public class QuestDestroy : Quest {

    // Objects that have to be destroyed
    public Destroyable[] targets;
    // Highlight the target objects?
	public bool highlight = true;

    // Objects that still have to be destroyed
    private ArrayList toShoot;
    // Objects that have been destroyed
    private ArrayList shot;

    // Current progress of the quest
    private float progress;
    
    protected override void OnStart()
    {
        // Initialize the ArrayLists
        toShoot = new ArrayList();
        shot = new ArrayList();
        // Set progress to 0
        progress = 0.0f;

        // Add all of the targets to the toShoot list
        for (int i = 0; i < targets.Length; i++)
        {
            toShoot.Add(targets[i]);
        }
    }

    protected override void OnQuestStarted()
    {
        // If the objects should be highlighter...
		if (highlight) {
            // Iterate the targets
			foreach (Destroyable s in targets)
			{
                // Create a highlighter for each target
				CreateHighlight(s.transform);
			}
		}

        // Set the goal of the compass to the first element in the list
        SetGoal(((Destroyable)toShoot[0]).transform);
    }

    protected override void OnUpdate()
    {
        CheckTargets();
    }

    // Check if an object has been destroyed
    void CheckTargets()
    {
        // Iterate all of the objects still to be destroyed
        for (int i = 0; i < toShoot.Count; i++) {
            // If the health of the object is 0...
            if (((Destroyable)toShoot[i]).GetHealth() == 0.0f)
            {
                // ...it has been destroyed!
                TargetShot(i);
            }
        }
    }

    // An object has been destroyed
    void TargetShot(int index)
    {
        // Add it to the shot list
        shot.Add(toShoot[index]);
        // Remove it from the toShoot list
        toShoot.RemoveAt(index);

        // Update the progress
        progress = (float) shot.Count / (toShoot.Count + shot.Count);

        // If there are still reaming objects to be destroyed
        if (toShoot.Count > 0)
        {
            // Set the compass goal to the next element
            SetGoal(((Destroyable)toShoot[0]).transform);
        }
    }

    protected override void CheckFinish()
    {
        // Check if the quest is finished
        if (progress >= 1.0f)
        {
            QuestFinished();
        }
    }
}
