using UnityEngine;
using System.Collections;

public class TurnCompass : MonoBehaviour
{

    public Transform goal;
    public Transform playerTransform;

    public bool onlyYRotation;

    private Vector3 facing;
    private Vector3 target;
    private Vector3 player;

    public Transform pointer;

    // Use this for initialization
    void Start()
    {
        target = goal.position;
        facing = new Vector3(0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // Refresh position values
        player = playerTransform.position;
        target = goal.position;

        // Vector between target and player
        facing = target - player;
        facing.Normalize();

        // Angle between calculated arrow and the original facing of the arrow
        float angle = Mathf.Acos(Vector3.Dot(facing, new Vector3(0, 0, 1)));

        // Convert angle from radians to degree
        angle = Mathf.Rad2Deg * angle;

        // Get Axis of the rotation
        Vector3 axis = Vector3.Cross(new Vector3(0, 0, 1), facing);

        Quaternion quat;

        if (axis.magnitude < 0.01f)
        {
            quat = Quaternion.identity;
        }
        else
        {
            axis.Normalize();
            quat = Quaternion.AngleAxis(angle, axis);
        }

        // Get player's rotation
        Quaternion playerRotation = playerTransform.rotation;

        // If x/z rotation should be ignored, remove those parameters
        if (onlyYRotation)
        {
            quat.x = 0;
            quat.z = 0;

            playerRotation.x = 0;
            playerRotation.z = 0;
        }

        // Apply rotation multiplied by the player's rotation
        pointer.rotation = quat * Quaternion.Inverse(playerTransform.rotation);
        //pointer.Rotate(new Vector3(0, 0, 1), angle);
        //pointer.eulerAngles = new Vector3(0, 0, quat.z);
    }
}

/*
 * using UnityEngine;
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
        angleZ = (angleZ / Mathf.PI * 180) - 90 + player.transform.eulerAngles.y;
        vector = new Vector3(0, 0, angleZ);
    }
}
*/