﻿using UnityEngine;
using System.Collections;

public class QuestWaypoint : Quest
{
    public Transform[] waypoints;

    public float waypointSize = 3.0f;

    public enum MarkMode { None = 0, Next = 1, All = 2 }
    public MarkMode markMode = MarkMode.None;

    public bool inOrder = false;

    public enum TimeMode { None = 0, BetweenWaypoints = 1, Total = 2 }
    public TimeMode timeMode = TimeMode.None;

    public float timeLimitSeconds = 0.0f;

    private float timeLimit;

    public bool screamingAllowed = false;

    private ArrayList toVisit;
    private ArrayList visited;

    private EnviromentSound audioController;

    new void Start()
    {
        base.Start();
        audioController = GameObject.FindGameObjectWithTag("PlayerSound").GetComponent<EnviromentSound>();
    }

    protected override void OnUpdate()
    {
        CheckWaypoints();
    }

    private void CheckWaypoints()
    {
        Vector3 playerPosition = player.transform.position;

        if (inOrder)
        {
            float distance = Vector3.Distance(playerPosition, ((Transform)toVisit[0]).position);
            if (distance <= waypointSize)
            {
                WaypointVisited(0);
                RenewMarkers();
            }
        }
        else
        {
            for (int i = 0; i < toVisit.Count; i++)
            {
                float distance = Vector3.Distance(playerPosition, ((Transform)toVisit[i]).position);

                if (distance <= waypointSize)
                {
                    WaypointVisited(i);
                }
            }
        }
    }

    private void RenewMarkers()
    {
        if (markMode == MarkMode.Next && toVisit.Count >= 1)
        {
            CreateMarker((Transform)toVisit[0], waypointSize);
        }
    }

    private void WaypointVisited(int index)
    {
        if (((Transform)toVisit[index]).childCount != 0)
        {
            Destroy(((Transform)toVisit[index]).GetChild(0).gameObject);
        }

        if (timeMode == TimeMode.BetweenWaypoints)
        {
            startTime = Time.time;
        }

        visited.Add(toVisit[index]);
        toVisit.RemoveAt(index);

        if (toVisit.Count > 0)
        {
            SetGoal((Transform)toVisit[0]);
        }

        audioController.playReached();
    }

    protected override void CheckFinish()
    {
        if (toVisit.Count == 0)
        {
            QuestFinished();
        }
    }

    protected override void CheckFail()
    {
        if (timeMode == TimeMode.Total || timeMode == TimeMode.BetweenWaypoints)
        {
            if (Time.time - startTime >= timeLimitSeconds)
            {
                QuestFailed();
            }
        }
    }

    protected override void OnQuestActivated()
    {
        if (screamingAllowed)
        {
            Invoke("PlayScreamSound", 2F);
            Invoke("PlayScreamSound", 5F);
        }
    }

    private void PlayScreamSound()
    {
        audioController.playScream();
    }

    protected override void OnQuestStarted()
    {
        if (screamingAllowed)
        {
            PlayScreamSound();
            Invoke("PlayScreamSound", 3F);
            Invoke("PlayScreamSound", 5F);
        }
        SetWaypoints();
        MarkWaypoints();
    }

    private void SetWaypoints()
    {

        toVisit = new ArrayList();
        visited = new ArrayList();

        for (int i = 0; i < waypoints.Length; i++)
        {
            toVisit.Add(waypoints[i]);
        }

        SetGoal((Transform)toVisit[0]);
    }

    private void MarkWaypoints()
    {
        if (markMode == MarkMode.Next)
        {
            CreateMarker((Transform)toVisit[0], waypointSize);
        }
        else if (markMode == MarkMode.All)
        {
            for (int i = 0; i < toVisit.Count; i++)
            {
                CreateMarker((Transform)toVisit[i], waypointSize);
            }
        }
    }
}
