using System;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [Header("UI Elements")]
    public Text resultText;
    public Text timerText;
    public Text killsText;
    public Text healthText;
    public Text playerLevelText;

    [Header("References")]
    public CanvasGroup LevelUpView;
    public PlayerController playerController;
    public GameController gameController;

    private void Start()
    {
        resultText.text = "";
        killsText.text = "Kills: 0";
        healthText.text = "Health: " + playerController.GetHealth();
        playerLevelText.text = "Level: 0";
    }

    void Update()
    {
        if (gameController.IsGameOver())
        {
            resultText.text = "Game Over!";
        }
        else if (gameController.IsGameWon())
        {
            resultText.text = "You Win!";
        }
        else
        {
            resultText.text = "";
        }

        SetKillsText(playerController.GetKills());
        SetHealthText((int)Math.Round(playerController.GetHealth()));
        SetPlayerLevelText(playerController.GetPlayerLevel());
    }

    public void ShowLevelUpView()
    {
        ShowCanvasGroup(LevelUpView);
    }

    public void HideLevelUpView()
    {
        HideCanvasGroup(LevelUpView);
    }
   

    public void SetTimerText(int count)
    {
        timerText.text = "Time: " + count;
    }

    public void SetKillsText(int count)
    {
        killsText.text = "Kills: " + count;
    }

    public void SetHealthText(int count)
    {
        healthText.text = "Health: " + count;
    }

    public void SetPlayerLevelText(int count)
    {
        playerLevelText.text = "Level: " + count;
    }

    private void HideCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

}
