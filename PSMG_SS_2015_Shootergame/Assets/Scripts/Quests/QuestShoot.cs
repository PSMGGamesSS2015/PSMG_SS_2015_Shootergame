﻿using UnityEngine;
using System.Collections;

public class QuestShoot : Quest {

    public Shootable[] targets;
	public bool highlight = true;

    private ArrayList toShoot;
    private ArrayList shot;

    private float progress;

	void Start () {
        base.Start();
	}

    protected override void OnStart()
    {
        toShoot = new ArrayList();
        shot = new ArrayList();
        progress = 0.0f;

        for (int i = 0; i < targets.Length; i++)
        {
            toShoot.Add(targets[i]);
        }
    }

    protected override void OnQuestStarted()
    {
		if (highlight) {
			foreach (Shootable s in targets)
			{
				CreateHighlight(s.transform);
			}
		}

        SetGoal(((Shootable)toShoot[0]).transform);
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
        for (int i = 0; i < toShoot.Count; i++) {
            if (((Shootable)toShoot[i]).GetHealth() == 0.0f)
            {
                TargetShot(i);
            }
        }
    }

    void TargetShot(int index)
    {
        shot.Add(toShoot[index]);
        toShoot.RemoveAt(index);

        progress = (float) shot.Count / (toShoot.Count + shot.Count);

        if (toShoot.Count > 0)
        {
            SetGoal(((Shootable)toShoot[0]).transform);
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
