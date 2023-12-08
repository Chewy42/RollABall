using System;
using UnityEngine;
using UnityEngine.UI;

public class SimpleTimer : MonoBehaviour
{
    public int timeLimit = 60;
    [SerializeField] private float _currentTime;
    private bool isTimerActive = true;
    public Text timerText;

    private void Start()
    {
        _currentTime = 0f;
    }

    private void Update()
    {
        if (isTimerActive)
        {
            _currentTime += Time.deltaTime;
            if (_currentTime >= 60)
            {
                _currentTime = 60;
                isTimerActive = false;
            }

            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(_currentTime);
        timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    public bool IsTimeUp()
    {
        return _currentTime >= timeLimit;
    }

    public void PauseTimer()
    {
        isTimerActive = false;
    }

    public void ResumeTimer()
    {
        isTimerActive = true;
    }

    public float GetCurrentTime()
    {
        return _currentTime;
    }

    public void ResetTimer()
    {
        _currentTime = 0f;
    }
}
