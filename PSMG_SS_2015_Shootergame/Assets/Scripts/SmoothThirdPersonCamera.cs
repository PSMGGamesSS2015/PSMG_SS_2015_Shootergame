using UnityEngine;
using System.Collections;

public class SmoothThirdPersonCamera : MonoBehaviour {

    public Transform target;

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 wantedPosition = target.TransformPoint(0, 5, -15);

        wantedPosition = RotatePointAroundPivot(wantedPosition, target.position, transform.localEulerAngles);
        transform.position = Vector3.SmoothDamp(transform.position, wantedPosition, ref velocity, 0.4f);

        transform.LookAt(target);
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

}
