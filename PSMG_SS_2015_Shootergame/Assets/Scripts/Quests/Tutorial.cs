using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{

    public GameObject rangeIndicatorProjector;

    private Transform player;

    public float distance = 20.0f;

    private bool activated = false;
    private bool questStarted = false;
    private bool questFinished = false;
    
    public ShowTutorialText textScript;

    public bool activateOnStart = false;

    public GameObject nextQuest;

    public string startText;
    public string finishText;

    public float startTextTime;
    public float finishTextTime;

    private GameObject indicator;

    // Use this for initialization
    void Start()
    {
        GetPlayer();

        if (activateOnStart)
        {
            ActivateQuest();
        }
    }

    void GetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (activated)
        {
            if (!questStarted)
            {
                StartQuest();
            }
            else
            {
                CheckFinish();
            }
        }
    }
    
    GameObject CreateIndicator(Transform parent, float range)
    {
        GameObject indicator = Instantiate(rangeIndicatorProjector, parent.position, Quaternion.Euler(90, 0, 0)) as GameObject;
        indicator.transform.parent = parent;
        indicator.GetComponent<Projector>().orthographicSize = range;

        return indicator;
    }
    
    void CheckFinish()
    {
        float d = Vector3.Distance(player.position, transform.position);
        if (d <= distance)
        {
            QuestFinished();
        }
    }

    void StartQuest()
    {
        questStarted = true;

        textScript.showTextinUI(startText, startTextTime, 14);

        Debug.Log("starting!");
        indicator = CreateIndicator(transform, distance);
    }

    void QuestFinished()
    {
        activated = false;
        questStarted = false;
        questFinished = true;

        textScript.showTextinUI(finishText, finishTextTime);

        Destroy(indicator);

        ActivateNextQuest();
    }

    void ActivateNextQuest()
    {
        if (nextQuest != null)
        {
            if (nextQuest.GetComponent<QuestFollow>() != null)
            {
                nextQuest.GetComponent<QuestFollow>().ActivateQuest();
            }
            else if (nextQuest.GetComponent<QuestWaypoint>() != null)
            {
                nextQuest.GetComponent<QuestWaypoint>().ActivateQuest();
            }
            else if (nextQuest.GetComponent<Tutorial>() != null)
            {
                nextQuest.GetComponent<Tutorial>().ActivateQuest();
            }
        }
    }
    
    public void ActivateQuest()
    {
        activated = true;
        GameObject.FindGameObjectWithTag("Ghost").GetComponent<TutorialGhost>().SetGoal(transform.position);
    }

    public void ActivateQuest(float delay)
    {
        Invoke("ActivateQuest", delay);
    }
}
