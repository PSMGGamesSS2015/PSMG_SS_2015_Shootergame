using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour {

    private bool collected = false;
    private EnviromentSound audioController;

    void OnTriggerEnter(Collider other)
    {
        audioController = GameObject.FindGameObjectWithTag("PlayerSound").GetComponent<EnviromentSound>();
        if (other.gameObject.tag == "Player")
        {
            audioController.playCollected();
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
