using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float speed = 3f;

    Vector3 movement;
    Rigidbody playerRigidbody;

	void Awake () {
        playerRigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Move(h, v);
        Turn();
	}

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        Debug.Log(movement);
        movement = movement * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turn()
    {
        
    }
}
