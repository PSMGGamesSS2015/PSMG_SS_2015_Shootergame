using UnityEngine;
using System.Collections;

public class TurnCompass : MonoBehaviour {
    public BasePlayer player;
    private Transform goal;
    public GameObject pointer;
    private bool first = true;
    private Vector3 vector;
    public Transform target;

	// Use this for initialization
	void Start () {
        setGoal(target);
	}
	
	// Update is called once per frame
	void OnGUI () {
        if (goal != null)
        {
            getDirection();
            if (vector.z != pointer.transform.eulerAngles.z)
            {
                pointer.transform.eulerAngles = vector;
            }
        }
	}

    public void setGoal(Transform target2)
    {
        goal = target2;
    }

    private void getDirection()
    {
        float angleZ = Mathf.Atan2(goal.transform.position.z, goal.transform.position.x) - Mathf.Atan2(player.transform.position.z, player.transform.position.x);
        angleZ = (angleZ / Mathf.PI * 180) + player.transform.eulerAngles.y;
        vector = new Vector3(0, 0, angleZ);
    }
}
