using UnityEngine;
using System.Collections;

public class QuestWaypoint : MonoBehaviour {

    public Transform[] waypoints;

    public float waypointSize = 3.0f;

    public bool inOrder;

    public enum TimeMode { None = 0, BetweenWaypoints = 1, Total = 2 }
    public TimeMode timeMode = TimeMode.None;

    public float timeLimitSeconds = 0.0f;

    public enum MarkMode { None = 0, Next = 1, All = 2 }
    public MarkMode markMode = MarkMode.None;

    public GameObject rangeIndicatorProjector;

    public SetUIVisualisation textScript;

    public string activateText;
    public string startText;
    public string finishText;
    public string failText;

    public float startTextTime;
    public float activateTextTime;
    public float finishTextTime;
    public float failTextTime;

    private Transform player;

    private ArrayList toVisit;
    private ArrayList visited;

    private float startTime;
    private float timeLimit;

    private bool activated = false;
    private bool questStarted = false;
    private bool questFinished = false;

    

	// Use this for initialization
	void Start () {
        GetPlayer();
        ActivateQuest();        
	}

    void GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void StartQuest()
    {
        questStarted = true;
        textScript.showTextinUI(startText, startTextTime);

        startTime = Time.time;
    }

    void SetWaypoints()
    {
        toVisit = new ArrayList();
        visited = new ArrayList();

        for (int i = 0; i < waypoints.Length; i++)
        {
            toVisit.Add(waypoints[i]);
        }
    }

    void MarkWaypoints()
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

    void CreateMarker(Transform waypoint)
    {
        GameObject indicator = Instantiate(rangeIndicatorProjector, waypoint.position, Quaternion.Euler(90, 0, 0)) as GameObject;
        indicator.transform.parent = waypoint;
        indicator.GetComponent<Projector>().orthographicSize = waypointSize;
    }

	void Update () {
        if (activated)
        {
            CheckWaypoints();

            if (questStarted)
            {
                CheckFinish();
                CheckTime();
            }
        }        
	}

    void CheckTime()
    {
        if (timeMode == TimeMode.Total || timeMode == TimeMode.BetweenWaypoints)
        {
            if (Time.time - startTime >= timeLimitSeconds)
            {
                QuestFailed();
            }
        }
    }

    void CheckWaypoints()
    {
        if (inOrder)
        {
            float distance = Vector3.Distance(player.position, ((Transform)toVisit[0]).position);
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
                float distance = Vector3.Distance(player.position, ((Transform)toVisit[i]).position);

                if (distance <= waypointSize)
                {
                    WaypointVisited(i);
                }
            }
        }
    }

    void WaypointVisited(int index)
    {
        if (!questStarted)
        {
            StartQuest();
        }

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
    }

    void RenewMarkers()
    {
        if (markMode == MarkMode.Next && toVisit.Count >= 1)
        {
            CreateMarker((Transform)toVisit[0]);
        }
    }

    void CheckFinish()
    {
        if (toVisit.Count == 0)
        {
            QuestFinished();
        }
    }

    void QuestFinished()
    {
        textScript.showTextinUI(finishText, finishTextTime);

        activated = false;
        questStarted = false;
        questFinished = true;

        DestroyMarkers();

        ActivateQuest();

        Debug.Log("Quest complete!");
    }

    void QuestFailed()
    {
        textScript.showTextinUI(failText, failTextTime);

        activated = false;
        questStarted = false;
        questFinished = false;

        DestroyMarkers();

        ActivateQuest();

        Debug.Log("Quest failed!");
    }

    void DestroyMarkers() {
        for (int i = 0; i < toVisit.Count; i++)
        {
            if (((Transform)toVisit[i]).childCount != 0)
            {
                Destroy(((Transform)toVisit[i]).GetChild(0).gameObject);
            }
        }

        for (int i = 0; i < visited.Count; i++)
        {
            if (((Transform)visited[i]).childCount != 0)
            {
                Destroy(((Transform)visited[i]).GetChild(0).gameObject);
            }
        }   
    }

    public void ActivateQuest()
    {
        activated = true;
        textScript.showTextinUI(activateText, activateTextTime);

        questStarted = false;
        questFinished = false;

        SetWaypoints();
        MarkWaypoints();
    }
}
