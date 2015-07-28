using UnityEngine;
using System.Collections;

public class MenueSelector : MonoBehaviour {
    public GameObject loadingImage;
    
        
    public void LoadScene(int sceneNumber)
    {
        loadingImage.SetActive(true);
        Application.LoadLevel(sceneNumber);
    }
}
