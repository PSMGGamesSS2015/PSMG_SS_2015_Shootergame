using UnityEngine;
using System.Collections;

public class TotemQuest : MonoBehaviour {

    public Transform[] totems;

    public float activateDistance = 5.0f;
    public float activateTime = 600.0f;

    public float speedBonus = 1.2f;

    private ArrayList inactive;
    private ArrayList active;

    private Transform player;

    private PlayerMovement movement;

	// Use this for initialization
	void Awake () {
        SetPlayer();
        SetTotems();
	}
	
	// Update is called once per frame
	void Update () {
	    CheckActivation();
	}

    void SetPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void SetTotems()
    {
        inactive = new ArrayList();
        active = new ArrayList();

        for (int i = 0; i < totems.Length; i++)
        {
            inactive.Add(totems[i]);
        }
    }

    void CheckActivation()
    {
        for (int i = 0; i < inactive.Count; i++)
        {
            float distance = Vector3.Distance(player.position, ((Transform) inactive[i]).position);

            if (distance <= activateDistance)
            {
                TotemActivated(i);
            }
        }
    }

    void CheckBuff()
    {
        if (inactive.Count == 0)
        {
            ActivateBuff();
        }
    }

    void TotemActivated(int index)
    {
        active.Add(inactive[index]);
        inactive.RemoveAt(index);

        Invoke("DeactivateTotem", activateTime);

        CheckBuff();
    }

    void DeactivateTotem()
    {
        inactive.Add(active[0]);
        active.RemoveAt(0);

        DisableBuff();
    }

    void ActivateBuff()
    {
        movement.IncreaseSpeed(speedBonus);
    }

    void DisableBuff()
    {
        movement.ResetSpeed();
    }
}
