using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    public TMP_Text taskText ;

    public TMP_Text levelClearText;

    public TMP_Text AmmoOverText;

    public TMP_Text timerText;
    
    public static UIManager instance;

    public GameObject LevelClearScreen;

    public Button nextLevelButon;
    
    public GameObject loadingScreen;

    public Image loadingImage;

    public GameObject gameOverScreen;


    [SerializeField] TMP_Text bulletRemainText, bulletReloadReamainAmountText;
    

    public GameObject GameWonButton;


    public LevelTimer levelTimer;

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

        //Stopping the level timer
        levelTimer.timerIsRunning = false;
    }
    
    public void GameWonScreen()
    {
        LevelClearScreen.SetActive(true);
        nextLevelButon.gameObject.SetActive(false);
        GameWonButton.SetActive(true);
    }

    private void LoadNextLevel(string LevelName)
    {
        StartCoroutine(LoadLevelAsync(LevelName));
    }

    private IEnumerator LoadLevelAsync(string LevelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(LevelName);
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

    public void ShowRemainingBullets(int remainingBullets, int totalBullets, int reloadAmount, int totalRemainAmountBullet)
    {
        bulletRemainText.text = remainingBullets.ToString() + "/" + totalBullets.ToString();
        bulletReloadReamainAmountText.text = reloadAmount.ToString() + "/" + totalRemainAmountBullet.ToString();
    }

    public void GameOver() 
    {
        gameOverScreen.SetActive(true);
       
    }

    public void UsePunchAnimation(GameObject AnyObject)
    {
        Utils.PunchOnButtonClick(AnyObject);
    }

}
