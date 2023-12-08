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
    public Text levelText;

    [Header("References")]
    public CanvasGroup LevelUpView;
    public CanvasGroup ResultsView;
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
        SetKillsText(playerController.GetKills());
        SetHealthText((int)Math.Round(playerController.GetHealth()));
        SetPlayerLevelText(playerController.GetPlayerLevel());
    }

    public void ShowResults(string result)
    {
        resultText.text = result;
        ShowCanvasGroup(ResultsView);
    }

    public void HideResults()
    {
        HideCanvasGroup(ResultsView);
    }

    public void ShowLevelUpView()
    {
        ShowCanvasGroup(LevelUpView);
    }

    public void HideLevelUpView()
    {
        HideCanvasGroup(LevelUpView);
    }
   
   public void SetLevelText(int count)
    {
        levelText.text = "Level: " + count;
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
