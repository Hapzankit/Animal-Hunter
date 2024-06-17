using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor; // Required for Unity Events
using DG.Tweening;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] AnimalInfoSetUp animalInfoSetUp;

    public Action onAnimationComplete;

    private Loading loading; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        animalInfoSetUp.SetupStatsUI("Hippo");
    }

    
    
    public UnityEvent onPlay;
    public UnityEvent onOptions;
    public UnityEvent onExit;

    public Button PlayButton, ExitButton;

    
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void ExitGame()
    {
        
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

   
    public void OnButtonClicked(string action)
    {
        switch (action)
        {
            case "Play":
                onPlay?.Invoke();
                break;
            case "Options":
                onOptions?.Invoke();
                break;
            case "Exit":
                onExit?.Invoke();
                ExitGame();
                break;
            default:
                Debug.LogError("Action not found: " + action);
                break;
        }
    }

   
    public void PunchOnButtonClick(GameObject ButtonObject, Action taskAfterPunch)
    {
        
        RectTransform rectTransform = ButtonObject.GetComponent<RectTransform>();
        rectTransform.DOPunchScale(new Vector3(0.2f, 0.2f, 0), 0.3f, 10, 1).OnComplete(() =>
        {
            // Invoke the callback if it's set
            taskAfterPunch?.Invoke();
        });
    }

    void Start()
    {
        loading = GetComponent<Loading>();
        PlayButton.onClick.AddListener(() => Utils.PunchOnButtonClick(PlayButton.gameObject, () => loading.loadscene(1)));
        ExitButton.onClick.AddListener(() => Utils.PunchOnButtonClick(ExitButton.gameObject, () => ExitGame()));
    }
}
