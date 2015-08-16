using UnityEngine;
using System.Collections;

public class Totem : MonoBehaviour {

    public bool bonusTotem = false;

    public Color activatedColor = Color.red;

    public float activatedTime = 360.0f;
    public float activatingTime = 5.0f;

    public float activationRange = 10.0f;

    public bool showIndicator = false;

    private bool active = false;
    private float activationState = 0.0f;
    private float activeTime = 0.0f;

    private Transform player;
    private PrefabManager prefabs;

    private bool activatable = false;

    private Color baseColor;

	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        prefabs = GameObject.FindGameObjectWithTag("Prefabs").GetComponent<PrefabManager>();
        baseColor = GetComponent<MeshRenderer>().material.color;

        if (showIndicator)
        {
            GameObject marker = Instantiate(prefabs.rangeIndicator, transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
            marker.transform.parent = transform;
            marker.GetComponent<Projector>().orthographicSize = activationRange;
            marker.GetComponentInChildren<ParticleSystem>().Stop();
        }
	}
	
	void Update () {
        if (active && !bonusTotem)
        {
            DrainTime();
        }
        else
        {
            if (!bonusTotem || activatable)
            {
                CheckForActivation();
            }
        }

        MakeColor();
	}

    void MakeColor()
    {
        if (active)
        {
            GetComponent<MeshRenderer>().material.color = Color.Lerp(baseColor, activatedColor, getPct());
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.Lerp(baseColor, activatedColor, activationState / activatingTime);
        }
    }

    void DrainTime()
    {
        activeTime -= Time.deltaTime;
        Mathf.Clamp(activeTime, 0.0f, activatedTime);

        if (activeTime <= 0.0f)
        {
            active = false;

            OnDeactivation();
        }
    }

    void CheckForActivation()
    {
        if (Vector3.Distance(player.position, transform.position) <= activationRange)
        {
            activationState += Time.deltaTime;
        }
        else if (activationState > 0.0f)
        {
            activationState -= Time.deltaTime;
        }

        Mathf.Clamp(activationState, 0.0f, activatingTime);

        if (activationState >= activatingTime)
        {
            active = true;
            activationState = 0.0f;
            activeTime = activatedTime;

            OnActivation();
        }
    }

    public bool IsActive()
    {
        return active;
    }

    public void SetActivatable(bool b)
    {
        activatable = b;
    }

    public void Deactivate()
    {
        active = false;
        OnDeactivation();
    }

    public float getPct()
    {
        return activeTime / activatedTime;
    }
    
    protected virtual void OnActivation()
    {

    }

    protected virtual void OnDeactivation()
    {

    }
}
