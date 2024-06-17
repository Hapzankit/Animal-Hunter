using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Loading : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadingImage;

    public void loadscene(int Index)
    {
        StartCoroutine(LoadLevelAsync(Index));
    }

    private IEnumerator LoadLevelAsync(int index)
    {
       // Scene scene =SceneManager.GetSceneByBuildIndex(index);
        UnityEngine.AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        operation.allowSceneActivation = false;

        loadingScreen.SetActive(true);

        // Simulated progress value
        float simulatedProgress = Random.Range(0.15f, 0.85f);
        float simulatedProgressTime = Random.Range(0.7f, 0.9f);

        loadingImage.DOFillAmount(simulatedProgress, simulatedProgressTime);

        while (true)
        {
            Debug.Log("Check for the scene load");
            // Check if the loading is almost complete
            if (operation.progress >= 0.9f)
            {
                Debug.Log("scene loaded activating it now");
                float randomWait = Random.Range(3, 8f);
                yield return new WaitForSeconds(randomWait);
                loadingImage.DOFillAmount(1, 0.2f).OnComplete(() => operation.allowSceneActivation = true);
                break;
            }

            yield return null;
        }
    }


    
}
