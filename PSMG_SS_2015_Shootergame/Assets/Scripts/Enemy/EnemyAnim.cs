using UnityEngine;
using System.Collections;

public class EnemyAnim : MonoBehaviour {

    private float speed;
    private Animator anim;
    private Vector3 prev;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        speed = ((transform.position - prev).magnitude) / Time.deltaTime;
        prev = transform.position;
        anim.SetFloat("Speed", speed);
	}
}
