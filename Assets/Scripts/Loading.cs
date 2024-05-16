using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingImage.fillAmount = progressValue;

            yield return null;
        }
    }
}
