
using System;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTimer : MonoBehaviour
{
    public int timeLimit = 60;
    public int startingTime;
    public GameController gameController;
    public Text timerText;
    private float _currentTime;
    private bool isTimerActive = true;



    private void Start()
    {
        _currentTime = startingTime;
    }

    private void Update()
    {
        if (!isTimerActive) return;

        _currentTime += Time.deltaTime;

        if (_currentTime >= timeLimit)
        {
            _currentTime = timeLimit;
            isTimerActive = false;
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(_currentTime);
        string timeText = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        timerText.text = timeText;

        gameController.UpdateGameTimer((int)_currentTime);
    }

    public float GetCurrentTime()
    {
        return _currentTime;
    }

    public void PauseTimer()
    {
        isTimerActive = false;
    }

    public void ResumeTimer()
    {
        isTimerActive = true;
    }
}
