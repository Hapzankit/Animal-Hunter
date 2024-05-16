using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEditor; // Required for Unity Events

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    }

    // Unity Events for designer customization
    public UnityEvent onPlay;
    public UnityEvent onOptions;
    public UnityEvent onExit;

    // Alternatively, load scenes by index
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void ExitGame()
    {
        // Compile-time directive for Unity Editor
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    // Designer-friendly button click handler
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
}