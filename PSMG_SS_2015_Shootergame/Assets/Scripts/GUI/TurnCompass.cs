using UnityEngine;
using System.Collections;

public class TurnCompass : MonoBehaviour
{
    //the goal the compass is pointing at
    public Transform goal;

    //the player
    private Transform playerTransform;

    //Vector of the viewing direction of the player
    private Vector3 facing;
    //The position vector of the goal
    private Vector3 target;
    //the position vector of the player
    private Vector3 player;

    //the pointer of the compass
    public Transform pointer;

    // Use this for initialization
    void Start()
    {
        facing = new Vector3(0, 0, 1);

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    //calculation of the angle and rotation of the pointer
    void Update()
    {
        if (goal != null)
        {
            target = goal.position - playerTransform.position;
            target.y = 0.0f;

            player = playerTransform.forward;
            player.y = 0.0f;

            float angle = Vector3.Angle(player, target);

            Vector3 cross = Vector3.Cross(player, target);

            if (cross.y > 0)
            {
                angle = -angle;
            }

            pointer.rotation = Quaternion.AngleAxis(angle, facing);
        }
    }

    //method to set the goal
    public void SetGoal(Transform t)
    {
        goal = t;
    }
}