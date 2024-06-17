using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [Tooltip("Set the starting time in seconds")]
    public float startTime;
    
    private float timeRemaining;
    
    [Space]
    public bool timerIsRunning = false;


    private void Start()
    {
        timeRemaining = startTime;
    
        UpdateTimerText();
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerText();
                // Optionally, handle timer end event here
                //Debug.Log("Timer has ended!");
                //Gameover
                UIManager.instance.GameOver();
            }
        }

    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        UIManager.instance.timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }



}
