using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour
{
    public enum GameStates
    {
        GamePlaying,
        GamePaused,
        GameWon,
        GameLost,
        GameIntermission
    };

    public GameStates CurrentState { get; private set; }
    
    [Header("References")]
    public GameView gameView;
    public LevelManager levelManager;
    public PlayerController playerController;
    public EnemySpawner enemySpawner;
    public SimpleTimer simpleTimer;

    public void ChangeState(GameStates newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case GameStates.GamePlaying:
                OnGamePlaying();
                break;
            case GameStates.GamePaused:
                OnGamePaused();
                break;
            case GameStates.GameWon:
                OnGameWon();
                break;
            case GameStates.GameLost:
                OnGameLost();
                break;
            case GameStates.GameIntermission:
                OnGameIntermission();
                break;
        }
    }

    private void OnGameWon()
    {
        gameView.ShowResults("WIN");
        enemySpawner.ClearEnemies();
        ChangeState(GameStates.GameIntermission);
    }

    private void OnGameLost()
    {
        gameView.ShowResults("LOSE");
        enemySpawner.ClearEnemies();
    }

    private void OnGamePaused()
    {
        CurrentState = GameStates.GamePaused;
        simpleTimer.PauseTimer();
    }

    private void OnGamePlaying()
    {
        CurrentState = GameStates.GamePlaying;   
        simpleTimer.ResumeTimer();
    }

    private void OnGameIntermission()
    {
        CurrentState = GameStates.GameIntermission;
        simpleTimer.ResetTimer();
        StartCoroutine(StartNextLevel());
    }

    IEnumerator StartNextLevel()
    {
        yield return new WaitForSeconds(3f);
        levelManager.SetCurrentLevel(levelManager.GetCurrentLevel() + 1);
    }

    public void PauseGame()
    {
        ChangeState(GameStates.GamePaused);
    }

    public void ResumeGame()
    {
        ChangeState(GameStates.GamePlaying);
    }
    
    public bool IsGameWon()
    {
        return CurrentState == GameStates.GameWon;
    }

    public bool IsGameOver()
    {
        return CurrentState == GameStates.GameLost;
    }


    public bool IsGamePlaying()
    {
        return CurrentState == GameStates.GamePlaying;
    }

    public bool IsGamePaused()
    {
        return CurrentState == GameStates.GamePaused;
    }
}
