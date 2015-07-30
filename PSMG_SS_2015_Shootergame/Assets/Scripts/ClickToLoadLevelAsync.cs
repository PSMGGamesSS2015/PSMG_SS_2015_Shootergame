using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClickToLoadLevelAsync : MonoBehaviour
{
    public Slider loadingBar;
    public GameObject loadingImage;

    private AsyncOperation async;

    public void ClickAsync(int level)
    {
        loadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(level));
    }

    IEnumerator LoadLevelWithBar(int level)
    {
        async = Application.LoadLevelAsync(level);
        while (!async.isDone)
        {
            //Debug.Log(async.progress);
            loadingBar.value = async.progress;
            //Debug.Log(loadingBar.value);
            yield return null;
        }
    }
}
