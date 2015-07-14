using UnityEngine;
using System.Collections;

/*
 * HELPING SOURCE:
 * https://www.youtube.com/watch?v=zM6L5WJOsWg
*/

public class BobCamera : MonoBehaviour
{

    private Animation anim;
    public bool isSprinting = true;

    private bool animationSwitch = true;


    void Start()
    {
        anim = GetComponent<Animation>();
    }


    void Update()
    {
        if (isSprinting == true)
        {
            if (!anim.isPlaying)
            {
                if (animationSwitch)
                {
                    anim.Play("BobCameraLeft");
                    animationSwitch = false;
                }
                else
                {
                    anim.Play("BobCameraRight");
                    animationSwitch = true;
                }
            }
        }
    }
}
