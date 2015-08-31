using UnityEngine;
using System.Collections;

public class QuestCollect : Quest {

    public Collectable[] targets;
	public bool highlight = true;

    private ArrayList toCollect;
    private ArrayList collected;

    private float progress;

	void Start () {
        base.Start();
	}

    protected override void OnQuestStarted()
    {
		toCollect = new ArrayList();
		collected = new ArrayList();
		progress = 0.0f;
		
		for (int i = 0; i < targets.Length; i++)
		{
			toCollect.Add(targets[i]);
		}

		if (highlight) {
			foreach (Collectable c in targets)
			{
				CreateHighlight(c.transform);
			}
		}

        SetGoal(((Collectable)toCollect[0]).transform);
    }

	protected override void OnReset() {
		foreach (Collectable c in targets) {
			c.Reset ();
		}
	}

	void Update () {
        base.Update();
	}

    protected override void OnUpdate()
    {
        CheckTargets();
    }

    void CheckTargets()
    {
        for (int i = 0; i < toCollect.Count; i++)
        {
            if (((Collectable)toCollect[i]).IsCollected())
            {
                TargetCollected(i);
            }
        }
    }

    void TargetCollected(int index)
    {
        collected.Add(toCollect[index]);
        toCollect.RemoveAt(index);

        progress = (float) collected.Count / (toCollect.Count + collected.Count);

        if (toCollect.Count > 0)
        {
            SetGoal(((Collectable)toCollect[0]).transform);
        }
    }

    protected override void CheckFinish()
    {
        if (progress >= 1.0f)
        {
            QuestFinished();
        }
    }
}
