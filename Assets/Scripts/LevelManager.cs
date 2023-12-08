using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private SimpleTimer simpleTimer;
    [SerializeField] private GameView gameView;
    [SerializeField] private PlayerController playerController;

    public List<BaseLevel> levels;

    private int _currentLevel = 0;

    public void StartLevel(int level)
    {
        StartCoroutine(WaitForPlayer());
        StartCoroutine(LevelRoutine(level));
    }

private IEnumerator LevelRoutine(int level)
{
    BaseLevel currentLevel = levels[level - 1];
    ChangeEnemySpawnRate(level);

    List<float> bossSpawnTimes = new List<float>(currentLevel.GetBossSpawnTimes());
    int maxBosses = currentLevel.GetMaxBosses();
    float spawnRate = enemySpawner.GetSpawnRate();

    float levelDuration = 60f; // Duration of the level in seconds
    float startTime = Time.time;

    while (Time.time - startTime < levelDuration)
    {
        enemySpawner.SpawnEnemy();

        // Spawn regular bosses at specified times before the final fight
        if (bossSpawnTimes.Count > 0 && Time.time - startTime >= bossSpawnTimes[0])
        {
            enemySpawner.SpawnBoss(false); // false indicates a regular boss
            bossSpawnTimes.RemoveAt(0);
        }

        yield return new WaitForSeconds(spawnRate);
    }

    // Start the boss fight exactly at 60 seconds
    StartCoroutine(StartBossFight(maxBosses));
}


    private void ChangeEnemySpawnRate(int level)
    {
        foreach (BaseLevel baseLevel in levels)
        {
            if (baseLevel.GetLevel() == level)
            {
                enemySpawner.SetSpawnRate(baseLevel.GetEnemySpawnRate());
                break;
            }
        }
    }


    private IEnumerator StartBossFight(int maxBosses)
{
    int defeatedBosses = 0;

    while (defeatedBosses < maxBosses)
    {
        Enemy_Boss boss = enemySpawner.SpawnBoss(true); // true indicates a final boss
        if (boss == null) yield break;

        // Continue the game loop until the boss is defeated
        while (boss.IsAlive())
        {
            yield return null; // Wait until the boss is defeated
        }

        defeatedBosses++;
    }

    WinGame();
}

    private void WinGame()
    {
        gameStateManager.ChangeState(GameStateManager.GameStates.GameWon);
    }

     IEnumerator WaitForPlayer()
    {
        while (playerController == null)
        {
            yield return null;
        }
    }

    public void StartLevelUp()
    {
        gameView.ShowLevelUpView();
        simpleTimer.PauseTimer();
    }

    public void SetCurrentLevel(int level)
    {
        _currentLevel = level;
        PrepareNextLevel();
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    private void PrepareNextLevel()
    {
        ResetLevel();
        StartLevel(_currentLevel);
        playerController.StartCoroutine(playerController.AutoShoot());
    }

    private void ResetLevel()
    {
        enemySpawner.ClearEnemies();
        gameView.SetLevelText(_currentLevel);
        gameView.HideResults();
        simpleTimer.ResetTimer();
        gameStateManager.ResumeGame();
        playerController.SetCanShoot(true);
    }

}
