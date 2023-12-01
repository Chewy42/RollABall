using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour
{
    public enum GameStates
    {
        GamePlaying,
        GamePaused,
        GameWon,
        GameLost
    };
    private GameStates gameState;

    [Header("Player")]
    [SerializeField] PlayerController playerController;

    [Header("Enemies")]
    [SerializeField] GameObject enemySpawnPointsGameObject;
    [SerializeField] List<Transform> enemySpawnPoints;
    [SerializeField] GameObject enemiesParent;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] float _enemySpawnRate = 2.5f;

    [Header("Game View")]
    [SerializeField] GameView gameView;

    [Header("Collectibles")]
    private int maxCollectiblesCount;

    [Header("Timer")]
    [SerializeField] SimpleTimer simpleTimer;


    void Awake()
    {
        HideSpawnPoints();
    }

    private void Start()
    {
        gameView = GetComponentInChildren<GameView>();
        gameState = GameStates.GamePlaying;
        maxCollectiblesCount = GameObject.FindGameObjectsWithTag("Pick Up").Length;
        InitializeEnemySpawnPoints();
        StartCoroutine(WaitForPlayerSpawn());
    }

    private void InitializeEnemySpawnPoints()
    {
        foreach (Transform child in enemySpawnPointsGameObject.transform)
        {
            enemySpawnPoints.Add(child);
        }
    }

    IEnumerator WaitForPlayerSpawn()
    {
        while (playerController == null)
        {
            yield return null;
        }
        StartCoroutine(GameLoop());
    }

    private void OnGameWon()
    {

        gameView.resultText.text = "You Win!";

        gameView.killsText.gameObject.SetActive(false);
        gameView.timerText.gameObject.SetActive(false);
    }

    private void OnGameLost()
    {
        gameView.resultText.text = "You Lose.";

        gameView.killsText.gameObject.SetActive(false);
        gameView.timerText.gameObject.SetActive(false);

        enemies = new List<GameObject>();
    }

    private void OnGamePaused()
    {
        gameState = GameStates.GamePaused;
        simpleTimer.PauseTimer();
    }

    private void OnGamePlaying()
    {
        gameState = GameStates.GamePlaying;   
        simpleTimer.ResumeTimer();
    }


    public void StateUpdate(GameStates newState)
    {
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
        }
    }

    public void InitializeLevelUp()
    {
        StateUpdate(GameStates.GamePaused);
        gameView.ShowLevelUpView();
    }

    IEnumerator GameLoop()
    {
        while (simpleTimer.GetCurrentTime() < 60f)
        {
            if (gameState == GameStates.GamePlaying)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(_enemySpawnRate);
        }
        if (simpleTimer.GetCurrentTime() == 60f)
        {
            SpawnBoss();
        }
    }

    private void SpawnEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
        Transform spawnPoint = enemySpawnPoints[randomIndex];
        Vector3 pos = spawnPoint.position;

        GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        enemies.Add(enemy);
        enemy.transform.parent = enemiesParent.transform;

    }

    private void SpawnBoss()
    {
        int randomIndex = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
        Transform spawnPoint = enemySpawnPoints[randomIndex];
        Vector3 pos = spawnPoint.position;
        pos = new Vector3(pos.x, pos.y + 2.5f, pos.z); 

        GameObject boss = Instantiate(bossPrefab, pos, Quaternion.identity);
        enemies.Add(boss);
        boss.transform.parent = enemiesParent.transform;

    }

    private void HideSpawnPoints()
    {
        foreach (Transform spawnPoint in enemySpawnPoints)
        {
            // make transparent
            spawnPoint.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void Lose()
    {
        StateUpdate(GameStates.GameLost);
    }

    public void UpdateGameTimer(int timerCount)
    {
        gameView.SetTimerText(timerCount);
    }

    public bool IsGameOver()
    {
        return gameState == GameStates.GameLost;
    }

    public bool IsGameWon()
    {
        return gameState == GameStates.GameWon;
    }

    public bool IsGamePlaying()
    {
        return gameState == GameStates.GamePlaying;
    }

    public bool IsGamePaused()
    {
        return gameState == GameStates.GamePaused;
    }

    public void PauseGame()
    {
        StateUpdate(GameStates.GamePaused);
    }

    public void ResumeGame()
    {
        StateUpdate(GameStates.GamePlaying);
    }

    public void RestartGame()
    {
        StateUpdate(GameStates.GamePlaying);
    }
}
