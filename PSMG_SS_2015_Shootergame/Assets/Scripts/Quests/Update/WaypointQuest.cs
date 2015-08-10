using UnityEngine;
using System.Collections;

public class WaypointQuest : Quest
{

    public Transform[] waypoints;
    private ArrayList toVisit;
    private ArrayList visited;

    // Use this for initialization
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void OnQuestActivated()
    {
        SetWaypoints();
        MarkWaypoints();
        toVisit = new ArrayList();
        visited = new ArrayList();

        for (int i = 0; i < waypoints.Length; i++)
        {
            toVisit.Add(waypoints[i]);
        }
    }

    private void SetWaypoints()
    {

    }

    private void MarkWaypoints()
    {
        if (markMode != MarkMode.None)
        {
            if (markMode == MarkMode.Next)
            {
                CreateMarker((Transform)toVisit[0]);
            }
            else if (markMode == MarkMode.All)
            {
                for (int i = 0; i < toVisit.Count; i++)
                {
                    CreateMarker((Transform)toVisit[i]);
                }
            }
        }
    }

}
