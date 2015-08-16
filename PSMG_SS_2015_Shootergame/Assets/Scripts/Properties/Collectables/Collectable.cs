using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour {

    private bool collected = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            collected = true;
            gameObject.SetActive(false);
            OnCollect(other);
        }
    }

    public bool IsCollected()
    {
        return collected;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    protected virtual void OnCollect(Collider player)
    {

    }
}
