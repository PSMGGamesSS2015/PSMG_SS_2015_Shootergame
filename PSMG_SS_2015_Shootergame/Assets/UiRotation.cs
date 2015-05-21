using UnityEngine;
using System.Collections;

public class UiRotation : MonoBehaviour
{
    public PlayerMovement movement;

    public Transform redHealthBar;
    public Transform blueFlapBar;
    public Transform panel1;
    public Transform panel2;

    /// <summary>
    /// The time taken to move from the start to finish positions
    /// </summary>
    public float timeTakenDuringLerp = 1f;
 
    /// <summary>
    /// How far the object should move when 'space' is pressed
    /// </summary>
    public float distanceToMove = 10;
 
    //Whether we are currently interpolating or not
    private bool _isLerping;
 
    //The start and finish positions for the interpolation
    private Vector3 _startPositionRed;
    private Vector3 _endPositionRed;
    private Vector3 _startPositionBlue;
    private Vector3 _endPositionBlue;
 
    //The Time.time value when we started the interpolation
    private float _timeStartedLerping;

    private Vector3 birdVector;
    private Vector3 humanVector;

    void Start()
    {
        //flapObjects = GameObject.FindGameObjectsWithTag("LifeVisualisation Flap");
    }

    /// <summary>
    /// Called to begin the linear interpolation
    /// </summary>
    void StartLerping()
    {

        _isLerping = true;
        _timeStartedLerping = Time.time;
 
        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        _startPositionRed = redHealthBar.transform.position;
        _endPositionRed = redHealthBar.transform.position - new Vector3(75f, 60f, 0f);
        _startPositionBlue = blueFlapBar.transform.position;
        _endPositionBlue = blueFlapBar.transform.position + new Vector3(50f, 37f, 0f);
    }
 
    void Update()
    {
        //When the user hits the spacebar, we start lerping
        if(Input.GetKey(KeyCode.F10))
        {
            if (movement.getMode())
            {
                birdVector = new Vector3(0.3f, 0.3f, 0.3f);
                humanVector = new Vector3(2f, 2f, 2f);
            }
            else
            {
                birdVector = new Vector3(2f, 2f, 2f);
                humanVector = new Vector3(0.3f, 0.3f, 0.3f);
            }

            StartLerping();
        }
    }
 
    //We do the actual interpolation in FixedUpdate(), since we're dealing with a rigidbody
    void FixedUpdate()
    {
        if(_isLerping)
        {
            //We want percentage = 0.0 when Time.time = _timeStartedLerping
            //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
            //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
            //"Time.time - _timeStartedLerping" is.
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;
 
            //Perform the actual lerping.  Notice that the first two parameters will always be the same
            //throughout a single lerp-processs (ie. they won't change until we hit the space-bar again
            //to start another lerp)
            //panel1.transform.position = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);
            redHealthBar.transform.position = Vector3.Lerp(_startPositionRed, _endPositionRed, percentageComplete);
            redHealthBar.transform.localScale = Vector3.Lerp(redHealthBar.transform.localScale, new Vector3(5f, 5f, 5f), Time.deltaTime);
            blueFlapBar.transform.position = Vector3.Lerp(_startPositionBlue, _endPositionBlue, percentageComplete);
            blueFlapBar.transform.localScale = Vector3.Lerp(blueFlapBar.transform.localScale, new Vector3(10.0f, 10.0f, 10.0f), Time.deltaTime);
            panel1.transform.localScale = Vector3.Lerp(panel1.transform.localScale, humanVector, Time.deltaTime);
            panel2.transform.localScale = Vector3.Lerp(panel2.transform.localScale, birdVector, Time.deltaTime);
            
            //When we've completed the lerp, we set _isLerping to false
            if(percentageComplete >= 1.0f)
            {
                _isLerping = false;
            }
        }
    }
}