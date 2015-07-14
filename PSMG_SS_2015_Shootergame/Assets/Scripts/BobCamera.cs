using UnityEngine;
using System.Collections;

/*
 * SOURCE:
 * https://www.youtube.com/watch?v=zM6L5WJOsWg
*/

public class BobCamera : MonoBehaviour
{

    private Animation anim; //Empty GameObject's animation component
    public bool isMoving = true;

    private bool left;
    private bool right;

    void CameraAnimations()
    {
        if (isMoving == true)
        {
            if (left == true)
            {
                if (!anim.isPlaying)
                {//Waits until no animation is playing to play the next
                    anim.Play("BobCameraLeft");
                    left = false;
                    right = true;
                }
            }
            if (right == true)
            {
                if (!anim.isPlaying)
                {
                    anim.Play("BobCameraRight");
                    right = false;
                    left = true;
                }
            }
        }
    }


    void Start()
    { //First step in a new scene/life/etc. will be "walkLeft"
        left = true;
        right = false;

        anim = GetComponent<Animation>();
    }


    void Update()
    {
        CameraAnimations();
    }
}
