using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private List<Transform> enemySpawnPoints;
    [SerializeField] private GameObject enemySpawnPointsGameObject;
    [SerializeField] private GameObject enemiesParent;
    [SerializeField] private float spawnRate;
    [SerializeField] private List<GameObject> enemies;
    private int _boss_count = 0;
    private bool _spawnerReady = false;

    private void Start()
    {
        InitializeEnemySpawnPoints();
    }

public void SpawnEnemy()
{
    if (!_spawnerReady || enemySpawnPoints.Count == 0) return;

    int randomIndex = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
    GameObject enemy = Instantiate(enemyPrefab, enemySpawnPoints[randomIndex].position, Quaternion.identity, enemiesParent.transform);
    enemies.Add(enemy);
}

public Enemy_Boss SpawnBoss(bool isFinalBoss)
{
    if (!_spawnerReady || enemySpawnPoints.Count == 0) return null;

    int randomIndex = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
    Vector3 pos = enemySpawnPoints[randomIndex].position + (isFinalBoss ? new Vector3(0, 2.5f, 0) : Vector3.zero);
    GameObject boss = Instantiate(bossPrefab, pos, Quaternion.identity, enemiesParent.transform);
    Enemy_Boss bossComponent = boss.GetComponent<Enemy_Boss>();
    bossComponent.IsFinalBoss = isFinalBoss; // Set whether it's a final boss
    enemies.Add(boss);
    return bossComponent;
}

       public void InitializeEnemySpawnPoints()
    {
        foreach (Transform child in enemySpawnPointsGameObject.transform)
        {
            enemySpawnPoints.Add(child);
        }
        HideSpawnPoints();
        _spawnerReady = true;
    }

    public void ClearEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies = new List<GameObject>();
    }

    public void SetSpawnRate(float spawnRate)
    {
        this.spawnRate = spawnRate;
    }

    public float GetSpawnRate()
    {
        return spawnRate;
    }

     public void HideSpawnPoints()
    {
        foreach (Transform spawnPoint in enemySpawnPoints)
        {
            spawnPoint.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public int GetBossCount()
    {
        return _boss_count;
    }

    public void ResetBossCount()
    {
        _boss_count = 0;
    }
}
