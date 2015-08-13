using UnityEngine;
using System.Collections;

public class TurnCompass : MonoBehaviour
{

    public Transform goal;

    private Transform playerTransform;

    private Vector3 facing;
    private Vector3 target;
    private Vector3 player;

    public Transform pointer;

    // Use this for initialization
    void Start()
    {
        facing = new Vector3(0, 0, 1);

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (goal != null)
        {
            target = goal.position - playerTransform.position;
            player = playerTransform.forward;

            float angle = Vector3.Angle(player, target);

            Vector3 cross = Vector3.Cross(player, target);

            if (cross.y > 0)
            {
                angle = -angle;
            }

            pointer.rotation = Quaternion.AngleAxis(angle, facing);
        }
    }

    public void SetGoal(Transform t)
    {
        goal = t;
    }
}