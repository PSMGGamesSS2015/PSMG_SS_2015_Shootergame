using UnityEngine;
using System.Collections;

public class QuestKill : Quest {
    public Enemy[] targets;

    public bool highlight = false;

    private ArrayList toKill;
    private ArrayList killed;

    private float progress;

    protected override void OnStart()
    {
        foreach (Enemy e in targets)
        {
            e.gameObject.SetActive(false);
        }
    }

    protected override void OnQuestActivated()
    {
        foreach (Enemy e in targets)
        {
            e.gameObject.SetActive(true);
        }
    }

    protected override void OnQuestStarted()
    {
        toKill = new ArrayList();
        killed = new ArrayList();
        progress = 0.0f;
        
        for (int i = 0; i < targets.Length; i++)
        {
            toKill.Add(targets[i]);
        }

        if (highlight)
        {
            foreach (Enemy e in targets)
            {
                CreateHighlight(e.transform);
            }
        }

        SetGoal(((Enemy)toKill[0]).transform);
    }

    protected override void OnReset()
    {
        // NOT COMPLETELY IMPLEMENTED
        foreach (Enemy e in targets)
        {
            e.Reset();
        }
    }
    protected override void OnUpdate()
    {
        CheckTargets();
    }

    void CheckTargets()
    {
        for (int i = 0; i < toKill.Count; i++)
        {
            if (((Enemy)toKill[i]).Health <= 0)
            {
                EnemyKilled(i);
            }
        }
    }

    void EnemyKilled(int index)
    {
        killed.Add(toKill[index]);
        toKill.RemoveAt(index);

        progress = (float)killed.Count / (toKill.Count + killed.Count);

        if (toKill.Count > 0)
        {
            SetGoal(((Enemy)toKill[0]).transform);
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
