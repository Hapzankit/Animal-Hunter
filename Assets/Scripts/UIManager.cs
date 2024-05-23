using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public TMP_Text taskText ;

    public static UIManager instance;

    public GameObject LevelClearScreen;

    public Button nextLevelButon;
    
    public GameObject loadingScreen;

    public TMP_Text levelClearText;

    public Image loadingImage;

    public GameObject gameOverScreen;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        taskText.text = "Kill a " + LevelManager.instance.targetAnimal;
    }

    public void SetLevelClearScreen(string LevelNames)
    {
        LevelClearScreen.SetActive(true);
        nextLevelButon.onClick.AddListener(() => LoadNextLevel(LevelNames));
    }

    private void LoadNextLevel(string LevelName)
    {
        StartCoroutine(LoadLevelAsync2(LevelName));
    }

    private IEnumerator LoadLevelAsync(string LevelNames)
    {
        UnityEngine.AsyncOperation operation = SceneManager.LoadSceneAsync(LevelNames);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingImage.fillAmount = progressValue;

            

            if (operation.progress >= 0.7f)
            {
                Debug.Log("Current Progress" + operation.progress);
                // Wait for the specified delay
                yield return new WaitForSeconds(5f);

                // Activate the scene
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        
    }

    private IEnumerator LoadLevelAsync2(string LevelName)
    {
        UnityEngine.AsyncOperation operation = SceneManager.LoadSceneAsync(LevelName);
        operation.allowSceneActivation = false;

        loadingScreen.SetActive(true);

        // Simulated progress value
        float simulatedProgress = Random.Range(0.15f, 0.85f);
        float simulatedProgressTime = Random.Range(0.5f, 0.9f);

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
