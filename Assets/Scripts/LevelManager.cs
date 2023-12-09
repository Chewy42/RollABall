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

    private int _currentLevel = 1;

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
    List<GameObject> prefabs = new List<GameObject>(currentLevel.GetPrefabs());
    int maxBosses = currentLevel.GetMaxBosses();
    float spawnRate = enemySpawner.GetSpawnRate();

    float levelDuration = 60f;
    float startTime = Time.time;

    while (Time.time - startTime < levelDuration)
    {
        int randomIndex = Random.Range(0, prefabs.Count);
        GameObject prefab = prefabs[randomIndex];

        enemySpawner.SpawnEnemy(prefab);

        if (bossSpawnTimes.Count > 0 && Time.time - startTime >= bossSpawnTimes[0])
        {
            enemySpawner.SpawnBoss(false); 
            bossSpawnTimes.RemoveAt(0);
        }

        yield return new WaitForSeconds(spawnRate);
    }

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
        Enemy_Boss boss = enemySpawner.SpawnBoss(true); 
        if (boss == null) yield break;

        while (boss.IsAlive())
        {
            yield return null; 
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

    public bool IsLastLevel()
    {
        return _currentLevel == levels.Count;
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
