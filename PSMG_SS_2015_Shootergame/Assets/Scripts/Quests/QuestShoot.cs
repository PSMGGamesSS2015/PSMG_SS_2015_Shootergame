using UnityEngine;
using System.Collections;

public class QuestShoot : Quest {

    public GameObject[] targets;

    private int toShoot;
    private int shot;

    private float progress;

    private ArrayList shootables;

	void Start () {
        base.Start();
	}

    protected override void OnStart()
    {
        toShoot = targets.Length;
        shot = 0;
        progress = 0.0f;
        shootables = new ArrayList();

        for (int i = 0; i < targets.Length; i++)
        {
            shootables.Add(targets[i].GetComponent<Shootable>());
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
        for (int i = 0; i < shootables.Count; i++) {
            if (((Shootable)shootables[i]).GetHealth() == 0.0f) {
                shootables.RemoveAt(i);
                UpdateProgress();
            }
        }
    }

    void UpdateProgress()
    {
        toShoot--;
        shot++;
        progress = shot / (toShoot + shot);
    }

    protected override void CheckFinish()
    {
        if (toShoot == 0)
        {
            QuestFinished();
        }
    }
}
