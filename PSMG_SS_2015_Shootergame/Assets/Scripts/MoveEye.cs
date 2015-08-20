using UnityEngine;
using System.Collections;

public class MoveEye : MonoBehaviour {
    private float movement;
    private bool animationReady = true;
    private Vector3 startingPosition;
    private Vector3 movementVector;
    //The time taken to move from the start to finish positions
    public float timeTakenDuringLerp = 1f;

    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;
    float percentageComplete;

	// Use this for initialization
	void Start () {
        startingPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (animationReady)
        {
            Debug.Log("start");
            movement = Random.Range(2f, 8f);
            movementVector = startingPosition - new Vector3(movement, 0, 0);
            _timeStartedLerping = Time.time;
            animationReady = false;
        } 
        else
        {
            Debug.Log("lerp");
            float timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / timeTakenDuringLerp;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, movementVector, percentageComplete);
        }
        if (percentageComplete > 0.99f) //movementVector == gameObject.transform.position)
        {
            Debug.Log("finished");
            percentageComplete = 0;
            animationReady = true;
        }
	}
}
