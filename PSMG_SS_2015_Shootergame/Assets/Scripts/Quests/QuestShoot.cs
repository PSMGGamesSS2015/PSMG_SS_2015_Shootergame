﻿using UnityEngine;
using System.Collections;

public class QuestShoot : Quest {

    public Shootable[] targets;

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
    }

    protected override void CheckFinish()
    {
        if (progress >= 1.0f)
        {
            QuestFinished();
        }
    }
}