using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GameController : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private SimpleTimer simpleTimer;
    [SerializeField] private GameView gameView;

    void Awake()
    {
        gameStateManager.ChangeState(GameStateManager.GameStates.GamePlaying);
        levelManager.StartLevel(1);
    }

    public void GameLost()
    {
        gameStateManager.ChangeState(GameStateManager.GameStates.GameLost);
    }

    public void GameWon()
    {
        gameStateManager.ChangeState(GameStateManager.GameStates.GameWon);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
