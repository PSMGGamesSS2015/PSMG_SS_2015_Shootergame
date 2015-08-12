using UnityEngine;
using System.Collections;

public class QuestCollect : Quest {

    public Collectable[] targets;

    private ArrayList toCollect;
    private ArrayList collected;

    private float progress;

	void Start () {
        base.Start();
	}

    protected override void OnStart()
    {
        toCollect = new ArrayList();
        collected = new ArrayList();
        progress = 0.0f;
        
        for (int i = 0; i < targets.Length; i++)
        {
            toCollect.Add(targets[i]);
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
    }

    protected override void CheckFinish()
    {
        if (progress >= 1.0f)
        {
            QuestFinished();
        }
    }
}
